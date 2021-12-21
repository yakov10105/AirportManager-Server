using Final_Project.Server.Shared.DTO;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.AirportService
{
    public interface IAirportService
    {
        IEnumerable<Airplane> GetAllAirplanes();
        AirportDTO GetAirportData();

        Task NewFlight(Flight flight);

        IEnumerable<FlightHistoryDTO> GetFlightHistoryByStationId(int id);
    }
}
