using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Interfaces
{
    public interface IConnectedToOtherStations
    {
        IEnumerable<IStationRelation> NextStations { get; }

        IEnumerable<IFlightHandler> ArrivingStations { get; }
        IEnumerable<IFlightHandler> DeparturingStations { get; }

        public void ConnectToNextStations(IEnumerable<IFlightHandler> arrivingStations, IEnumerable<IFlightHandler> departuringStations);

    }
}
