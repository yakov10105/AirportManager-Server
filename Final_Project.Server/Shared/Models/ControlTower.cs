using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.Models
{
    public class ControlTower
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string  Name { get; set; }

        public virtual ICollection<StationToControlTower> ConnectedStations { get; set; }
        public virtual ICollection<Flight> WaitingFlights { get; set; }
        public virtual ICollection<Station> Stations { get; set; }


    }
}
