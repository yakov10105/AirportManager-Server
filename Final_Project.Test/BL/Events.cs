using Final_Project.Server.BL.Interfaces;
using Final_Project.Server.BL.Services.EventsSevice;
using Final_Project.Test.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project.Test.BL
{
    [TestClass]
    public class Events
    {
        [TestMethod]
        public void ShouldThrowNull1()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new EventsService(null, null));
        }
        [TestMethod]
        public void ShouldThrowNull2()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new EventsService(new HubNotifierMock(), null));
        }
        [TestMethod]
        public void ShouldThrowNull3()
        {
            var eventsService = new EventsService(new HubNotifierMock(), new DBUpdateServiceMock());
            Assert.ThrowsException<ArgumentNullException>(() => eventsService.AddStationsToEventListener(null));
        }
        [TestMethod]
        public void ShouldBeSameEvent()
        {
            var notifier = new HubNotifierMock();
            var dbService = new DBUpdateServiceMock();
            var eventsService = new EventsService(notifier, dbService);
            var stationServiceMock = new StationServiceMock();
            IFlightModifier[] stations = { stationServiceMock };

            Assert.IsNull(notifier.FlightNotification);

            eventsService.AddStationsToEventListener(stations);
            stationServiceMock.RaiseFlightEvent();

            Assert.IsTrue(notifier.FlightNotification != null);
            Assert.IsTrue(dbService.FlightEvent != null);

            Assert.IsTrue(notifier.FlightNotification.Equals(dbService.FlightEvent));
        }

    }
}
