using Final_Project.Server.Shared.DTO;
using Final_Project.Server.Shared.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Simulator.Services.ServerConnectionService
{
    public class ServerConnectionService : IServerConnectionService
    {
        private readonly HttpClient _client;

        public ServerConnectionService(IHttpClientFactory clientFactory)
        {

            _client = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:8082")
            };
        }
        public async Task PostFlight(FlightDTO flight)
        {
            try
            {
                await _client.PostAsJsonAsync("api/Airport", flight);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<ICollection<AirplaneDTO>> GetAirplanes()
        {
            ICollection<AirplaneDTO> airplanes = null;
            try
            {
                var res = await _client.GetAsync("api/Airport/airplanes");
                if (res.IsSuccessStatusCode)
                    airplanes = await res.Content.ReadAsAsync<ICollection<AirplaneDTO>>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                airplanes = Array.Empty<AirplaneDTO>();
            }
            return airplanes;
        }

    }
}
