using LaserAPI.Enums;
using LaserAPI.Logic.Fft_algorithm;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.FromFrontend.LasershowGenerator;
using LaserAPI.Models.Helper;
using LaserAPI.Models.Helper.LaserAnimations;
using LaserAPI.Models.ToFrontend.LasershowGenerator;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LaserAPI.Logic
{
    public class LaserShowGeneratorAlgorithm
    {
        private readonly AudioAnalyser _audioAnalyser;
        public AnimationLogic AnimationLogic { get; set; }
        private double[] _spectrumData;
        private SongData _songData = new();
        private AlgorithmSettings _algorithmSettings = new();
        private bool _fftIsActive;
        private readonly List<IPreMadeLaserAnimation> _animations = new();
        private string _currentPlayingSongName = "";
        private readonly AnimationDto _generatedLasershow = new();
        private readonly Stopwatch _stopwatch = new();

        public LaserShowGeneratorAlgorithm(AudioAnalyser audioAnalyser)
        {
            _generatedLasershow.PatternAnimations = new List<PatternAnimationDto>();
            _audioAnalyser = audioAnalyser;
            List<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(e => e.GetTypes())
                .Where(e => typeof(IPreMadeLaserAnimation).IsAssignableFrom(e) && e.IsClass)
                .ToList();

            _animations.AddRange(types.Select(t => (IPreMadeLaserAnimation)Activator.CreateInstance(t)));
        }

        public void SetSongData(SongData songData)
        {
            MusicGenre musicGenre = GetMusicGenreFromSpotifyGenre(songData.Genres);
            _algorithmSettings = GetAlgorithmSettingsByGenre(musicGenre);
            songData.MusicGenre = musicGenre;
            _songData = songData;
            HandleSongData();
        }

        private void HandleSongData()
        {
            if (!_fftIsActive || !_songData.IsPlaying)
            {
                return;
            }

            try
            {
                bool songChanged = _currentPlayingSongName != _songData.SongName && !string.IsNullOrEmpty(_currentPlayingSongName);
                if (_songData.SaveLasershow && !_stopwatch.IsRunning)
                {
                    _stopwatch.Start();
                }

                if (!_songData.IsPlaying && _stopwatch.IsRunning)
                {
                    _stopwatch.Stop();
                }
                else if (_songData.IsPlaying && !_stopwatch.IsRunning)
                {
                    _stopwatch.Start();
                }

                if (_songData.SaveLasershow && songChanged)
                {
                    _stopwatch.Restart();
                    _generatedLasershow.Name = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} generated show | {_songData.SongName}";
                    AnimationLogic.AddOrUpdate(_generatedLasershow).Wait();
                }
            }
            finally
            {
                _currentPlayingSongName = _songData.SongName;
            }
        }

        public MMDeviceCollection GetDevices()
        {
            return _audioAnalyser.GetDevices();
        }

        public LaserGeneratorStatusViewmodel GetStatus => new(_fftIsActive,
            Enum.GetName(_songData.MusicGenre), _songData.Bpm);

        public void Start(string deviceName)
        {
            MMDeviceCollection devices = _audioAnalyser.GetDevices();
            MMDevice device = devices.First(d => d.FriendlyName == deviceName);
            _audioAnalyser.Initialize(device);
            _audioAnalyser.Capture.DataAvailable += CaptureOnDataAvailable;
            _audioAnalyser.SpectrumCalculated += AudioAnalyserOnSpectrumCalculated;
            _audioAnalyser.Capture.StartRecording();
            _fftIsActive = true;
        }

        public void Stop()
        {
            _audioAnalyser.SampleAggregator.PerformFft = false;
            _audioAnalyser.Capture.StopRecording();
            _songData = new SongData();
            _fftIsActive = false;
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
            if (_algorithmSettings == null)
            {
                return;
            }

            _spectrumData = calculatedData;
            int spectrumDataLength = calculatedData.Length;
            List<double> frequencyRangeValues = new();
            double average = 0;

            for (int i = 0; i < spectrumDataLength; i++)
            {
                bool dataIsBetweenFrequencyRange = i.IsBetweenOrEqualTo(_algorithmSettings.FrequencyRange.Start.Value,
                    _algorithmSettings.FrequencyRange.End.Value);
                if (dataIsBetweenFrequencyRange)
                {
                    average += _spectrumData[i];
                    frequencyRangeValues.Add(_spectrumData[i]);
                }
            }

            average /= frequencyRangeValues.Count;

            bool displayAnimation = average > _algorithmSettings.Threshold;
            if (displayAnimation && LaserConnectionLogic.LaserIsAvailable())
            {
                OnAnimationDisplay();
            }
        }

        private void OnAnimationDisplay()
        {
            AnimationDto animation = GenerateLaserAnimation();
            AnimationLogic.PlayAnimation(animation);

            if (_songData.SaveLasershow)
            {
                PatternAnimationDto patternAnimation = animation.PatternAnimations[0];
                patternAnimation.StartTimeOffset = Convert.ToInt32(_stopwatch.ElapsedMilliseconds);
                _generatedLasershow.PatternAnimations.Add(patternAnimation);
            }
        }

        private AnimationDto GenerateLaserAnimation()
        {
            PreMadeAnimationOptions options = new()
            {
                CenterX = new Random(Guid.NewGuid().GetHashCode()).Next(-1000, 1000),
                CenterY = new Random(Guid.NewGuid().GetHashCode()).Next(-1000, 1000),
                Rotation = new Random(Guid.NewGuid().GetHashCode()).Next(0, 361),
                Scale = NumberHelper.GetRandomDouble(0.4, 1)
            };

            int patternIndex = new Random(Guid.NewGuid().GetHashCode()).Next(0, _animations.Count);
            IPreMadeLaserAnimation preMadeAnimation = _animations[patternIndex];
            preMadeAnimation.Speed = GetSpeedFromGenre(_songData.MusicGenre);

            return preMadeAnimation.GetAnimation(options);
        }

        /// <summary>
        /// Gets the music genre from the genres. The most occurring genre will be returned.
        /// </summary>
        /// <param name="spotifyMusicGenres">The music genre collection from Spotify</param>
        /// <returns>The most occurring genre</returns>
        public static MusicGenre GetMusicGenreFromSpotifyGenre(List<string> spotifyMusicGenres)
        {
            string[] genres = Enum.GetNames(typeof(MusicGenre));
            int genresLength = genres.Length;

            List<GenresSorter> genreOccurrences = new();
            int spotifyMusicGenresLength = spotifyMusicGenres.Count;
            for (int i = 0; i < genresLength; i++)
            {
                genres[i] = genres[i].ToLower();
                string genre = genres[i];
                for (int j = 0; j < spotifyMusicGenresLength; j++)
                {
                    string spotifyMusicGenre = spotifyMusicGenres[j].ToLower();
                    if (!spotifyMusicGenre.Contains(genre))
                    {
                        continue;
                    }

                    int sorterIndex = genreOccurrences.FindIndex(g => g.Genre == genre);
                    if (sorterIndex == -1)
                    {
                        genreOccurrences.Add(new GenresSorter(genre, 1));
                    }
                    else
                    {
                        genreOccurrences[sorterIndex].Occurrences++;
                    }
                }
            }

            GenresSorter sorter = genreOccurrences.MaxBy(go => go.Occurrences);
            if (sorter == null)
            {
                return default;
            }

            return (MusicGenre)Enum.Parse(typeof(MusicGenre), sorter.Genre, true);
        }

        private static int GetSpeedFromGenre(MusicGenre genre)
        {
            return genre switch
            {
                MusicGenre.Hardstyle => 15,
                MusicGenre.Hardcore => 15,
                MusicGenre.Classic => 3,
                MusicGenre.Techno => 7,
                MusicGenre.Metal => 6,
                MusicGenre.Trance => 7,
                MusicGenre.Rock => 6,
                MusicGenre.House => 6,
                _ => throw new ArgumentOutOfRangeException(nameof(genre), genre, null)
            };
        }

        public static AlgorithmSettings GetAlgorithmSettingsByGenre(MusicGenre genre)
        {
            return genre.ToString().ToLower() switch
            {
                "hardstyle" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.015 },
                "hardcore" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.015 },
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
