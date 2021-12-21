using AutoMapper;
using Final_Project.Server.Shared.DTO;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.Shared.Mappers
{
    public class FlightProfile:Profile
    {
        public FlightProfile()
        {
            CreateMap<Flight, FlightDTO>();
            CreateMap<FlightDTO, Flight>();
        }
    }
}
