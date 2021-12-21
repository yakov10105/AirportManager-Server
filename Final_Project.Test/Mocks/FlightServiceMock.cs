using Final_Project.Server.BL.Services.FlightService;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Test.Mocks
{
    public class FlightServiceMock : IFlightService

    {
        public Flight Flight { get; set; }

        public bool IsDoneWaiting { get; set; }

        public event EventHandler<EventArgs> ContinueNextStationEvent;

        public Task StartWaitingAsync(int interval)
        {
            IsDoneWaiting = false;
            return null;
        }
        public void StopWaiting()
        {
            ContinueNextStationEvent?.Invoke(this, EventArgs.Empty);
            IsDoneWaiting = true;
        }
    }
}
