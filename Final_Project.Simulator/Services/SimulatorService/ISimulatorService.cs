using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Simulator.Services.SimulatorService
{
    public interface ISimulatorService
    {
        Task SimulateFlights(Action<string> handleFlightAction);
    }
}
