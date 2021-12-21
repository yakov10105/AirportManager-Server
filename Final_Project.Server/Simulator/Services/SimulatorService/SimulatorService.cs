using Final_Project.Server.BL.Services.AirportService;
using Final_Project.Server.DAL.Repo;
using Final_Project.Server.Shared;
using Final_Project.Server.Shared.DTO;
using Final_Project.Server.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Final_Project.Server.Simulator.Services.SimulatorService
{
    public class SimulatorService : ISimulatorService
    {
        private readonly Random _rand = new Random();
        private readonly IAirportService _airportService;
        private bool _stopFlag;
        private IEnumerable<Airplane> _airplanes;

        public SimulatorService(IAirportService airportService)
        {
            _airportService = airportService;
            GetAirplanesFromServer();
        }

        public async Task SimulateFlights(Action<string> handleFlightAction)
        {
            _stopFlag = false;
            while (!_stopFlag)
            {
                await Task.Delay(_rand.Next(4000, 7000));
                var flight = await PostFlightToServer();
                if (flight != null)
                    handleFlightAction?.Invoke($"{flight.Direction} => from: {flight.From} to: {flight.To} ; Time: {flight.Time}"); 
            }
        }

        private Flight CreateFlight()
        {
            var names = new string[] { "LAX", "DXB", "LHR", "SVO", "JFK", "DME" };
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
            var time = DateTime.Now.AddSeconds(20);
            return new Flight { Direction = direction, AirplaneId = airplane.Id, From = from, To = to, Time = time };
        }
        private async Task<Flight> PostFlightToServer()
        {
            var flight = CreateFlight();
           
            if (flight!=null)
            {
                try
                {
                    await _airportService.NewFlight(flight);
                    return flight;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                } 
            }
            return null;
            
        }
        private Airplane GetAirplane()
        {
            if (_airplanes != null && _airplanes.Count() > 0)
            {
                if (_airplanes.Count() == 1) return _airplanes.First();
                return _airplanes.ElementAt(_rand.Next(0, _airplanes.Count()));
            }
            else return null;
        }
        private void GetAirplanesFromServer()
        {
            _airplanes = _airportService.GetAllAirplanes();
            if(_airplanes == null)
            {
                Debug.WriteLine("There is no airplane from the server");
                _airplanes= Enumerable.Empty<Airplane>();
            }
        }
        public void StopSimulating()
        {
            _stopFlag = true;
        }
    }
}
