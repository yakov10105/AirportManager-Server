using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.Models
{
    public class Station
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual int? CurrentFlightId { get; set; }
        public virtual Flight CurrentFlight { get; set; }

        public virtual int ControlTowerId { get; set; }
        public virtual ControlTower ControlTower { get; set; }

        public virtual StationToControlTower ControlTowerRelation { get; set; }

        public virtual ICollection<StationToStation> ParentStations { get; set; }
        public virtual ICollection<StationToStation> ChildrenStations { get; set; }
        public virtual ICollection<FlightHistory> History { get; set; }
    }
}
