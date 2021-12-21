using Final_Project.Server.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Simulator.Services.HubService
{
    public interface IHubService
    {
        IDisposable Listen<T>(string mName, Action<T> p);
        Task AddFlight(FlightDTO flight);
        Task<IEnumerable<AirplaneDTO>> GetAirplanes();
    }
}
