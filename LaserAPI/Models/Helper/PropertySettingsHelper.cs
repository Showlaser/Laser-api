using System.Collections.Generic;

namespace LaserAPI.Models.Helper
{
    public enum PropertyEdited
    {
        Scale,
        XOffset,
        YOffset,
        Rotation
    }

    public enum PropertyType
    {
        Float,
        Int
    }

    public struct PropertySetting
    {
        public PropertyEdited PropertyEdited { get; set; }
        public PropertyType Type { get; set; }
        public int DefaultValue { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }
    }

    public static class PropertySettinsHelper
    {
        public static readonly List<PropertySetting> PropertySettings =
        [
            new()
            {
                PropertyEdited = PropertyEdited.Scale,
                Type = PropertyType.Float,
                DefaultValue = 1,
                Min = 0.1,
                Max = 10
            },
            new()
            {
                PropertyEdited = PropertyEdited.XOffset,
                Type = PropertyType.Int,
                DefaultValue = 0,
                Min = -200
            }
        ];
    }
}
