using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LaserAPI.Logic.Fft_algorithm
{
    public class AudioAnalyser
    {
        public event Action<double[]> SpectrumCalculated;

        public WasapiCapture Capture { get; private set; }
        public SampleAggregator SampleAggregator { get; private set; }
        public MMDevice AudioDevice { get; set; }
        private readonly Queue<double[]> _dataBuffer = new();

        public MMDeviceCollection GetDevices()
        {
            MMDeviceEnumerator enumerator = new();
            MMDeviceCollection devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            return devices;
        }

        public void Initialize(MMDevice audioDevice)
        {
            AudioDevice = audioDevice;
            Capture = new WasapiLoopbackCapture();
            SampleAggregator = new SampleAggregator();
            SampleAggregator.FftCalculated += FftCalculated;
            SampleAggregator.PerformFft = true;
        }

        void FftCalculated(object sender, FftEventArgs fftEvent)
        {
            int bufferLength = 5;
            double[] spectrumData = CalculateSpectrum(fftEvent.Result);
            int spectrumDataLength = spectrumData.Length;

            _dataBuffer.Enqueue(spectrumData);
            if (_dataBuffer.Count < bufferLength)
            {
                return;
            }

            double[] average = new double[spectrumDataLength];
            for (int i = 0; i < spectrumDataLength; i++)
            {
                average[i] = _dataBuffer.Sum(x => x[i]) / _dataBuffer.Count;
            }

            while (_dataBuffer.Count != bufferLength)
            {
                _dataBuffer.Dequeue();
            }

            SpectrumCalculated.Invoke(average);
        }

        private static double[] CalculateSpectrum(IList<Complex> fftResult)
        {
            int fftResultLength = fftResult.Count;
            double[] data = new double[fftResultLength];
            for (int i = 0; i < fftResultLength; i++)
            {
                double magnitude = Math.Sqrt(fftResult[i].X * fftResult[i].X + fftResult[i].Y * fftResult[i].Y);
                data[i] = magnitude;
            }

            return data;
        }
    }
}
