using Final_Project.Server.Simulator.Services.SimulatorService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Final_Project.Server
{
    public class Program
    {
        public static void Main(string[] args)
        { 

            CreateHostBuilder(args).Build().Run();         
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return
                 Host.CreateDefaultBuilder(args)
                 .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();

                 });
                 //.ConfigureServices(services =>
                 //{
                 //   services.AddHostedService<Simulator.Services.BackgroundService.BackgroundService>();
                 //   });
        }
        
    }
}
