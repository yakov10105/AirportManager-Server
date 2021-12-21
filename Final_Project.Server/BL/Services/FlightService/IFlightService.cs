using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.FlightService
{
    public interface IFlightService
    {
        public Flight Flight{ get;}
        public bool IsDoneWaiting { get; }

        public event EventHandler<EventArgs> ContinueNextStationEvent;

        Task StartWaitingAsync(int interval);
        
    }
}
