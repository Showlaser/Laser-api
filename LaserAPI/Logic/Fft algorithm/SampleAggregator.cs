using NAudio.Dsp;
using System;

namespace LaserAPI.Logic.Fft_algoritm
{
    public class SampleAggregator
    {
        public event EventHandler<FftEventArgs> FftCalculated;
        public bool PerformFft { get; set; }

        private readonly Complex[] _fftBuffer;
        private readonly FftEventArgs _fftArgs;
        private int _fftPos;
        private const int FftLength = 1024;
        private readonly int _m;

        public SampleAggregator()
        {
            if (!IsPowerOfTwo(FftLength))
            {
                throw new ArgumentException("Invalid FFT length");
            }
            _m = (int)Math.Log(FftLength, 2.0);
            _fftBuffer = new Complex[FftLength];
            _fftArgs = new FftEventArgs(_fftBuffer);
        }

        private static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        public void Add(float value)
        {
            if (!PerformFft || FftCalculated == null)
            {
                return;
            }

            _fftBuffer[_fftPos].X = (float)(value * FastFourierTransform.HannWindow(_fftPos, FftLength));
            _fftBuffer[_fftPos].Y = 0;
            _fftPos++;
            if (_fftPos < FftLength)
            {
                return;
            }

            _fftPos = 0;
            FastFourierTransform.FFT(true, _m, _fftBuffer);
            FftCalculated(this, _fftArgs);
        }
    }
}
