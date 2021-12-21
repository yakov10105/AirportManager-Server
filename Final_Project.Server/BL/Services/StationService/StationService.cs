using Final_Project.Server.BL.Events;
using Final_Project.Server.BL.Interfaces;
using Final_Project.Server.BL.Services.FlightService;
using Final_Project.Server.Shared;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.StationService
{
    public class StationService : IStationService
    {
        public Station Station{ get; set; }

        public IFlightService FlightService { get; private set; }

        public int WaitingInterval { get; }

        public bool IsHandlerAvailable => FlightService == null;

        public IEnumerable<IStationRelation> NextStations => Station?.ChildrenStations;

        public IEnumerable<IFlightHandler> ArrivingStations { get; set; }

        public IEnumerable<IFlightHandler> DeparturingStations { get; set; }

        public event EventHandler<FlightEventArgs> FlightEvent;

        private readonly object _lockObj = new object();

        public StationService(Station station ,int interval)
        {
            if (interval < 0) throw new ArgumentOutOfRangeException();
            if (station == null) throw new ArgumentNullException();
            this.Station = station;
            this.WaitingInterval = interval;
        }

        public void ConnectToNextStations(IEnumerable<IFlightHandler> arrivingStations , IEnumerable<IFlightHandler> departuringStations)
        {
            StopListenToStationsEvents();
            ArrivingStations = arrivingStations;
            DeparturingStations = departuringStations;
            StartListenToStationsEvents();
        }

        //handles new flight that arrives to the station
        public bool NewFlightArrived(IFlightService flight)
        {
            Debug.WriteLine($"<<<================Flight :{flight.Flight.Id} Entered : Station {Station.Name} ====================>>>");
            lock (_lockObj)
            {
                if(FlightService == null)
                {
                    FlightService = flight;
                    FlightService.StartWaitingAsync(WaitingInterval);
                    FlightService.ContinueNextStationEvent += ReadyContinueToNextStation;
                    return true;
                }
                return false;
            }
        }
        private void ChangeStationState(Station nextStation)
        {
            var flight = FlightService.Flight;
            FlightService.ContinueNextStationEvent -= ReadyContinueToNextStation;
            FlightService = null;
            FlightEvent?.Invoke(this,new FlightEventArgs(flight,Station,nextStation));
        }

        private void ReadyContinueToNextStation(object sender , EventArgs e)
        {
            if (sender is not IFlightService)
                throw new Exception("Sender must be FlightService");
            var nextStations = 
                FlightService.Flight.Direction.Equals(FlightDirection.Arriving) ? ArrivingStations : DeparturingStations;
            if(nextStations == null || !nextStations.Any())
            {
                ChangeStationState(null);
                return;
            }
            var flightNextStation = nextStations.FirstOrDefault(s => s.IsHandlerAvailable);
            if (flightNextStation != null && flightNextStation.NewFlightArrived(FlightService))
                ChangeStationState(flightNextStation.Station);
            
        }

        private void NextStationAvailable(object sender , EventArgs e)
        {
            if(FlightService ==null || FlightService.IsDoneWaiting==false) return;
            if (sender is not IFlightHandler n)
                throw new Exception("Sender must be FlightService");
            var nextStations =
                FlightService.Flight.Direction.Equals(FlightDirection.Arriving) ? ArrivingStations : DeparturingStations;
            if (nextStations.Contains(n)==false) return;

            if (n.IsHandlerAvailable && n.NewFlightArrived(FlightService))
                ChangeStationState(n.Station);
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
    }
}
