using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.Models
{
    public class FlightHistory
    {
        [Key]
        public int Id { get; set; }
        public DateTime? EnterStationTime { get; set; }
        public DateTime? LeaveStationTime { get; set; }

        public virtual int FlightId { get; set; }
        public virtual Flight Flight { get; set; }

        public virtual int  StationId { get; set; }
        public virtual Station Statoin { get; set; }
    }
}
