using System;
using System.Data;

namespace LaserAPI.Models.Helper
{
    public static class NumberHelper
    {
        public static bool IsBetweenOrEqualTo(this object source, double min, double max)
        {
            var data = Convert.ToDouble(source);
            if (double.IsNaN(data))
            {
                throw new NoNullAllowedException(nameof(source));
            }

            return data >= min && data <= max;
        }

        public static bool IsBetweenOrEqualTo(this object source, int min, int max)
        {
            var data = Convert.ToInt32(source);
            return data >= min && data <= max;
        }

        public static int GetHighestNumber(int nr1, int nr2)
        {
            return nr1 > nr2 ? nr1 : nr2;
        }

        public static int GetLowestNumber(int nr1, int nr2)
        {
            return nr1 < nr2 ? nr1 : nr2;
        }

        public static int Map(int value, int fromLow, int fromHigh, int toLow, int toHigh)
        {
            return (value - fromLow) / (fromHigh - fromLow) * (toHigh - toLow) + toLow;
        }
    }
}
