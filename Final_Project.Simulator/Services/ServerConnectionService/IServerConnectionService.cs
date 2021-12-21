using Final_Project.Server.Shared.DTO;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Simulator.Services.ServerConnectionService
{
    public interface IServerConnectionService
    {
        Task<ICollection<AirplaneDTO>> GetAirplanes();
        Task PostFlight(FlightDTO flight);
    }
}
