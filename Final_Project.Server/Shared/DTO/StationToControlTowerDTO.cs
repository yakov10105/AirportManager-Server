using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.DTO
{
    public class StationToControlTowerDTO
    {
        public FlightDirection Direction { get; set; }
        public int StationToId { get; set; }
        public int ControlTowerId { get; set; }

    }
}
