using NAudio.Dsp;
using System;

namespace LaserAPI.Logic.Fft_algoritm
{
    public class SampleAggregator
    {
        public event EventHandler<FftEventArgs> FftCalculated;
        public bool PerformFFT { get; set; }

        private readonly Complex[] fftBuffer;
        private readonly FftEventArgs fftArgs;
        private int fftPos;
        private readonly int fftLength;
        private readonly int m;

        public SampleAggregator()
        {
            if (!IsPowerOfTwo(fftLength))
            {
                throw new ArgumentException("Invalid FFT length");
            }
            m = (int)Math.Log(fftLength, 2.0);
            fftBuffer = new Complex[fftLength];
            fftArgs = new FftEventArgs(fftBuffer);
        }

        static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        public void Add(float value)
        {
            if (!PerformFFT || FftCalculated == null)
            {
                return;
            }

            fftBuffer[fftPos].X = (float)(value * FastFourierTransform.HannWindow(fftPos, fftLength));
            fftBuffer[fftPos].Y = 0;
            fftPos++;
            if (fftPos < fftLength)
            {
                return;
            }

            fftPos = 0;
            FastFourierTransform.FFT(true, m, fftBuffer);
            FftCalculated(this, fftArgs);
        }
    }
}
