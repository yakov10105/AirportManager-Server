using Final_Project.Server.BL.Interfaces;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.ControlTowerService
{
    public interface IControlTowerService: IConnectedToOtherStations,ICanReciveFlight,IFlightModifier
    {
        public ControlTower ControlTower{ get;}
    }
}
