using Final_Project.Server.Simulator.Services.SimulatorService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Final_Project.Server.Simulator.Services.BackgroundService
{
    public class BackgroundService : IHostedService
    {
        IServiceScopeFactory _serviceFactory;
        public BackgroundService(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using(var scope = _serviceFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ISimulatorService>();
                await service.SimulateFlights(flight => Debug.WriteLine(flight));
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ISimulatorService>();
                await Task.Run(() => service.StopSimulating());
            }
        }
    }
}
