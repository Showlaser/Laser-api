using LaserAPI.Enums;
using LaserAPI.Logic.Fft_algorithm;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.FromFrontend.LasershowGenerator;
using LaserAPI.Models.Helper;
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
        private static readonly AudioAnalyser _audioAnalyser = new();
        private static double[] _spectrumData;
        private static SongData _songData = new();
        private static AlgorithmSettings _algorithmSettings = new();
        private static bool _fftIsActive;
        private static string _currentPlayingSongName = "";
        private static AnimationDto _generatedLaserShow = new();
        private static readonly Stopwatch Stopwatch = new();

        public static void Setup()
        {
            _generatedLaserShow.AnimationPatterns = [];
        }

        public static void SetSongData(SongData songData)
        {
            MusicGenre musicGenre = GetMusicGenreFromSpotifyGenre(songData.Genres);
            _algorithmSettings = GetAlgorithmSettingsByGenre(musicGenre);
            songData.MusicGenre = musicGenre;
            _songData = songData;
            HandleSongData();
        }

        private static void HandleSongData()
        {
            if (!_fftIsActive || !_songData.IsPlaying)
            {
                return;
            }

            try
            {
                bool songChanged = _currentPlayingSongName != _songData.SongName && !string.IsNullOrEmpty(_currentPlayingSongName);
                if (!_songData.IsPlaying && Stopwatch.IsRunning)
                {
                    Stopwatch.Stop();
                }
                else if (_songData.IsPlaying && !Stopwatch.IsRunning)
                {
                    Stopwatch.Start();
                }

                if (_songData.SaveLasershow && songChanged)
                {
                    Stopwatch.Restart();
                    _generatedLaserShow.Name = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()} generated show | {_songData.SongName}";
                    GeneratedLaserShowsQueue.LaserShowToSave = _generatedLaserShow;
                    _generatedLaserShow = new AnimationDto();
                }
            }
            finally
            {
                _currentPlayingSongName = _songData.SongName;
            }
        }

        public static MMDeviceCollection GetDevices()
        {
            return _audioAnalyser.GetDevices();
        }

        public static LaserGeneratorStatusViewmodel GetStatus => new(_fftIsActive,
            Enum.GetName(_songData.MusicGenre), _songData.Bpm);

        public static void Start(string deviceName)
        {
            MMDeviceCollection devices = _audioAnalyser.GetDevices();
            MMDevice device = devices.First(d => d.FriendlyName == deviceName);
            _audioAnalyser.Initialize(device);
            _audioAnalyser.Capture.DataAvailable += CaptureOnDataAvailable;
            _audioAnalyser.SpectrumCalculated += AudioAnalyserOnSpectrumCalculated;
            _audioAnalyser.Capture.StartRecording();
            _fftIsActive = true;
        }

        public static void Stop()
        {
            _audioAnalyser.SampleAggregator.PerformFft = false;
            _audioAnalyser.Capture.StopRecording();
            _songData = new SongData();
            _fftIsActive = false;
        }

        private static void CaptureOnDataAvailable(object sender, WaveInEventArgs audioEvent)
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

        private static void AudioAnalyserOnSpectrumCalculated(double[] calculatedData)
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

            bool displayAnimation = average > _algorithmSettings.Threshold + _songData.ThreshHoldOffset;
        }

        /// <summary>
        /// Gets the music genre from the genres. The most occurring genre will be returned.
        /// </summary>
        /// <returns>The most occurring genre</returns>
        public static MusicGenre GetMusicGenreFromSpotifyGenre(List<string> spotifyGenres)
        {
            List<MusicGenre> supportedGenres = Enum.GetValues<MusicGenre>().ToList();
            IEnumerable<MusicGenre> supportedGenresFromSpotify = spotifyGenres
                .SelectMany(sg => supportedGenres.FindAll(spg =>
                {
                    bool supportedGenreDetected = sg.ToLower().Contains(spg.ToString().ToLower());
                    return supportedGenreDetected;
                }));

            Dictionary<MusicGenre, int> genreOccurrenceDictionary = new();
            foreach (MusicGenre genre in supportedGenresFromSpotify)
            {
                if (!genreOccurrenceDictionary.ContainsKey(genre))
                {
                    genreOccurrenceDictionary.Add(genre, 0);
                }

                genreOccurrenceDictionary[genre]++;
            }

            return genreOccurrenceDictionary.Keys.Any() ?
                genreOccurrenceDictionary.MaxBy(god => god.Value).Key :
                MusicGenre.Unsupported;
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
                MusicGenre.Unsupported => 6,
                _ => throw new ArgumentOutOfRangeException(nameof(genre), genre, null)
            };
        }

        public static AlgorithmSettings GetAlgorithmSettingsByGenre(MusicGenre genre)
        {
            return genre.ToString().ToLower() switch
            {
                "hardstyle" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.018 },
                "hardcore" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.018 },
                "classic" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "techno" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "metal" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "trance" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "rock" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.01 },
                "house" => new AlgorithmSettings { FrequencyRange = new Range(2, 3), Threshold = 0.018 },
                _ => null
            };
        }
    }
}
