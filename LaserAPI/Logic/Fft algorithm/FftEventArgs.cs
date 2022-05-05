﻿using System.Diagnostics;
using NAudio.Dsp;

namespace LaserAPI.Logic.Fft_algorithm
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
