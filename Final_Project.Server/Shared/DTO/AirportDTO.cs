using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.DTO
{
    public class AirportDTO
    {
        public IEnumerable<FlightDTO> Flights { get; set; }
        public IEnumerable<StationDTO> Stations { get; set; }
        public IEnumerable<StationToStationDTO> StationRelations { get; set; }
        public IEnumerable<StationToControlTowerDTO> FirstStations { get; set; }
        public ControlTowerDTO ControlTower { get; set; }

    }
}
