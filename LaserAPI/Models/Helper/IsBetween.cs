using System.Data;

namespace LaserAPI.Models.Helper
{
    public static class IsBetween
    {
        public static bool IsBetweenOrEqualTo(this object source, double min, double max)
        {
            var data = (double)source;
            if (double.IsNaN(data))
            {
                throw new NoNullAllowedException(nameof(source));
            }

            return data >= min && data <= max;
        }
    }
}
