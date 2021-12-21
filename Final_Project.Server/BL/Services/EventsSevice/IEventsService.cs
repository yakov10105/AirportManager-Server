using Final_Project.Server.BL.Interfaces;
using System.Collections.Generic;

namespace Final_Project.Server.BL.Services.EventsSevice
{
    public interface IEventsService
    {

        void AddStationsToEventListener(IEnumerable<IFlightModifier> stationsFlightModifier);
    }
}
