using BenchmarkDotNet.Running;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace LaserAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkDotNet.Reports.Summary[]? summary = BenchmarkRunner.Run(typeof(Program).Assembly);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
