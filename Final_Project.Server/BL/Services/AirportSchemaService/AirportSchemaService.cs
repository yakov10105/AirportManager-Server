using Final_Project.Server.BL.Interfaces;
using Final_Project.Server.BL.Services.ControlTowerService;
using Final_Project.Server.BL.Services.EventsSevice;
using Final_Project.Server.BL.Services.StationService;
using Final_Project.Server.Shared;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.AirportSchemaService
{
    public class AirportSchemaService : IAirportSchemaService
    {
        const int INTERVAL = 3000;  

        private readonly object _lockObject = new object();

        private readonly IEventsService _eventService;

        private  ICollection<IStationService> _stationServices;

        public  IControlTowerService ControlTowerService { get; set; }

        public bool IsCreated { get; set; }

        public AirportSchemaService(IEventsService eventsService)
        {
            if(eventsService == null) throw new ArgumentNullException();
            _eventService = eventsService;
        }


        public void CreateSchema(ControlTower controlTower, IEnumerable<Station> stations)
        {
            if(controlTower == null || stations ==null) throw new ArgumentNullException();

            lock (_lockObject)
            {
                if(_stationServices == null) _stationServices = new List<IStationService>();

                var isNewCT = CreateControlTowerService(controlTower);
                var isNewST = CreateStationsServices(stations);

                if(isNewCT || isNewST)
                {
                    Connect(ControlTowerService);
                    ConnectStationsToStations();

                    var controlTowerFlightModifier = ControlTowerService as IFlightModifier;
                    var stationsFlightModifier = _stationServices.ToList<IFlightModifier>();
                    stationsFlightModifier.Add(controlTowerFlightModifier);
                    _eventService.AddStationsToEventListener(stationsFlightModifier.AsEnumerable());
                }
            }

        }
        public void InitFlightHistory(IEnumerable<FlightHistory> history)
        {
            IsCreated = true;
            foreach (var fh in history)
            {
                var stationsService = _stationServices.FirstOrDefault(s => s.Station.Id == fh.StationId);
                if(stationsService != null)
                    stationsService.NewFlightArrived(new FlightService.FlightService(fh.Flight));
                else throw new ArgumentNullException();
            }
        }

        private void ConnectStationsToStations()
        {
            foreach (var s in _stationServices)
            {
                Connect(s);
            }
        }
        private void Connect(IConnectedToOtherStations connectedToOther)
        {
            var arrivingStations = connectedToOther
                                    .NextStations
                                    .Where(sr => sr.Direction.Equals(FlightDirection.Arriving))
                                    .Join(_stationServices, sr => sr.StationToId, s => s.Station.Id, (sr, s) => s);
            var departuringStations = connectedToOther
                                        .NextStations
                                        .Where(sr => sr.Direction.Equals(FlightDirection.Departuring))
                                        .Join(_stationServices, sr => sr.StationToId, s => s.Station.Id, (sr, s) => s);
            connectedToOther.ConnectToNextStations(arrivingStations, departuringStations);
        }


        private bool CreateControlTowerService(ControlTower controlTower)
        {
            bool retValue = false;
            if(controlTower != null)
            {
                retValue = true;
                ControlTowerService = new ControlTowerService.ControlTowerService(controlTower);
            }
            return retValue;
        }
        private bool CreateStationsServices(IEnumerable<Station> stations)
        {
            bool retValue = false;
            foreach(Station station in stations)
            {
                retValue = true;
                var stationService = new StationService.StationService(station, INTERVAL);
                _stationServices.Add(stationService);
            }
            return retValue;
        }
    }
}
