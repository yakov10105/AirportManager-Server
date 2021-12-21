using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.Models
{
    public class StationToStation:IStationRelation
    {
        public virtual int  StationFromId { get; set; }
        public virtual int StationToId { get; set; }

        public FlightDirection Direction { get; set; }

        public virtual Station FromStation { get; set; }
        public virtual Station ToStation { get; set; }

    }
}
