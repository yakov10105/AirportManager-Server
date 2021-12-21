using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.DTO
{
    public class StationToStationDTO
    {
        public FlightDirection Direction { get; set; }
        public int FromStationId { get; set; }
        public int ToStationId { get; set; }
    }
}
