using AutoMapper;
using Final_Project.Server.BL.Services.AirportService;
using Final_Project.Server.Shared.DTO;
using Final_Project.Server.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Final_Project.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : ControllerBase
    {
        private readonly IMapper _iMapper;
        private readonly IAirportService _iAirportService;

        public AirportController(IMapper mapper,  IAirportService airportService)
        {
            _iMapper = mapper;
            _iAirportService = airportService;
        }

        [HttpGet("airport-data")]
        public IActionResult GetAirportData()
        {
            try
            {
                var dto = _iAirportService.GetAirportData();
                if (dto == null) return BadRequest();
                return Ok(JsonSerializer.Serialize(dto));
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return BadRequest();
            }
        }
        [HttpGet("airplanes")]
        public IActionResult GetAirplanes()
        {
            try
            {
                var airplanesDTOList = new List<AirplaneDTO>();
                var airplanes = _iAirportService.GetAllAirplanes();
                foreach (var item in airplanes)
                {
                    airplanesDTOList.Add(_iMapper.Map<AirplaneDTO>(item));
                }
                if (airplanesDTOList == null) return BadRequest();
                return Ok(airplanesDTOList);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return BadRequest();
            }
        }
        [HttpPost]
        public IActionResult Post([FromBody] FlightDTO flightDto )
        {
            var flight = _iMapper.Map<Flight>(flightDto);
            try
            {
                _iAirportService.NewFlight(flight);
                return Created("flight added",flight);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
