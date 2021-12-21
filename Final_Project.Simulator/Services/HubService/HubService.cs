using Final_Project.Server.Shared.DTO;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Simulator.Services.HubService
{
    public class HubService : IHubService
    {
        private readonly HubConnection _hubConnection;
        public HubService()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:47632/airport-socket")
                .WithAutomaticReconnect()
                .Build();
            _hubConnection.StartAsync();
        }
        public async Task AddFlight(FlightDTO flight)
        {
            await _hubConnection.InvokeAsync("AddFlight", flight);
        }

        public async Task<IEnumerable<AirplaneDTO>> GetAirplanes()
        {
            IEnumerable<AirplaneDTO> res = null; 
            try
            {
                res =await _hubConnection.InvokeAsync<IEnumerable<AirplaneDTO>>("GetAirplanes");
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }
            return res;
            
        }

        public IDisposable Listen<T>(string mName, Action<T> p)
        {
            return _hubConnection.On(mName, p);
        }
    }
}
