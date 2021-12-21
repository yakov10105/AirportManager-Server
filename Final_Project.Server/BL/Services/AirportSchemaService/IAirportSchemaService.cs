using Final_Project.Server.BL.Services.ControlTowerService;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.AirportSchemaService
{
    public interface IAirportSchemaService
    {
        public IControlTowerService ControlTowerService { get; }

        public bool IsCreated { get;}

        public void CreateSchema(ControlTower controlTower, IEnumerable<Station> stations);

        public void InitFlightHistory(IEnumerable<FlightHistory> history);
    }
}
