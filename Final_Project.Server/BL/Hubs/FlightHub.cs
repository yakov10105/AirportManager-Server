using AutoMapper;
using Final_Project.Server.BL.Services.AirportService;
using Final_Project.Server.Shared.DTO;
using Final_Project.Server.Shared.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Hubs
{
    public class FlightHub:Hub
    {
        //private readonly IMapper _iMapper;
        private readonly IAirportService _iAirportService;

        public FlightHub(IAirportService airportService)
        {
            _iAirportService = airportService;
        }

        public async Task AddToGroup(string controlTowerName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"{controlTowerName}");
        }
        public async Task<AirportDTO> GetAirportData()
        {
            return await Task.Run(() => _iAirportService.GetAirportData()); 
        }
    }
}
