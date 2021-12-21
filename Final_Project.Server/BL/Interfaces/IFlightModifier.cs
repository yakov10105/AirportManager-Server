using Final_Project.Server.BL.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Interfaces
{
    public interface IFlightModifier
    {
        public event EventHandler<FlightEventArgs> FlightEvent;
    }
}
