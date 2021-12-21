using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.Models
{
    public class StationToControlTower:IStationRelation
    {
        public FlightDirection Direction { get; set; }

        public virtual int StationToId { get; set; }
        public virtual Station Station { get; set; }

        public virtual int ControlTowerId { get; set; }
        public virtual ControlTower ControlTower { get; set; }
    }
}
