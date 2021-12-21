using Final_Project.Server.BL.Events;
using Final_Project.Server.BL.Services.HubNotifierService;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Test.Mocks
{
    public class HubNotifierMock : IHubNotifierService
    {
        public FlightEventArgs FlightNotification { get; set; }
        public void NotifyChanged(FlightEventArgs flightEventArgs)
        {
            FlightNotification = flightEventArgs;
        }

        public void NotifyNewFlightAdded(Flight flight)
        {
            throw new NotImplementedException();
        }
    }
}
