using Final_Project.Server.BL.Events;
using Final_Project.Server.BL.Interfaces;
using Final_Project.Server.BL.Services.DatabaseUpdateService;
using Final_Project.Server.BL.Services.HubNotifierService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Final_Project.Server.BL.Services.EventsSevice
{
    public class EventsService : IEventsService
    {
        private readonly IHubNotifierService _iNotifier;
        private readonly IDatabaseUpdateService _updateService;
        private IEnumerable<IFlightModifier> _modifiers = Enumerable.Empty<IFlightModifier>();


        public EventsService(IHubNotifierService hubNotifier, IDatabaseUpdateService updateService)
        {
            if (hubNotifier != null)_iNotifier = hubNotifier;
            else throw new ArgumentNullException();
            if(updateService != null) _updateService = updateService;
            else throw new ArgumentNullException();
        }


        public void AddStationsToEventListener(IEnumerable<IFlightModifier> stationsFlightModifier)
        {
            if (stationsFlightModifier != null)
            {
                var newModifiers = stationsFlightModifier.Where(fm => _modifiers.Any(fm2 => fm == fm2).Equals(false));
                foreach (var station in newModifiers)
                    station.FlightEvent += FlightEvent;
                _modifiers = _modifiers.Concat(newModifiers);
            }
            else throw new ArgumentNullException();
        }


        private void FlightEvent(object sender ,FlightEventArgs e)
        {
            if (e != null)
            {
                _iNotifier.NotifyChanged(e);
                _updateService.FlightMoved(e);
            }
            else
                throw new ArgumentNullException();
        }
    }
}
