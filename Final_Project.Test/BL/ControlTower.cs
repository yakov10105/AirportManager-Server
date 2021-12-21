using Final_Project.Server.BL.Events;
using Final_Project.Server.BL.Interfaces;
using Final_Project.Server.BL.Services.ControlTowerService;
using Final_Project.Server.BL.Services.StationService;
using Final_Project.Test.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Final_Project.Test.BL
{
    public class ControlTower
    {
        [Fact]
        public void ShouldThrowException1()
        {
            Server.Shared.Models.ControlTower ct = new Server.Shared.Models.ControlTower();
            Assert.Throws<ArgumentNullException>(() => new ControlTowerService(null));
        }
        [Fact]
        public void ShouldThrowException2()
        {
            IControlTowerService ct = new ControlTowerService(new Server.Shared.Models.ControlTower());
            Assert.Throws<ArgumentNullException>(() => ct.NewFlightArrived(null));
        }

        [Fact]
        public void ShouldHaveRightNextStations()
        {
            var arriving = new IFlightHandler[] { new StationService(new Server.Shared.Models.Station(), 1) };
            var departuring = new IFlightHandler[] { new StationService(new Server.Shared.Models.Station(), 1) };

            IControlTowerService ct = new ControlTowerService(new Server.Shared.Models.ControlTower());

            ct.ConnectToNextStations(arriving, departuring);

            Assert.True(ct.ArrivingStations == arriving);
            Assert.True(ct.DeparturingStations == departuring);
        }
        [Fact]
        public void ShouldSendFlightToStation()
        {
            var stationService = new StationService(new Server.Shared.Models.Station(), 1);
            var flightService = new FlightServiceMock() { Flight = new Server.Shared.Models.Flight { Direction = Server.Shared.FlightDirection.Arriving } };
            stationService.NewFlightArrived(flightService);
            var flightHandler = new IFlightHandler[] { stationService };
            IControlTowerService ct = new ControlTowerService(new Server.Shared.Models.ControlTower());
            ct.ConnectToNextStations(flightHandler,null);

            Assert.True(stationService == ct.ArrivingStations.First());
            Assert.True(flightService == stationService.FlightService);
            var flight = new Server.Shared.Models.Flight() { Direction = Server.Shared.FlightDirection.Arriving };
            var flightService2 = new FlightServiceMock { Flight = flight };
            ct.NewFlightArrived(flightService2);

            Assert.Raises<FlightEventArgs>(handler => stationService.FlightEvent += handler, handler => stationService.FlightEvent -= handler, () => flightService.StopWaiting());

            Assert.True(flight == stationService.FlightService.Flight);
        }
    }
}
