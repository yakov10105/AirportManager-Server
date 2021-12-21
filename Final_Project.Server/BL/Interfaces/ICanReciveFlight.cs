using Final_Project.Server.BL.Services.FlightService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Interfaces
{
    public interface ICanReciveFlight
    {
        bool NewFlightArrived(IFlightService flight);
    }
}
