using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.Models
{
    public class Flight
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public DateTime Time { get; set; }
        public FlightDirection Direction { get; set; }

        public virtual int AirplaneId { get; set; }
        public virtual Airplane Airplane { get; set; }

        public virtual int? ControlTowerId { get; set; }
        public virtual ControlTower ControlTower { get; set; }

        public virtual int? StationId { get; set; }
        public virtual Station Station { get; set; }


        public virtual ICollection<FlightHistory> History { get; set; }
    }
}
