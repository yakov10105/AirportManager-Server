using AutoMapper;
using Final_Project.Server.BL.Events;
using Final_Project.Server.BL.Hubs;
using Final_Project.Server.Shared;
using Final_Project.Server.Shared.DTO;
using Final_Project.Server.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Text.Json;

namespace Final_Project.Server.BL.Services.HubNotifierService
{

    public class HubNotifierService : IHubNotifierService
    {
        private readonly IMapper _iMapper;
        private readonly IHubContext<FlightHub> _iHubContext;

        public HubNotifierService(IMapper iMapper, IHubContext<FlightHub> hubContext)
        {
            _iMapper = iMapper;
            _iHubContext= hubContext;
        }

        public void NotifyChanged(FlightEventArgs flightEventArgs)
        {
            if (flightEventArgs != null)
            {
                var flight = _iMapper.Map<FlightDTO>(flightEventArgs.Flight);
                StationDTO from = null, to = null;
                if (flightEventArgs.StationFrom != null)
                    from = _iMapper.Map<StationDTO>(flightEventArgs.StationFrom);
                if (flightEventArgs.StationTo != null)
                    to = _iMapper.Map<StationDTO>(flightEventArgs.StationTo);

                _iHubContext.Clients.All.SendAsync("FlightChanged",flight, from, to);
            }
            else
                throw new ArgumentNullException();
        }

        public void NotifyNewFlightAdded(Flight flight)
       {
            if(flight != null)
            {
                var dt = _iMapper.Map<FlightDTO>(flight);
           
                _iHubContext.Clients.All.SendAsync("NewFlightAdded", dt);
            }
        }
    }
}
