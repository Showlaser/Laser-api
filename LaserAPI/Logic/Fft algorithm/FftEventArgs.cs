using NAudio.Dsp;
using System.Diagnostics;

namespace LaserAPI.Logic.Fft_algoritm
{
    public class FftEventArgs
    {
        [DebuggerStepThrough]
        public FftEventArgs(Complex[] result)
        {
            Result = result;
        }
        public Complex[] Result { get; }
    }
}
