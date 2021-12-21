using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Interfaces
{
    public interface IFlightHandler:IFlightModifier,ICanReciveFlight
    {
        public Station Station{ get;}
        public bool IsHandlerAvailable { get; }
    }
}
