using LaserAPI.Dal;
using LaserAPI.Interfaces;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Logic;
using LaserAPI.Logic.Fft_algorithm;
using LaserAPI.Logic.Game;
using LaserAPI.Models.Dto.Animations;
using LaserAPI.Models.Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Text.Json.Serialization;

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

        private static void AddDependencyInjection(ref IServiceCollection services)
        {
            services.AddScoped<LaserLogic>();
            services.AddScoped<PatternLogic>();
            services.AddScoped<AnimationLogic>();
            services.AddScoped<LasershowSpotifyConnectorLogic>();
            services.AddScoped<ZoneLogic>();
            services.AddSingleton<AudioAnalyser>();
            services.AddSingleton<LaserShowGeneratorAlgorithm>();
            services.AddSingleton<LasershowGeneratorConnectedSongSelector>();
            services.AddScoped<GameLogic>();
            services.AddTransient<ControllerResultHandler>();
            services.AddSingleton<GameStateLogic>();
            services.AddSingleton<ILaserConnectionLogic, LaserConnectionLogic>();
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
            // SetProjectionZones(app);

            /*Timer timer = new() { Interval = 10000 };
            timer.Elapsed += delegate (object o, ElapsedEventArgs eventArgs)
            {
                SaveGeneratedLaserShow(app);
            };

            timer.Start();*/
        }

        private static void SaveGeneratedLaserShow(IApplicationBuilder app)
        {
            bool generatedLaserShowIsWaitingInQueue = GeneratedLaserShowsQueue.LaserShowToSave.Uuid != Guid.Empty;
            if (generatedLaserShowIsWaitingInQueue)
            {
                IServiceScope serviceScope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>()
                    .CreateScope();
                AnimationLogic animationLogic = serviceScope.ServiceProvider.GetService<AnimationLogic>();
                animationLogic.AddOrUpdate(GeneratedLaserShowsQueue.LaserShowToSave).Wait();
                GeneratedLaserShowsQueue.LaserShowToSave = new AnimationDto();
            }
        }

        private static void SetProjectionZones(IApplicationBuilder app)
        {
            IServiceScope serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            ZoneLogic zoneLogic = serviceScope.ServiceProvider.GetService<ZoneLogic>();
            System.Collections.Generic.List<Models.Dto.Zones.SafetyZoneDto> zones = zoneLogic.All().Result;
            ProjectionZonesLogic.Zones = zones;
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
