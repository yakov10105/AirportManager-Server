using Final_Project.Server.Shared;
using Final_Project.Server.Shared.DTO;
using Final_Project.Server.Shared.Models;
using Final_Project.Simulator.Services.HubService;
using Final_Project.Simulator.Services.ServerConnectionService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Simulator.Services.SimulatorService
{
    public class SimulatorService : ISimulatorService
    {
        private readonly IServerConnectionService _serverConService;
        private readonly Random _rand = new Random();
        private readonly IHubService _hubService;
        private IEnumerable<AirplaneDTO> _airplanes;

        public SimulatorService(IServerConnectionService serverConnService, IHubService hubService)
        {
            _serverConService = serverConnService;
            _hubService = hubService;
            Task.WaitAll(GetAirplanesFromServer());
            _hubService.Listen<ICollection<AirplaneDTO>>("AirplaneUpdates", a => _airplanes = a);
        }

        public async Task SimulateFlights(Action<string> handleFlightAction)
        {
            while (true)
            {
                await Task.Delay(_rand.Next(4000, 6000));
                var flight = await PostFlightToServer();
                if (flight != null)
                    handleFlightAction?.Invoke($"{flight.Direction} => from: {flight.From} to: {flight.To} ; Time: {flight.Time}"); 
            }
        }

        private FlightDTO CreateFlight()
        {
            var names = new string[] { "LAX","DXB","LHR","SVO","JFK","DME"};
            var direction = Enum.GetValues<FlightDirection>().ElementAt(_rand.Next(2));
            string from, to;
            if (direction.Equals(FlightDirection.Arriving))
            {
                from = names.ElementAt(_rand.Next(names.Length));
                to = "TLV";
            }
            else
            {
                to = names.ElementAt(_rand.Next(names.Length));
                from = "TLV";
            }
            var airplane = GetAirplane();
            var time = DateTime.Now.AddSeconds(4);
            return new FlightDTO { Direction = direction, AirplaneId = airplane.Id, From = from, To = to, Time = time };
        }

        private async Task<FlightDTO> PostFlightToServer()
        {
            var flight = CreateFlight();
            if(flight!= null)
            {
                await _serverConService.PostFlight(flight);
                return flight;
            }
            else
            {
                Console.WriteLine("Unsuccessfull flight creation...");
                return null;
            }
        }

        private AirplaneDTO GetAirplane()
        {
            if (_airplanes != null && _airplanes.Count() > 0)
            {
                if (_airplanes.Count() == 1) return _airplanes.First();
                return _airplanes.ElementAt(_rand.Next(0, _airplanes.Count()));
            }
            else return null;
        }

        private async Task GetAirplanesFromServer()
        {
            _airplanes = await _serverConService.GetAirplanes();
            if (_airplanes == null)
            {
                Console.WriteLine("Theres no airplanes returned from the server");
                _airplanes = Array.Empty<AirplaneDTO>();
            }
        }

    }
}
