using LaserAPI.Dal;
using LaserAPI.Interfaces;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Logic;
using LaserAPI.Logic.Fft_algorithm;
using LaserAPI.Logic.Game;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.FromLaser;
using LaserAPI.Models.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace LaserAPI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            string connectionString = "Data Source=Application.db;Cache=Shared";
            services.AddDbContextPool<DataContext>(
                dbContextOptions => dbContextOptions
                    .UseSqlite(connectionString, o =>
                        o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
            services
                .AddControllers()
                .AddJsonOptions(opts =>
                {
                    JsonStringEnumConverter enumConverter = new();
                    opts.JsonSerializerOptions.Converters.Add(enumConverter);
                });

            AddDependencyInjection(ref services);
        }

        private void ListenForBroadcasts()
        {
            int listenPort = 8888;
            using UdpClient udpClient = new(listenPort);
            IPEndPoint remoteEndPoint = new(IPAddress.Any, 0);

            Console.WriteLine("Make sure you have opened port 8888 and 5004 on your Windows firewall to allow the showlasers to communicate with the controller.\n" +
                "Execute the following commands in PowerShell with Administrator rights to open the ports:");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("New-NetFirewallRule -DisplayName \"LaserAPI UDP 8888\" -Direction Inbound -Protocol UDP -LocalPort 8888 -Action Allow -Profile Any");
            Console.WriteLine("New-NetFirewallRule -DisplayName \"LaserAPI 5004\" -Direction Inbound -Protocol TCP -LocalPort 5004 -Action Allow -Profile Private,Public \n");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine($"Listening for UDP broadcasts on port {listenPort}...");

            while (true)
            {
                try
                {
                    byte[] receivedBytes = udpClient.Receive(ref remoteEndPoint);
                    string message = Encoding.UTF8.GetString(receivedBytes);
                    UDPBroadcast broadcast = Newtonsoft.Json.JsonConvert.DeserializeObject<UDPBroadcast>(message);

                    if (!LaserConnectionLogicState.AdoptionPending.Exists(ap => ap.Uuid == broadcast.Uuid) && !LaserConnectionLogicState.RegisteredLasers.Exists(rl => rl.Uuid == broadcast.Uuid))
                    {
                        LaserConnectionLogicState.AdoptionPending.Add(broadcast);
                    }

                    Console.WriteLine($"[{DateTime.Now}] {remoteEndPoint}: {message}");
                }
                catch (Exception ex)
                {
                    // Keep listening: a single malformed packet or transient socket
                    // error must not tear down the discovery loop (it runs on a
                    // fire-and-forget Task, so a rethrow would silently stop discovery).
                    Console.WriteLine($"[{DateTime.Now}] Error handling UDP broadcast: {ex.Message}");
                }
            }
        }

        private static void AddDependencyInjection(ref IServiceCollection services)
        {
            services.AddScoped<PatternLogic>();
            services.AddScoped<AnimationLogic>();
            services.AddScoped<LasershowLogic>();
            services.AddScoped<LaserConnectionLogic>();
            services.AddScoped<LasershowSpotifyConnectorLogic>();
            services.AddSingleton<AudioAnalyser>();
            services.AddSingleton<LaserShowGeneratorAlgorithm>();
            services.AddSingleton<LasershowGeneratorConnectedSongSelector>();
            services.AddScoped<GameLogic>();
            services.AddTransient<ControllerResultHandler>();
            services.AddSingleton<GameStateLogic>();
            services.AddScoped<ILaserConnectionLogic, LaserConnectionLogic>();
            services.AddScoped<IPatternDal, PatternDal>();
            services.AddScoped<IAnimationDal, AnimationDal>();
            services.AddScoped<ILasershowDal, LasershowDal>();
            services.AddScoped<ILasershowSpotifyConnectorDal, LasershowSpotifyConnectorDal>();
            services.AddScoped<IRegisteredLaserDal, RegisteredLaserDal>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Serve the built React frontend (wwwroot) so the API can host the UI itself.
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://127.0.0.1:3000", "https://127.0.0.1:3000")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // Any non-API route falls back to the SPA so client-side routing works on refresh.
                endpoints.MapFallbackToFile("index.html");
            });

            CreateDatabaseIfNotExist(app);
            IServiceScope serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();

            var laserConnectionLogic = serviceScope.ServiceProvider.GetService<LaserConnectionLogic>();
            laserConnectionLogic.Init().Wait();
            Task.Run(() => ListenForBroadcasts());

            // SetProjectionZones(app);

            System.Timers.Timer timer = new() { Interval = 30000 };
            timer.Elapsed += delegate (object o, ElapsedEventArgs eventArgs)
            {
                _ = LaserConnectionLogic.GetStatus();
            };

            timer.Start();
        }

        /// <summary>
        /// Creates and database if it does not exists
        /// </summary>
        /// <param name="app">IApplicationBuilder object</param>
        private static void CreateDatabaseIfNotExist(IApplicationBuilder app)
        {
            IServiceScope serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            DataContext context = serviceScope.ServiceProvider.GetService<DataContext>();
            context.Database.Migrate();
        }
    }
}
