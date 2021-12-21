using Final_Project.Server.BL.Events;
using Final_Project.Server.BL.Interfaces;
using Final_Project.Server.BL.Services.FlightService;
using Final_Project.Server.BL.Services.StationService;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Test.Mocks
{
    public class StationServiceMock : IStationService
    {
        public IFlightService FlightService => throw new NotImplementedException();

        public int WaitingInterval => throw new NotImplementedException();

        public Station Station => throw new NotImplementedException();

        public bool IsHandlerAvailable => throw new NotImplementedException();

        public IEnumerable<IStationRelation> NextStations => throw new NotImplementedException();

        public IEnumerable<IFlightHandler> ArrivingStations => throw new NotImplementedException();

        public IEnumerable<IFlightHandler> DeparturingStations => throw new NotImplementedException();

        public event EventHandler<FlightEventArgs> FlightEvent;

        public void ConnectToNextStations(IEnumerable<IFlightHandler> arrivingStations, IEnumerable<IFlightHandler> departuringStations)
        {
            throw new NotImplementedException();
        }

        public bool NewFlightArrived(IFlightService flight)
        {
            throw new NotImplementedException();
        }

        public void RaiseFlightEvent()
        {
            Flight flight = new() { From = "MOCK", To = "MOCK" };
            Station stationFrom = new() { Name = "FromMock" };
            Station stationTo = new() { Name = "ToMock" };

            FlightEvent?.Invoke(this, new(flight, stationFrom, stationTo));
        }
    }
}
