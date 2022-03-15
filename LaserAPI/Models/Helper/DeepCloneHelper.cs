using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace LaserAPI.Models.Helper
{
    public static class DeepCloneHelper
    {
        public static T DeepClone<T>(this T obj)
        {
            using MemoryStream memStream = new();
            BinaryFormatter bFormatter = new();
            bFormatter.Serialize(memStream, obj);
            memStream.Position = 0;

            return (T)bFormatter.Deserialize(memStream);
        }
    }
}
