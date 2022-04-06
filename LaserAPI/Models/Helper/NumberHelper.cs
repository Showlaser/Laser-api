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

        public static bool IsBetweenOrEqualToWithMinMaxCheck(this int source, int nr1, int nr2)
        {
            return source >= GetLowestNumber(nr1, nr2) && source <= GetHighestNumber(nr1, nr2);
        }

        public static bool IsBetweenOrEqualTo(this int source, int min, int max)
        {
            return source >= min && source <= max;
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
            if (value == 0 && fromLow == 0 && fromHigh == 0 && toLow == 0 && toHigh == 0)
            {
                return 0;
            }
            return (value - fromLow) / (fromHigh - fromLow) * (toHigh - toLow) + toLow;
        }

        public static int GetDifferenceBetweenTwoNumbers(int nr1, int nr2)
        {
            return GetHighestNumber(nr1, nr2) - GetLowestNumber(nr1, nr2);
        }
    }
}
