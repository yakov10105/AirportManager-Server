using Final_Project.Simulator.Services.HubService;
using Final_Project.Simulator.Services.ServerConnectionService;
using Final_Project.Simulator.Services.SimulatorService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Final_Project.Simulator
{
    internal class Program
    {
        static async Task Main(string[] args)
       {
            Thread.Sleep(10000);
            var provider = new ServiceCollection()
                .AddSingleton<ISimulatorService, SimulatorService>()
                .AddSingleton<IServerConnectionService, ServerConnectionService>()
                .AddSingleton<IHubService, HubService>()
                .AddHttpClient()
                .BuildServiceProvider();

            var simulator = provider.GetRequiredService<ISimulatorService>();

            await simulator.SimulateFlights(flight => Console.WriteLine(flight));
        }
    }
}
