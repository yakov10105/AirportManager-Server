using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.DTO
{
    public class FlightHistoryDTO
    {
        public int Id { get; set; }
        public DateTime EnterTime { get; set; }
        public DateTime LeaveTime { get; set; }
        public FlightDTO Flight { get; set; }
        public StationDTO Station { get; set; }
    }
}
