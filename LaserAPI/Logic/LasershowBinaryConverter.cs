using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LaserAPI.Logic
{
    /// <summary>
    /// Converts an exported lasershow (the JSON the frontend produces, shaped as
    /// { "kpps": ..., "laserCommands": [ { "timeMs": ..., "commands": [[ {r,g,b,x,y} ]] } ] })
    /// into the compact ".lzs" binary the showlaser streams from its SD card.
    ///
    /// This exists so the Teensy never has to deserialize a multi-KB JSON document
    /// into RAM: the API converts once and the laser writes/streams raw bytes.
    ///
    /// Format "LZS1" (all little-endian, which both this writer and the showlaser's
    /// Cortex-M7 reader use natively, so the firmware can read fields directly):
    ///   Header: char[4] "LZS1" | u16 version=1 | u16 reserved | u32 kpps | u32 frameCount
    ///   Frame : u16 durationMs | u16 pathCount
    ///           per path: u16 pointCount | point[]: i16 x, i16 y, u8 r, u8 g, u8 b
    /// </summary>
    public static class LasershowBinaryConverter
    {
        private const int DefaultFrameDurationMs = 10;

        // The frontend's "save to SD" payload carries no kpps (it sends only the
        // frame array), so fall back to this. Matches the UI's default galvo speed.
        private const int DefaultKpps = 40000;

        /// <summary>
        /// Converts an exported lasershow JSON string into the ".lzs" binary format.
        /// </summary>
        public static byte[] JsonToBinary(string showJson)
        {
            if (string.IsNullOrWhiteSpace(showJson))
            {
                throw new ArgumentException("Show JSON is empty", nameof(showJson));
            }

            JToken root;
            try
            {
                root = JToken.Parse(showJson);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Show JSON could not be parsed", nameof(showJson), ex);
            }

            // Accept both shapes: the frontend's SD upload sends a bare array of
            // frames, while the exported .json file wraps it as {kpps, laserCommands}.
            int kpps;
            JToken framesToken;
            if (root is JArray)
            {
                kpps = DefaultKpps; // no kpps travels with the bare-array payload
                framesToken = root;
            }
            else
            {
                int parsedKpps = root["kpps"]?.Value<int?>() ?? 0;
                kpps = parsedKpps > 0 ? parsedKpps : DefaultKpps;
                framesToken = root["laserCommands"];
            }

            List<FrameJsonModel> frames = framesToken?.ToObject<List<FrameJsonModel>>() ?? [];

            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream, Encoding.ASCII, leaveOpen: false);

            // --- Header --- (BinaryWriter is always little-endian)
            writer.Write(Encoding.ASCII.GetBytes("LZS1")); // raw 4 bytes, no length prefix
            writer.Write((ushort)1);                        // version
            writer.Write((ushort)0);                        // reserved
            writer.Write((uint)kpps);
            writer.Write((uint)frames.Count);

            // --- Frames ---
            for (int i = 0; i < frames.Count; i++)
            {
                FrameJsonModel frame = frames[i];
                writer.Write((ushort)FrameDurationMs(frames, i));

                List<List<PointJsonModel>> paths = frame.Commands ?? [];
                writer.Write(ToUInt16Count(paths.Count, "paths in a frame"));

                foreach (List<PointJsonModel> path in paths)
                {
                    List<PointJsonModel> points = path ?? [];
                    writer.Write(ToUInt16Count(points.Count, "points in a path"));

                    foreach (PointJsonModel p in points)
                    {
                        writer.Write(ClampInt16(p.X));
                        writer.Write(ClampInt16(p.Y));
                        writer.Write(ClampByte(p.R));
                        writer.Write(ClampByte(p.G));
                        writer.Write(ClampByte(p.B));
                    }
                }
            }

            writer.Flush();
            return stream.ToArray();
        }

        /// <summary>
        /// Frame length = gap to the next frame's timeMs. The last frame reuses the
        /// previous gap (there is no "next" to measure), falling back to a default.
        /// </summary>
        private static int FrameDurationMs(List<FrameJsonModel> frames, int index)
        {
            if (index + 1 < frames.Count)
            {
                int gap = frames[index + 1].TimeMs - frames[index].TimeMs;
                if (gap > 0)
                {
                    return Math.Min(gap, ushort.MaxValue);
                }
            }
            else if (index > 0)
            {
                int gap = frames[index].TimeMs - frames[index - 1].TimeMs;
                if (gap > 0)
                {
                    return Math.Min(gap, ushort.MaxValue);
                }
            }

            return DefaultFrameDurationMs;
        }

        /// <summary>
        /// A count that must fit the format's u16 fields. Throws loudly rather than
        /// silently truncating the count while still writing every element (which
        /// would desync the firmware reader and corrupt the rest of the file).
        /// </summary>
        private static ushort ToUInt16Count(int count, string what)
        {
            if (count > ushort.MaxValue)
            {
                throw new NotSupportedException(
                    $"Too many {what}: {count} exceeds the {ushort.MaxValue} the .lzs format allows");
            }

            return (ushort)count;
        }

        private static short ClampInt16(int value)
        {
            if (value < short.MinValue)
            {
                return short.MinValue;
            }

            if (value > short.MaxValue)
            {
                return short.MaxValue;
            }

            return (short)value;
        }

        private static byte ClampByte(int value)
        {
            if (value < 0)
            {
                return 0;
            }

            if (value > byte.MaxValue)
            {
                return byte.MaxValue;
            }

            return (byte)value;
        }

        private sealed class FrameJsonModel
        {
            [JsonProperty("timeMs")] public int TimeMs { get; set; }
            [JsonProperty("commands")] public List<List<PointJsonModel>> Commands { get; set; }
        }

        private sealed class PointJsonModel
        {
            [JsonProperty("r")] public int R { get; set; }
            [JsonProperty("g")] public int G { get; set; }
            [JsonProperty("b")] public int B { get; set; }
            [JsonProperty("x")] public int X { get; set; }
            [JsonProperty("y")] public int Y { get; set; }
        }
    }
}
