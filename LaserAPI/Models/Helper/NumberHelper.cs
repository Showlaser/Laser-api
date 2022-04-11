using System;
using System.Data;

namespace LaserAPI.Models.Helper
{
    public static class NumberHelper
    {
        public static bool IsBetweenOrEqualTo(this object source, double min, double max)
        {
            double data = Convert.ToDouble(source);
            if (double.IsNaN(data))
            {
                throw new NoNullAllowedException(nameof(source));
            }

            return data >= min && data <= max;
        }

        public static bool IsBetweenOrEqualTo(this int source, int min, int max) =>
            source >= min && source <= max;

        public static int ToInt(this double source) => Convert.ToInt32(source);

        public static double ToDouble(this int source) => Convert.ToDouble(source);

        public static int GetHighestNumber(int nr1, int nr2) =>
            nr1 > nr2 ? nr1 : nr2;

        public static int GetLowestNumber(int nr1, int nr2) =>
            nr1 < nr2 ? nr1 : nr2;

        public static int Map(int value, int inMin, int inMax, int outMin, int outMax) =>
            (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;

        public static int CalculateDifference(int nr1, int nr2) =>
            GetHighestNumber(nr1, nr2) - GetLowestNumber(nr1, nr2);
    }
}
