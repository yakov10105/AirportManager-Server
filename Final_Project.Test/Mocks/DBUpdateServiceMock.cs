using Final_Project.Server.BL.Events;
using Final_Project.Server.BL.Services.DatabaseUpdateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Test.Mocks
{
    class DBUpdateServiceMock : IDatabaseUpdateService
    {
        public FlightEventArgs FlightEvent { get; set; }
        public Task FlightMoved(FlightEventArgs eventArgs)
        {
            FlightEvent = eventArgs;
            return null;
        }
    }
}
