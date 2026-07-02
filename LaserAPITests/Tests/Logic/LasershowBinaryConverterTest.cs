using System;
using LaserAPI.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LaserAPITests.Tests.Logic
{
    [TestClass]
    public class LasershowBinaryConverterTest
    {
        // One frame, one path, two points, as the exported file wraps it.
        private const string WrappedTwoPointShow =
            "{\"kpps\":40000,\"laserCommands\":[{\"timeMs\":0,\"commands\":[[" +
            "{\"r\":193,\"g\":21,\"b\":21,\"x\":0,\"y\":2000}," +
            "{\"r\":1,\"g\":2,\"b\":3,\"x\":-2000,\"y\":-1999}" +
            "]]}]}";

        // The same show as the frontend's "save to SD" sends it: a bare array of
        // frames with no kpps wrapper. Should default kpps to 40000, so it yields
        // byte-for-byte the same output as WrappedTwoPointShow.
        private const string BareArrayTwoPointShow =
            "[{\"timeMs\":0,\"commands\":[[" +
            "{\"r\":193,\"g\":21,\"b\":21,\"x\":0,\"y\":2000}," +
            "{\"r\":1,\"g\":2,\"b\":3,\"x\":-2000,\"y\":-1999}" +
            "]]}]";

        private static byte[] ExpectedTwoPointBytes()
        {
            return
            [
                // Header: "LZS1", version 1, reserved 0, kpps 40000, frameCount 1
                0x4C, 0x5A, 0x53, 0x31,
                0x01, 0x00,
                0x00, 0x00,
                0x40, 0x9C, 0x00, 0x00,
                0x01, 0x00, 0x00, 0x00,
                // Frame: durationMs 10, pathCount 1
                0x0A, 0x00,
                0x01, 0x00,
                // Path: pointCount 2
                0x02, 0x00,
                // Point 0: x 0, y 2000, rgb 193,21,21
                0x00, 0x00, 0xD0, 0x07, 0xC1, 0x15, 0x15,
                // Point 1: x -2000, y -1999, rgb 1,2,3
                0x30, 0xF8, 0x31, 0xF8, 0x01, 0x02, 0x03,
            ];
        }

        [TestMethod]
        public void JsonToBinaryWrappedObjectProducesExpectedLittleEndianBytes()
        {
            byte[] actual = LasershowBinaryConverter.JsonToBinary(WrappedTwoPointShow);
            CollectionAssert.AreEqual(ExpectedTwoPointBytes(), actual);
        }

        [TestMethod]
        public void JsonToBinaryBareFrameArrayDefaultsKppsAndMatches()
        {
            // The bare array (real SD-upload payload) must produce the same bytes,
            // proving both input shapes are accepted and kpps defaults to 40000.
            byte[] actual = LasershowBinaryConverter.JsonToBinary(BareArrayTwoPointShow);
            CollectionAssert.AreEqual(ExpectedTwoPointBytes(), actual);
        }

        [TestMethod]
        public void JsonToBinaryThrowsOnEmptyInput()
        {
            Assert.Throws<ArgumentException>(() => LasershowBinaryConverter.JsonToBinary("  "));
        }

        [TestMethod]
        public void JsonToBinaryThrowsOnUnparseableInput()
        {
            Assert.Throws<ArgumentException>(() => LasershowBinaryConverter.JsonToBinary("not json"));
        }

        [TestMethod]
        public void JsonToBinaryClampsCoordinatesToInt16Range()
        {
            // x well above int16 max must clamp to short.MaxValue rather than wrap.
            const string outOfRangeShow =
                "[{\"timeMs\":0,\"commands\":[[" +
                "{\"r\":0,\"g\":0,\"b\":0,\"x\":40000,\"y\":0}]]}]";

            byte[] actual = LasershowBinaryConverter.JsonToBinary(outOfRangeShow);

            // Header(16) + durationMs(2) + pathCount(2) + pointCount(2) => x at offset 22.
            short x = BitConverter.ToInt16(actual, 22);
            Assert.AreEqual(short.MaxValue, x);
        }
    }
}
