using AutoMapper;
using Final_Project.Server.BL.Services.AirportSchemaService;
using Final_Project.Server.BL.Services.HubNotifierService;
using Final_Project.Server.DAL.Repo;
using Final_Project.Server.Shared.DTO;
using Final_Project.Server.Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.AirportService
{
    public class AirportService : IAirportService
    {

        private readonly IRepository<Airplane> _airplaneRepo;
        private readonly IRepository<Station> _stationRepo;
        private readonly IRepository<StationToStation> _stationToStationRepo;
        private readonly IRepository<ControlTower> _controlTowerRepo;
        private readonly IRepository<Flight> _flightRepo;
        private readonly IRepository<FlightHistory> _historyRepo;

        private readonly IAirportSchemaService _schemaService;
        private readonly IHubNotifierService _hubNotifier;
        private readonly IMapper _iMapper;

        public AirportService(IRepository<Airplane> airplaneRepo,
                              IRepository<Station> stationRepo,
                              IRepository<StationToStation> stationToStationRepo,
                              IRepository<ControlTower> controlTowerRepo,
                              IRepository<Flight> flightRepo,
                              IRepository<FlightHistory> historyRepo,
                              IAirportSchemaService schemaService,
                              IHubNotifierService hubNotifier,
                              IMapper mapper)
        {
            if (
                airplaneRepo == null || 
                stationRepo == null || 
                stationToStationRepo == null || 
                controlTowerRepo == null || 
                flightRepo==null ||
                historyRepo== null||
                schemaService==null ||
                hubNotifier==null
                )
                throw new ArgumentNullException();
            _airplaneRepo = airplaneRepo;
            _stationRepo = stationRepo;
            _stationToStationRepo = stationToStationRepo;
            _controlTowerRepo = controlTowerRepo;
            _flightRepo = flightRepo;
            _historyRepo = historyRepo;
            _schemaService = schemaService;
            _hubNotifier = hubNotifier;
            _iMapper = mapper;
            CreateSchema();
            if (!_schemaService.IsCreated)
            {
                ConnectDbHistoryToStations();
                ConnectWaitingFlightsFromDb();
            }
        }







        public AirportDTO GetAirportData()
        {
            var ct = _schemaService.ControlTowerService.ControlTower;
            if (ct != null)
            {
                var flights = GetFlights(ct.Id);
                var stations = ct.Stations.Select(s => _iMapper.Map<StationDTO>(s));
                var sts = GetStationToStations(ct.Stations);
                var firstStations = ct.ConnectedStations.Select(s => _iMapper.Map<StationToControlTowerDTO>(s));
                var controlTower = _iMapper.Map<ControlTowerDTO>(ct);
                return new AirportDTO { ControlTower = controlTower, Stations = stations, FirstStations = firstStations, Flights = flights, StationRelations = sts };
            }
            else throw new ArgumentNullException();
        }

        private IEnumerable<StationToStationDTO> GetStationToStations(ICollection<Station> stations)
        {
            try
            {
                return _stationToStationRepo.GetAll().AsEnumerable()
                        .Where(sr=>stations.Any(s=>sr.StationFromId==s.Id))
                        .Select(s=>_iMapper.Map<StationToStationDTO>(s));
            }
            catch (Exception)
            {

                return Enumerable.Empty<StationToStationDTO>();
            }
        }

        private IEnumerable<FlightDTO> GetFlights(int id)
        {
            try
            {
                return _flightRepo.GetAll()
                                    .Where(f => f.History.Count == 0 && f.ControlTowerId==id)
                                    .OrderBy(f => f.Time)
                                    .Select(f=>_iMapper.Map<FlightDTO>(f));
            }
            catch (Exception)
            {

                return Enumerable.Empty<FlightDTO>();
            }
        }

        public IEnumerable<Airplane> GetAllAirplanes()
        {
            try
            {
                return _airplaneRepo.GetAll();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IEnumerable<FlightHistoryDTO> GetFlightHistoryByStationId(int stationId)
        {
            Station station;
            try
            {
                station = _stationRepo.GetAll().FirstOrDefault(s => s.Id == stationId);
            }
            catch(Exception)
            {
                station=null;
            }
            if(station != null)
            {
                var history = station.History.OrderByDescending(s => s.EnterStationTime)
                                .Select(h => _iMapper.Map<FlightHistoryDTO>(h));
                return history;
            }
            else throw new Exception();
        }

        public async Task NewFlight(Flight flight)
        {
            try
            {
                if(_schemaService.ControlTowerService != null)
                {
                    flight.ControlTowerId = _schemaService.ControlTowerService.ControlTower.Id;
                    var flightToAdd = await _flightRepo.AddAsync(flight);
                    flight.ControlTower = _schemaService.ControlTowerService.ControlTower;
                    SendToControlTower(flightToAdd);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("==========New Flight Error=========");
                Debug.WriteLine(e.Message);
                Debug.WriteLine("===================================");

            }
        }



        private void ConnectDbHistoryToStations()
        {
            //handle Exceptions
            var inStationFlights = _historyRepo.GetAll()
                                   .AsEnumerable().GroupBy(x => x.StationId)
                                   .Where(group=> group.All(fh=>fh.LeaveStationTime.Value.Equals(false)))
                                   .Select(x => x.Last());
            _schemaService.InitFlightHistory(inStationFlights);
        }
        private void ConnectWaitingFlightsFromDb()
        {
            //handle exceptions
            var waitingFlights = _flightRepo.GetAll()
                                 .Where(f=>f.History.Count ==0)
                                 .OrderBy(f=>f.Time).AsEnumerable();
            if(waitingFlights==null)
                waitingFlights = Enumerable.Empty<Flight>();

            foreach(var flight in waitingFlights)
            {
                SendToControlTower(flight);
            }

        }

        private async void SendToControlTower(Flight flight)
        {
            if (flight != null)
            {
                if (_schemaService.ControlTowerService != null)
                {
                    var timeTillFlight = flight.Time - DateTime.Now;
                    _hubNotifier.NotifyNewFlightAdded(flight);
                    if(timeTillFlight > TimeSpan.Zero)
                        await Task.Delay(timeTillFlight);
                    _schemaService.ControlTowerService.NewFlightArrived(new FlightService.FlightService(flight));
                }
            }
        }

        private void CreateSchema()
        {
            try
            {
                var ct = _controlTowerRepo.GetAll().First();
                var stations = _stationRepo.GetAll();
                _schemaService.CreateSchema(ct, stations);
            }
            catch (Exception e)
            {
                Debug.WriteLine("==========Create Schema Error=========");
                Debug.WriteLine(e.Message);
                Debug.WriteLine("======================================");
            }
        }
    }
}
