using LaserAPI.Dal;
using LaserAPI.Interfaces.Dal;
using LaserAPI.Logic;
using LaserAPI.Logic.Fft_algorithm;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
            AddDependencyInjection(ref services);
        }

        private static void AddDependencyInjection(ref IServiceCollection services)
        {
            services.AddScoped<PatternLogic>();
            services.AddScoped<AnimationLogic>();
            services.AddScoped<ZoneLogic>();
            services.AddScoped<LasershowLogic>();
            services.AddSingleton<AudioAnalyser>();
            services.AddSingleton<LaserShowGeneratorLogic>();
            services.AddScoped<IPatternDal, PatterDal>();
            services.AddScoped<IAnimationDal, AnimationDal>();
            services.AddScoped<IZoneDal, ZoneDal>();
            services.AddScoped<ILasershowDal, LasershowDal>();
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
                builder.WithOrigins("http://localhost:3000")
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
