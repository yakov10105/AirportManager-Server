using Final_Project.Server.Shared;
using Final_Project.Server.Shared.DTO;
//using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using System;

namespace Final_Project.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:8082/airport-socket")
                .WithAutomaticReconnect()
                .Build();

            connection.On<FlightDTO, StationDTO, StationDTO>("FlightChanged", (flight, from, to) =>
               {
                   if(from==null && to != null)
                        Console.WriteLine($"Flight number : {flight.Id} Moved from Control-tower To {to.Name} .");
                   else if(from!=null && to!=null)
                       Console.WriteLine($"Flight number : {flight.Id} Moved from {from.Name} To {to.Name} .");
                   else if(from !=null && to==null)
                       Console.WriteLine($"Flight number : {flight.Id} Moved from {from.Name} and completed the flight ! .");

               });


            connection.On<FlightDTO>("NewFlightAdded", flight =>
            {
                string dir = flight.Direction == FlightDirection.Arriving ? $"Landing from {flight.From}" : $"Taking-off to {flight.To}";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"New flight {dir} . Id : {flight.Id}");
                Console.ResetColor();
            });

            var t = connection.StartAsync();
            t.Wait();
            Console.Read();
        }
    }
}
