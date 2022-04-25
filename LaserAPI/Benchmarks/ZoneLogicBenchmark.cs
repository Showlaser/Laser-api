using BenchmarkDotNet.Attributes;
using LaserAPI.Logic;
using LaserAPI.Models.Dto.Zones;
using LaserAPI.Models.Helper;

namespace LaserAPI.Benchmarks
{
    [MemoryDiagnoser]
    public class ZoneLogicBenchmark
    {
        private readonly ZoneDto _zone = new MockedZones().Zones[1];

        [Benchmark]
        public void GetLineHitByPathPerformanceTest()
        {
            LaserConnectionLogic.PreviousMessage = new LaserMessage(0, 0, 0, -4000, 0);
            for (int i = -4000; i < 4000; i++)
            {
                LaserMessage previousMessage = new(0, 0, 0, 0, i);
                ZoneLogic.GetZoneLineHitByPath(_zone, new LaserMessage(0, 0, 0, 4000, 4000), previousMessage);
            }
        }
    }
}
