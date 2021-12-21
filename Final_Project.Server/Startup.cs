using Final_Project.Server.DAL;
using Final_Project.Server.BL.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Final_Project.Server.BL.Services.HubNotifierService;
using Final_Project.Server.BL.Services.DatabaseUpdateService;
using Final_Project.Server.BL.Services.EventsSevice;
using Final_Project.Server.BL.Services.AirportSchemaService;
using Final_Project.Server.DAL.Repo;
using Final_Project.Server.BL.Services.AirportService;
using Newtonsoft.Json;
using AutoMapper;
using Final_Project.Server.Shared.Mappers;
using Final_Project.Server.Simulator.Services.SimulatorService;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Final_Project.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddCors(); 
            services.AddDbContext<AirportDbContext>(opt => {
                opt
                    .UseLazyLoadingProxies()
                    .ConfigureWarnings(warn => warn.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning))
                    .UseSqlServer(connectionString,opt=>opt.EnableRetryOnFailure());
                    }, ServiceLifetime.Singleton);

            var profiles = new Profile[]
            {
                new AirplaneProfile(),
                new ControlTowrProfile(),
                new FlightHistoryProfile(),
                new FlightProfile(),
                new StationControlTowerProfile(),
                new StationProfile(),
                new StationToStationProfile()
            };
            services.AddAutoMapper(am => am.AddProfiles(profiles));

            services.AddMvc();

            //services.AddScoped<ISimulatorService, SimulatorService>();
            services.AddSingleton<IHubNotifierService, HubNotifierService>();
            services.AddSingleton<IDatabaseUpdateService, DatabaseUpdateService>();
            services.AddSingleton<IEventsService, EventsService>();
            services.AddSingleton<IAirportSchemaService, AirportSchemaService>();
            services.AddSingleton<IAirportService, AirportService>();
            services.AddSingleton(typeof(IRepository<>), typeof(Repository<>));

            services.AddSignalR().AddNewtonsoftJsonProtocol(o => o.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);


            services.AddControllers().AddNewtonsoftJson(opt=>opt.SerializerSettings.ReferenceLoopHandling=ReferenceLoopHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app,AirportDbContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
            app.UseCors(opt=>opt
                .WithOrigins(new[] {"http://localhost:3000"})
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials());
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<FlightHub>("/airport-socket");
            });
        }
    }
}
