using LaserAPI.Enums;
using LaserAPI.Logic.Fft_algorithm;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.FromFrontend.LasershowGenerator;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.FftHelper;
using LaserAPI.Models.Helper.MusicGenre;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LaserShowGeneratorLogic
    {
        private readonly AudioAnalyser _audioAnalyser;
        private double[] _spectrumData;
        private SongData _songData = new();
        private AlgorithmSettings _algorithmSettings = new();
        private Task _playAnimationTask;

        public LaserShowGeneratorLogic(AudioAnalyser audioAnalyser)
        {
            _audioAnalyser = audioAnalyser;
        }

        public void SetSongData(SongData songData)
        {
            MusicGenre musicGenre = MusicGenreHelper.GetMusicGenreFromSpotifyGenre(songData.Genres);
            _algorithmSettings = FftHelper.GetAlgorithmSettingsByGenre(musicGenre);
            songData.MusicGenre = musicGenre;
            _songData = songData;
        }

        public MMDeviceCollection GetDevices()
        {
            return _audioAnalyser.GetDevices();
        }

        public void Start(string deviceName)
        {
            MMDeviceCollection devices = _audioAnalyser.GetDevices();
            MMDevice device = devices.First(d => d.FriendlyName == deviceName);
            _audioAnalyser.Initialize(device);
            _audioAnalyser.Capture.DataAvailable += CaptureOnDataAvailable;
            _audioAnalyser.SpectrumCalculated += AudioAnalyserOnSpectrumCalculated;
            _audioAnalyser.Capture.StartRecording();
        }

        public void Stop()
        {
            _audioAnalyser.SampleAggregator.PerformFft = false;
            _audioAnalyser.Capture.StopRecording();
        }

        private void CaptureOnDataAvailable(object sender, WaveInEventArgs audioEvent)
        {
            byte[] buffer = audioEvent.Buffer;
            int bytesRecorded = audioEvent.BytesRecorded;
            int bufferIncrement = _audioAnalyser.Capture.WaveFormat.BlockAlign;

            for (int index = 0; index < bytesRecorded; index += bufferIncrement)
            {
                float sample32 = BitConverter.ToSingle(buffer, index);
                _audioAnalyser.SampleAggregator.Add(sample32);
            }
        }

        private void AudioAnalyserOnSpectrumCalculated(double[] calculatedData)
        {
            _spectrumData = calculatedData;
            int spectrumDataLength = calculatedData.Length;
            List<double> frequencyRangeValues = new();
            double average = 0;

            for (int i = 0; i < spectrumDataLength; i++)
            {
                if (i.IsBetweenOrEqualTo(_algorithmSettings.FrequencyRange.Start.Value, _algorithmSettings.FrequencyRange.End.Value))
                {
                    average += _spectrumData[i];
                    frequencyRangeValues.Add(_spectrumData[i]);
                }
            }

            average /= frequencyRangeValues.Count;

            double threshold = _algorithmSettings.Threshold;
            bool taskAvailable = _playAnimationTask == null || _playAnimationTask.IsCompleted;

            if (average > threshold && taskAvailable)
            {
                AnimationDto animation = GenerateLaserAnimation();
                _playAnimationTask = new Task(() => PlayerHelper.PlayAnimation(animation).Wait(), TaskCreationOptions.LongRunning);
                _playAnimationTask.Start();
            }
        }

        private AnimationDto GenerateLaserAnimation()
        {
            int centerX = new Random(Guid.NewGuid().GetHashCode()).Next(-1000, 1000);
            int centerY = new Random(Guid.NewGuid().GetHashCode()).Next(-1000, 1000);
            int rotation = new Random(Guid.NewGuid().GetHashCode()).Next(0, 361);
            double scale = new Random(Guid.NewGuid().GetHashCode()).NextDouble();

            PreMadeAnimations preMadeAnimations = new((int)_songData.MusicGenre);
            int patternIndex = new Random(Guid.NewGuid().GetHashCode()).Next(0, 3);
            AnimationDto animation = patternIndex switch
            {
                0 => preMadeAnimations.LineAnimation(centerX, centerY, rotation, scale),
                1 => preMadeAnimations.RandomPoints(centerX, centerY, rotation, scale),
                _ => preMadeAnimations.LineAnimation(centerX, centerY, rotation, scale)
            };

            return animation;
        }
    }
}
