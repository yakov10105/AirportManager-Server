using Final_Project.Server.Shared;
using Final_Project.Server.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace Final_Project.Server.DAL.Extentions
{
    public static class DataSeed
    {
        private static ControlTower CONTROL_TOWER = new ControlTower { Id = 1, Name = "TLV" };
        private static List<Airplane> AIRPLANES = new List<Airplane>
        {
            new Airplane{ Id = 1 ,Airline="Arkia"},
            new Airplane{Id=2,Airline="Aeroflot"},
            new Airplane{Id=3,Airline="United"},
            new Airplane{Id =4,Airline="American-Airlines"}
        };
        private static List<Station> STATIONS = new List<Station>
        {
            new Station{ Id = 1 ,ControlTowerId = 1, Name="Landing Station 1"},
            new Station{ Id = 2, ControlTowerId = 1, Name="Landing Station 2" },
            new Station{ Id = 3 ,ControlTowerId = 1, Name="Landing Station 3" },
            new Station{ Id = 4 ,ControlTowerId = 1, Name="Landing/Takeoff Strip 4" },
            new Station{ Id = 5 ,ControlTowerId = 1, Name="Route to gates 5" },
            new Station{ Id = 6 ,ControlTowerId = 1, Name="Gate 6" },
            new Station{ Id = 7 ,ControlTowerId = 1, Name="Gate 7" },
            new Station{ Id = 8 ,ControlTowerId = 1, Name="Route to takeoff 8" },
            new Station{ Id = 9 ,ControlTowerId = 1, Name="Take-off Station 1" },
            new Station{ Id = 10 ,ControlTowerId = 1, Name="Wheel Folding 10" }
        };
        private static List<StationToStation> STATION_TO_STATION = new List<StationToStation>
        {
            new StationToStation{ Direction=FlightDirection.Arriving , StationFromId = 1,StationToId=2},
            new StationToStation{ Direction=FlightDirection.Arriving , StationFromId = 2,StationToId=3},
            new StationToStation{ Direction=FlightDirection.Arriving , StationFromId = 3,StationToId=4},
            new StationToStation{ Direction=FlightDirection.Arriving , StationFromId = 4,StationToId=5},
            new StationToStation{ Direction=FlightDirection.Arriving , StationFromId = 5,StationToId=6},
            new StationToStation{ Direction=FlightDirection.Arriving , StationFromId = 5,StationToId=7},
            new StationToStation{ Direction=FlightDirection.Departuring , StationFromId = 9,StationToId=6},
            new StationToStation{ Direction=FlightDirection.Departuring , StationFromId = 9,StationToId=7},
            new StationToStation{ Direction=FlightDirection.Departuring , StationFromId = 6,StationToId=8},
            new StationToStation{ Direction=FlightDirection.Departuring , StationFromId = 7,StationToId=8},
            new StationToStation{ Direction=FlightDirection.Departuring , StationFromId = 8,StationToId=4},
            new StationToStation{ Direction=FlightDirection.Departuring , StationFromId = 4,StationToId=10}
        };
        private static List<StationToControlTower> STATION_TO_CONTROL_TOWER = new List<StationToControlTower>
        {
            new StationToControlTower{Direction=FlightDirection.Arriving,ControlTowerId=1,StationToId=1},
            new StationToControlTower{Direction=FlightDirection.Departuring,ControlTowerId=1,StationToId=9}
        };

        public static void SeedDataOnCreate(this ModelBuilder builder)
        {
            builder.Entity<Airplane>().HasData(AIRPLANES);
            //builder.Entity<Flight>().HasData(FLIGHTS);
            builder.Entity<Station>().HasData(STATIONS);
            builder.Entity<StationToStation>().HasData(STATION_TO_STATION);
            //builder.Entity<FlightHistory>().HasData(FLIGHT_HISTORY);
            builder.Entity<ControlTower>().HasData(CONTROL_TOWER);
            builder.Entity<StationToControlTower>().HasData(STATION_TO_CONTROL_TOWER);
        }
    }
}
