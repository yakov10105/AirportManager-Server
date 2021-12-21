using Final_Project.Server.BL.Services.FlightService;
using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Test.BL
{
    public class Flight
    {
        [Fact]
        public void ShouldThrowNull()
        {
            Assert.Throws<ArgumentNullException>(() => new FlightService(null));
        }
        [Fact]
        public void ShouldThrowOutOfRange()
        {
            IFlightService service = new FlightService(new Server.Shared.Models.Flight());
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.StartWaitingAsync(-10));
        }
        [Fact]
        public void ShouldRaiseEvent()
        {
            IFlightService service = new FlightService(new Server.Shared.Models.Flight());

            Assert.RaisesAsync<EventArgs>(handler => service.ContinueNextStationEvent += handler,
                                          handler => service.ContinueNextStationEvent -= handler,
                                          () => service.StartWaitingAsync(0));
        }
        [Fact]
        public async void ShouldBeReadyToContinue()
        {
            IFlightService service = new FlightService(new Server.Shared.Models.Flight());

            Assert.False(service.IsDoneWaiting);
            await service.StartWaitingAsync(2);
            Assert.True(service.IsDoneWaiting);
        }
    }
}
