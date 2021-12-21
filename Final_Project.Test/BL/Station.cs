using Final_Project.Server.BL.Events;
using Final_Project.Server.BL.Interfaces;
using Final_Project.Server.BL.Services.StationService;
using Final_Project.Test.Mocks;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Test.BL
{
    public class Station
    {
        [Fact] 
        public void ShouldThrow1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new StationService(new Server.Shared.Models.Station(), -1));
        }
        [Fact]
        public void ShouldThrow2()
        {
            Assert.Throws<ArgumentNullException>(() => new StationService(null, 1));
        }
        [Fact]
        public void ShouldNotGetNewFlightWhileHaveOne()
        {
            IStationService stationService = new StationService(new Server.Shared.Models.Station(), 1);
            var service1 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };
            var service2 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };

            Assert.True(stationService.NewFlightArrived(service1));
            Assert.True(stationService.FlightService.Equals(service1));
            Assert.False(stationService.IsHandlerAvailable);
            Assert.False(stationService.NewFlightArrived(service2));
            Assert.True(stationService.FlightService.Equals(service1));

        }
        [Fact]
        public void ShouldGetNewFlightWhenDosentHaveOne()
        {
            IStationService stationService = new StationService(new Server.Shared.Models.Station(), 1);
            var service1 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };
            var service2 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };

            Assert.True(stationService.NewFlightArrived(service1));
            Assert.False(stationService.IsHandlerAvailable);
            Assert.Equal(stationService.FlightService,service1);

            service1.StopWaiting();
            Assert.Null(stationService.FlightService);
            Assert.True(stationService.IsHandlerAvailable);
            Assert.True(stationService.NewFlightArrived(service2));
            Assert.True(stationService.FlightService.Equals(service2));

        }
        [Fact] 
        public void ShouldStayIfNextStationOccupied()
        {
            IStationService stationService1 = new StationService(new Server.Shared.Models.Station(), 1);
            IStationService stationService2 = new StationService(new Server.Shared.Models.Station(), 1);
            stationService1.ConnectToNextStations(new IFlightHandler[] { stationService2 }, null);
            var service1 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };
            var service2 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };
            Assert.True(stationService1.NewFlightArrived(service1));
            Assert.True(stationService1.FlightService.Equals(service1));
            Assert.True(stationService2.NewFlightArrived(service2));
            Assert.True(stationService2.FlightService.Equals(service2));
            service1.StopWaiting();
            Assert.False(stationService1.IsHandlerAvailable);
            Assert.True(stationService1.FlightService.Equals(service1));
            Assert.False(stationService2.FlightService == service1);
            Assert.True(stationService2.FlightService.Equals(service2));
        }
        [Fact]
        public void ShouldBecomeAvailableWhenPlaneLeave()
        {
            IStationService stationService1 = new StationService(new Server.Shared.Models.Station(), 1);
            IStationService stationService2 = new StationService(new Server.Shared.Models.Station(), 1);
            stationService1.ConnectToNextStations(new IFlightHandler[] { stationService2 }, null);
            var service1 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };
            Assert.True(stationService1.NewFlightArrived(service1));
            Assert.True(stationService1.FlightService.Equals(service1));
            Assert.Null(stationService2.FlightService);

            var changeEvent = Assert.Raises<FlightEventArgs>(handler => stationService1.FlightEvent += handler, handler => stationService1.FlightEvent -= handler,
                () => service1.StopWaiting());
            Assert.NotNull(changeEvent);
            Assert.True(stationService1 == changeEvent.Sender);
            Assert.True(service1.Flight == changeEvent.Arguments.Flight);
            Assert.True(stationService1.Station == changeEvent.Arguments.StationFrom);
            Assert.True(stationService2.Station == changeEvent.Arguments.StationTo);
            Assert.True(stationService1.IsHandlerAvailable);
            Assert.True(stationService1.FlightService == null);
            Assert.True(stationService2.FlightService == service1);
        }
        [Fact]
        public void ShouldBecomeAvailableIfItLastStation()
        {
            IStationService stationService1 = new StationService(new Server.Shared.Models.Station(), 1);
            IStationService stationService2 = new StationService(new Server.Shared.Models.Station(), 1);
            stationService1.ConnectToNextStations(new IFlightHandler[] { stationService2 }, null);
            var service1 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };
            var service2 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };
            Assert.True(stationService1.NewFlightArrived(service1)); 
            Assert.True(stationService2.NewFlightArrived(service2));
            Assert.False(stationService1.IsHandlerAvailable);
            Assert.False(stationService2.IsHandlerAvailable);
            service2.StopWaiting();
            service1.StopWaiting();
            Assert.True(stationService1.IsHandlerAvailable);
            Assert.False(stationService2.IsHandlerAvailable);
            Assert.Equal(stationService2.FlightService,service1);
        }
        [Fact]
        public void PlaneTransfersToRightStation()
        {
            IStationService stationService1 = new StationService(new Server.Shared.Models.Station(), 1);
            IStationService stationService2 = new StationService(new Server.Shared.Models.Station(), 1);
            IStationService stationService3 = new StationService(new Server.Shared.Models.Station(), 1);
            stationService1.ConnectToNextStations(new IFlightHandler[] { stationService2 }, new IFlightHandler[] { stationService3 });
            var service1 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };
            var service2 = new FlightServiceMock { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Departuring } };
            stationService1.NewFlightArrived(service1);
            service1.StopWaiting();
            Assert.True(stationService1.IsHandlerAvailable);
            Assert.False(stationService2.IsHandlerAvailable);
            Assert.True(stationService2.FlightService == service1);

            stationService1.NewFlightArrived(service2);
            service2.StopWaiting();

            Assert.True(stationService1.IsHandlerAvailable);
            Assert.False(stationService3.IsHandlerAvailable);
            Assert.True(stationService3.FlightService == service2);
        }
    }
}
