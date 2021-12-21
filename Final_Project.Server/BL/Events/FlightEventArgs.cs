using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Events
{
    public class FlightEventArgs:EventArgs
    {
        public Flight Flight{ get; init; }
        public Station StationFrom{ get; init; }
        public Station StationTo{ get; init; }

        public bool IsArrivingToFirstStation => StationFrom == null && StationTo != null;
        public bool IsLeavingLastStation => StationFrom != null && StationTo == null;
        public bool IsToEqualsFrom => StationFrom == StationTo;


        public FlightEventArgs(Flight flight , Station from , Station to)
        {
            if(flight != null)
            {
                Flight = flight;
                if (from == null && to == null)
                    throw new ArgumentNullException();
                StationFrom = from;
                StationTo = to;
            }
            else throw new ArgumentNullException();
        }
    }
}
