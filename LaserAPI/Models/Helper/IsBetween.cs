using System;
using System.Data;

namespace LaserAPI.Models.Helper
{
    public static class IsBetween
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
    }
}
