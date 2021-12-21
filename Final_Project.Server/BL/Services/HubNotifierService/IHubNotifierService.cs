using Final_Project.Server.BL.Events;
using Final_Project.Server.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.HubNotifierService
{
    public interface IHubNotifierService:INewFlightNotifier
    {
        void NotifyChanged(FlightEventArgs flightEventArgs);
    }
}
