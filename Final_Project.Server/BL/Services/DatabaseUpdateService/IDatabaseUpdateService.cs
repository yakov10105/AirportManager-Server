using Final_Project.Server.BL.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.DatabaseUpdateService
{
    public interface IDatabaseUpdateService
    {
        Task FlightMoved(FlightEventArgs eventArgs);
    }
}
