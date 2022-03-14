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
using System.Threading.Tasks;

namespace LaserAPI.Logic
{
    public class LaserShowGeneratorLogic
    {
        private readonly AudioAnalyser _audioAnalyser;
        private readonly AnimationLogic _animationLogic;
        private double[] _spectrumData;
        private SongData _songData;
        private Task _playAnimationTask;

        public LaserShowGeneratorLogic(AudioAnalyser audioAnalyser, AnimationLogic animationLogic)
        {
            _audioAnalyser = audioAnalyser;
            _animationLogic = animationLogic;
        }

        public void SetSongData(SongData songData)
        {
            MusicGenre musicGenre = MusicGenreHelper.GetMusicGenreFromSpotifyGenre(songData.Genres);
            songData.FrequencyRange = FftHelper.GetFftFrequencyRangeByGenre(musicGenre);
            _songData = songData;
        }

        public void Start(int deviceIndex)
        {
            MMDeviceCollection devices = _audioAnalyser.GetDevices();
            _audioAnalyser.Initialize(devices[deviceIndex]);
            _audioAnalyser.Capture.DataAvailable += CaptureOnDataAvailable;
            _audioAnalyser.Capture.StartRecording();
            _audioAnalyser.SpectrumCalculated += AudioAnalyserOnSpectrumCalculated;
            _audioAnalyser.SampleAggregator.PerformFft = true;
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
                _spectrumData[i] *= Math.Log(i + 2, 10);
                average += _spectrumData[i];
                if (i.IsBetweenOrEqualTo(_songData.FrequencyRange.Start.Value, _songData.FrequencyRange.End.Value))
                {
                    frequencyRangeValues.Add(_spectrumData[i]);
                }
            }

            double threshold = 2;
            if (average > threshold && _playAnimationTask.IsCompleted)
            {
                AnimationDto animation = GenerateLaserAnimation();
                _playAnimationTask = new Task(() => _animationLogic.PlayAnimation(animation), TaskCreationOptions.LongRunning);
                _playAnimationTask.Start();
            }
        }

        private static AnimationDto GenerateLaserAnimation()
        {
            int centerX = new Random(Guid.NewGuid().GetHashCode()).Next(-1000, 1000);
            int centerY = new Random(Guid.NewGuid().GetHashCode()).Next(-1000, 1000);
            int rotation = new Random(Guid.NewGuid().GetHashCode()).Next(0, 361);
            double scale = new Random(Guid.NewGuid().GetHashCode()).NextDouble();

            PreMadeAnimations preMadeAnimations = new();
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
