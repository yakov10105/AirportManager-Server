using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.FlightService
{
    public class FlightService : IFlightService
    {
        public Flight Flight { get; }

        public bool IsDoneWaiting { get;private set; }


        public event EventHandler<EventArgs> ContinueNextStationEvent;

        public FlightService(Flight flight)
        {
            if (flight != null)
                Flight = flight;
            else throw new ArgumentNullException();
        }

        public async Task StartWaitingAsync(int interval)
        {
            if (interval > 0)
            {
                IsDoneWaiting = false;
                await Task.Delay(interval);
                IsDoneWaiting = true;
                ContinueNextStationEvent?.Invoke(this, EventArgs.Empty);
            }
            else throw new ArgumentOutOfRangeException();
        }
    }
}
