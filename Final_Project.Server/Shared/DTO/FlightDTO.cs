using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.DTO
{
    public class FlightDTO
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Time { get; set; }
        public FlightDirection Direction { get; set; }
        public int AirplaneId { get; set; }
        public int ControlTowerId { get; set; }
        public int StationId { get; set; }

    }
}
