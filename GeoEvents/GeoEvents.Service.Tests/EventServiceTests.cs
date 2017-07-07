using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Common;
using GeoEvents.Model;
using Xunit;
using GeoEvents.Repository.Common;

namespace GeoEvents.Service.Tests
{
    public class EventServiceTests
    {
        private EventService eventService;
        private Moq.Mock<IEventRepository> eventRepositoryMock;

        [Fact]
        public void CreateEventInvalidKeyReturnsFalse()
        {

            Event evt = new Event{ Id=new Guid("11112222-3333-4444-5555-666677778888"), Name="TestEvent", Description="Testni opis", StartTime=new DateTime(2017, 7, 6), EndTime=new DateTime(2017, 7, 7), Lat=40.2M, Long=23.1M, Categories = {1, 4}, Category=5 };
            List<Event> db = new List<Event>();
            db.Add(new Event { Id = new Guid("11112222-3333-4444-5555-666677778888"), Name = "TestEvent2", Description = "Testni opis", StartTime = new DateTime(2017, 7, 6), EndTime = new DateTime(2017, 7, 7), Lat = 40.2M, Long = 23.1M, Categories = { 1, 4 }, Category = 5 });
            db.Add(new Event { Id = new Guid("12112222-3333-4444-5555-666677778888"), Name = "TestEvent3", Description = "Testni opis", StartTime = new DateTime(2017, 7, 6), EndTime = new DateTime(2017, 7, 7), Lat = 40.2M, Long = 23.1M, Categories = { 1, 4 }, Category = 5 });
            eventRepositoryMock = new Moq.Mock<IEventRepository>("WSHttpBinding_IMyService");
            eventService = new EventService(eventRepositoryMock);
            bool actual =;

            Assert.Equal(false, actual);
        }

    }
}
