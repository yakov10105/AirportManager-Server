using Final_Project.Server.BL.Events;
using Final_Project.Server.BL.Interfaces;
using Final_Project.Server.BL.Services.FlightService;
using Final_Project.Server.Shared;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.ControlTowerService
{
    public class ControlTowerService : IControlTowerService
    {

        public ControlTower ControlTower { get;init;}

        public IEnumerable<IStationRelation> NextStations => ControlTower?.ConnectedStations;

        public IEnumerable<IFlightHandler> ArrivingStations { get; set; }

        public IEnumerable<IFlightHandler> DeparturingStations { get; set; }

        public event EventHandler<FlightEventArgs> FlightEvent;

        private readonly object _lockObject = new object();

        private LinkedList<Flight> ArrivingFlights { get; set; }
        private LinkedList<Flight> DeparturingFlights { get; set; }


        public ControlTowerService(ControlTower controlTower)
        {
            if (controlTower == null) throw new ArgumentNullException();
            ControlTower = controlTower;
            ArrivingFlights=new LinkedList<Flight>();
            DeparturingFlights=new LinkedList<Flight>();

        }

        public bool NewFlightArrived(IFlightService flight)
        {
            if(flight != null)
            {
                var flights = flight.Flight.Direction.Equals(FlightDirection.Arriving) ? ArrivingFlights : DeparturingFlights;  
                if(flights.Count == 0)
                {
                    SendFlightToStation(flight);
                }
                else
                {
                    SendFlightToWaitingList(flight.Flight);
                }
                return true;
            }
            else throw new ArgumentNullException();
        }

        public void ConnectToNextStations
            (IEnumerable<IFlightHandler> arrivingStations , IEnumerable<IFlightHandler> departuringStations)
        {
            StopListenToStationsEvents();
            ArrivingStations =arrivingStations;
            DeparturingStations=departuringStations;
            StartListenToStationsEvents();

            if (ArrivingFlights.First != null)
            {
                SendFlightToStation(new FlightService.FlightService(ArrivingFlights.First.Value));
                ArrivingFlights.RemoveFirst();
            }
            if(DeparturingFlights.First != null)
            {
                SendFlightToStation(new FlightService.FlightService(DeparturingFlights.First.Value));
                DeparturingFlights.RemoveFirst();
            }
        }

        private void SendFlightToWaitingList(Flight flight, bool isInWaitingList = false)
        {
            if(flight == null) throw new ArgumentNullException();
            var flights = flight.Direction.Equals(FlightDirection.Arriving)? ArrivingFlights : DeparturingFlights;
            if (isInWaitingList)
            {
                flights.AddFirst(flight);
            }
            else
            {
                flights.AddLast(flight);
            }
        }
        private void SendFlightToStation(IFlightService flightService , bool isInWaitingList=false)
        {
            if (flightService == null) throw new ArgumentNullException();
            var flights = flightService.Flight.Direction.Equals(FlightDirection.Arriving) ? ArrivingStations : DeparturingStations;
            var freeStation = flights?.FirstOrDefault(f => f.IsHandlerAvailable);
            if (freeStation != null && freeStation.IsHandlerAvailable && freeStation.NewFlightArrived(flightService))
                FlightEvent?.Invoke(this, new FlightEventArgs(flightService.Flight, null, freeStation.Station));
            else SendFlightToWaitingList(flightService.Flight, isInWaitingList);
        }

        //Stops the listening for all the stations
        private void StopListenToStationsEvents()
        {
            IEnumerable<IFlightHandler> allStations;
            if (ArrivingStations != null)
            {
                if (DeparturingStations != null)
                    allStations = ArrivingStations.Concat(DeparturingStations);
                else
                    allStations = ArrivingStations;
            }
            else
            {
                if (DeparturingStations != null)
                    allStations = DeparturingStations;
                else
                    allStations = Enumerable.Empty<IFlightHandler>();
            }
            foreach (var st in allStations)
            {
                st.FlightEvent -= NextStationAvailable;
            }

        }
        //Starts the listening for all the stations
        private void StartListenToStationsEvents()
        {
            IEnumerable<IFlightHandler> allStations;
            if (ArrivingStations != null)
            {
                if (DeparturingStations != null)
                    allStations = ArrivingStations.Concat(DeparturingStations);
                else
                    allStations = ArrivingStations;
            }
            else
            {
                if (DeparturingStations != null)
                    allStations = DeparturingStations;
                else
                    allStations = Enumerable.Empty<IFlightHandler>();
            }
            foreach (var st in allStations)
            {
                st.FlightEvent += NextStationAvailable;
            }
        }

        private void NextStationAvailable(object sender, EventArgs e)
        {
            if(sender is IFlightHandler flightHandler)
            {
                bool isArrivingStation = ArrivingStations == null ? false : ArrivingStations.Contains(flightHandler);
                bool isDeparturingStation = DeparturingStations == null ? false : DeparturingStations.Contains(flightHandler);
                IFlightService flightService=null;
                lock (_lockObject)
                {
                    if (isArrivingStation)
                    {
                        flightService = new FlightService.FlightService(ArrivingFlights.First());
                        ArrivingFlights.RemoveFirst();
                    } 
                    else if (isDeparturingStation)
                    {
                        flightService = new FlightService.FlightService(DeparturingFlights.First());
                        DeparturingFlights.RemoveFirst();
                    }
                }
                if(flightService != null) SendFlightToStation(flightService);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
