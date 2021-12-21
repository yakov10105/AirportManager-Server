using Final_Project.Server.BL.Events;
using Final_Project.Server.DAL.Repo;
using Final_Project.Server.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Server.BL.Services.DatabaseUpdateService
{
    public class DatabaseUpdateService : IDatabaseUpdateService
    {
        private readonly IRepository<Flight> _flightRepo;
        private readonly IRepository<Station> _stationRepo;

        public DatabaseUpdateService(IRepository<Flight> flightRepo,IRepository<Station> stationRepo)
        {
            _flightRepo = flightRepo;
            _stationRepo = stationRepo;
        }


        public async Task FlightMoved(FlightEventArgs eventArgs)
        {
            var flight = eventArgs.Flight;
            var from = eventArgs.StationFrom;
            //delete flight from current station
            if (from != null) from.CurrentFlightId = null;
            var to = eventArgs.StationTo;
            //add flight to next station
            if (to != null) to.CurrentFlightId = flight.Id;


            if (eventArgs.IsToEqualsFrom)
                return;

            //add new flight history to the next station
            if (eventArgs.IsArrivingToFirstStation)
                AddFlightHistory(flight, eventArgs.StationTo);
            //complete the current station flight history
            else if (eventArgs.IsLeavingLastStation)
                FinishFlightHistory(flight, eventArgs.StationFrom);
            //complete current station history and add new one to the next station
            else
            {
                FinishFlightHistory(flight, eventArgs.StationFrom);
                AddFlightHistory(flight, eventArgs.StationTo);
            }

            flight.StationId = eventArgs.StationTo?.Id;

            //using (IServiceScope scope = _serviceScope.CreateScope())
            //{
            //    try
            //    {
            //        var flightRepo = scope.ServiceProvider.GetRequiredService<IRepository<Flight>>();
            //        var stationsRepo = scope.ServiceProvider.GetRequiredService<IRepository<Station>>();

            //        //List<Task> tasks = new() { flightRepo.UpdateAsync(flight) };
            //        //if (from != null) tasks.Add(stationsRepo.UpdateAsync(from));
            //        //if (to != null) tasks.Add(stationsRepo.UpdateAsync(to));
            //        //Task.WaitAll(tasks.ToArray());

            //        await flightRepo.UpdateAsync(flight);
            //        if (from != null) await stationsRepo.UpdateAsync(from);
            //        if (to != null) await stationsRepo.UpdateAsync(to);
            //        await flightRepo.UpdateAsync(flight);
            //    }
            //    catch (Exception e)
            //    {
            //        Debug.WriteLine("=======DB UpdateService Error=======");
            //        Debug.WriteLine(e.Message);
            //        Debug.WriteLine("====================================");
            //        return;
            //    }
            //}
            try
            {
                await _flightRepo.UpdateAsync(flight);
                if (from != null) await _stationRepo.UpdateAsync(from);
                if (to != null) await _stationRepo.UpdateAsync(to);
                await _flightRepo.UpdateAsync(flight);
            }
            catch (Exception e)
            {
                Debug.WriteLine("=======DB UpdateService Error=======");
                Debug.WriteLine(e.Message);
                Debug.WriteLine("====================================");
                return;
            }
        }

        private static void FinishFlightHistory(Flight flight,Station stationFrom)
        {
            var history = flight.History.FirstOrDefault(fh => fh.StationId == stationFrom.Id && fh.LeaveStationTime.HasValue.Equals(false));
           if(history != null)
                history.LeaveStationTime=DateTime.Now;
        }

        private static void AddFlightHistory(Flight flight, Station stationTo)
        {
            var history = new FlightHistory { StationId=stationTo.Id,EnterStationTime=DateTime.Now };
            if (flight.History == null)
                flight.History = new List<FlightHistory>();
            flight.History.Add(history);
        }
    }
}
