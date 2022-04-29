using LaserAPI.Enums;
using LaserAPI.Logic.Fft_algorithm;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.FromFrontend.LasershowGenerator;
using LaserAPI.Models.Helper;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Logic
{
    public class LaserShowGeneratorLogic
    {
        private readonly AudioAnalyser _audioAnalyser;
        private readonly AnimationLogic _animationLogic;
        private double[] _spectrumData;
        private SongData _songData = new();
        private AlgorithmSettings _algorithmSettings = new();

        public LaserShowGeneratorLogic(AudioAnalyser audioAnalyser, AnimationLogic animationLogic)
        {
            _audioAnalyser = audioAnalyser;
            _animationLogic = animationLogic;
        }

        public void SetSongData(SongData songData)
        {
            MusicGenre musicGenre = GetMusicGenreFromSpotifyGenre(songData.Genres);
            _algorithmSettings = GetAlgorithmSettingsByGenre(musicGenre);
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
            if (average > threshold)
            {
                AnimationDto animation = GenerateLaserAnimation();
                _animationLogic.PlayAnimation(animation).Wait();
            }
        }

        private AnimationDto GenerateLaserAnimation()
        {
            int centerX = new Random(Guid.NewGuid().GetHashCode()).Next(-1000, 1000);
            int centerY = new Random(Guid.NewGuid().GetHashCode()).Next(-1000, 1000);
            int rotation = new Random(Guid.NewGuid().GetHashCode()).Next(0, 361);
            double scale = new Random(Guid.NewGuid().GetHashCode()).NextDouble();

            PreMadeAnimations preMadeAnimations = new((int)_songData.MusicGenre);
            int patternIndex = new Random(Guid.NewGuid().GetHashCode()).Next(0, 1);
            AnimationDto animation = patternIndex switch
            {
                0 => preMadeAnimations.LineAnimation(centerX, centerY, rotation, scale)
            };

            return animation;
        }

        public static MusicGenre GetMusicGenreFromSpotifyGenre(List<string> spotifyMusicGenres)
        {
            string[] genres = Enum.GetNames(typeof(Enums.MusicGenre));
            int genresLength = genres.Length;
            for (int i = 0; i < genresLength; i++)
            {
                genres[i] = genres[i].ToLower();
            }

            int spotifyMusicGenresLength = spotifyMusicGenres.Count;
            for (int i = 0; i < genresLength; i++)
            {
                string genre = genres[i];
                for (int j = 0; j < spotifyMusicGenresLength; j++)
                {
                    string spotifyMusicGenre = spotifyMusicGenres[j].ToLower();
                    if (spotifyMusicGenre.Contains(genre))
                    {
                        return (Enums.MusicGenre)Enum.Parse(typeof(Enums.MusicGenre), genre, true);
                    }
                }
            }

            return default;
        }

        public static AlgorithmSettings GetAlgorithmSettingsByGenre(Enums.MusicGenre genre)
        {
            return genre.ToString().ToLower() switch
            {
                "hardstyle" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.008 },
                "hardcore" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.008 },
                "classic" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "techno" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "metal" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "trance" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "rock" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "house" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                _ => null
            };
        }
    }
}
