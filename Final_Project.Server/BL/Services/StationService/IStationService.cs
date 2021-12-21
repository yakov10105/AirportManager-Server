using Final_Project.Server.BL.Interfaces;
using Final_Project.Server.BL.Services.FlightService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.StationService
{
    public interface IStationService:IFlightHandler,IConnectedToOtherStations
    {
        public IFlightService FlightService{ get;}

        public int WaitingInterval { get; }
    }
}
