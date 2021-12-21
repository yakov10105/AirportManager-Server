using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.DTO
{
    public class StationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ControlTowerId { get; set; }
        public FlightDTO CurrentFlight { get; set; }
    }
}
