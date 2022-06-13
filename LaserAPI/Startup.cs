using LaserAPI.Dal;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Logic;
using LaserAPI.Logic.Fft_algorithm;
using LaserAPI.Logic.Game;
using LaserAPI.Models.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using System.Timers;
using Timer = System.Timers.Timer;

namespace LaserAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            string connectionString = Configuration.GetConnectionString("DefaultConnection");
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

        private static void AddDependencyInjection(ref IServiceCollection services)
        {
            services.AddScoped<LaserLogic>();
            services.AddScoped<PatternLogic>();
            services.AddScoped<AnimationLogic>();
            services.AddScoped<LasershowSpotifyConnectorLogic>();
            services.AddScoped<ZoneLogic>();
            services.AddSingleton<AudioAnalyser>();
            services.AddSingleton<LaserShowGeneratorAlgorithm>();
            services.AddScoped<GameLogic>();
            services.AddScoped<LaserShowGeneratorLogic>();
            services.AddTransient<ControllerResultHandler>();
            services.AddSingleton<GameStateLogic>();
            services.AddScoped<IPatternDal, PatternDal>();
            services.AddScoped<IAnimationDal, AnimationDal>();
            services.AddScoped<IZoneDal, ZoneDal>();
            services.AddScoped<ILasershowSpotifyConnectorDal, LasershowSpotifyConnectorDal>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:3000", "https://localhost:3000")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            CreateDatabaseIfNotExist(app);
            SetCurrentIpAddress();

            Timer timer = new() { Interval = 10000 };
            timer.Elapsed += delegate (object o, ElapsedEventArgs eventArgs)
            {
                bool clientConnected = LaserConnectionLogic.TcpClient?.Connected is true;
                if (!clientConnected && !LaserConnectionLogic.ConnectionPending)
                {
                    LaserConnectionLogic.NetworkConnect();
                }

                //todo add connection alive check
            };

            timer.Start();
        }

        private static bool NetworkInterfaceHasEthernetAccess(NetworkInterface networkInterface)
        {
            return networkInterface.OperationalStatus == OperationalStatus.Up &&
                   networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                   networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                   networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                   networkInterface.GetIPProperties().GatewayAddresses.Any();
        }

        private static void SetCurrentIpAddress()
        {
            /*string currentIpAddress = NetworkInterface
                .GetAllNetworkInterfaces()
                .FirstOrDefault(n => NetworkInterfaceHasEthernetAccess(n) && !n.Description.ToLower()
                    .Contains("virtual"))
                ?.GetIPProperties()
                .GatewayAddresses
                .FirstOrDefault()
                ?.Address.ToString();
            */
            string currentIpAddress = "192.168.1.31";
            if (string.IsNullOrEmpty(currentIpAddress))
            {
                throw new ConnectionAbortedException("This computer is not connected to a local network. This application need access to an LAN network to function.");

                //string errorMessage = "Could not connect to client software, start it first before starting this application!";
                //throw new NoClientSoftwareFoundException(errorMessage);
            }

            LaserConnectionLogic.ComputerIpAddress = currentIpAddress;
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
