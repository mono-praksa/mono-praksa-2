using GeoEvents.Common;
using GeoEvents.Model;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GeoEvents.Service.Tests
{
    public class EventServiceTests
    {
        [Fact]
        public async Task CreateEventReturnsEvent()
        {
            var mockEventRepository = new Mock<IEventRepository>();
            var evt = new Event { Id = new Guid("11112222-3333-4444-5555-666677778888"), Name = "TestEvent", Description = "Testni opis", StartTime = new DateTime(2017, 7, 6), EndTime = new DateTime(2017, 7, 7), Lat = 40.2M, Long = 23.1M, Categories = new List<int>() { 1, 4 } };

            mockEventRepository
                .Setup(er => er.CreateEventAsync(evt))
                .ReturnsAsync(evt);

            var eventService = new EventService(mockEventRepository.Object);
            var result = await eventService.CreateEventAsync(evt);
            var expected = await new Task<IEvent>(() => new Event { Id = new Guid("11112222-3333-4444-5555-666677778888"), Name = "TestEvent", Description = "Testni opis", StartTime = new DateTime(2017, 7, 6), EndTime = new DateTime(2017, 7, 7), Lat = 40.2M, Long = 23.1M, Categories = new List<int>() { 1, 4 }, Category = 5 });

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetEventsReturnsEvents()
        {
            var filter = new Mock<Filter>();

            var db = new List<IEvent>();
            var evt1 = new Event { Id = Guid.NewGuid(), Category = 10, Name = "Test1", Description = "Testt1", Lat = 10.1M, Long = 10.2M, StartTime = new DateTime(), EndTime = new DateTime() };
            var evt2 = new Event { Id = Guid.NewGuid(), Category = 3, Name = "Test2", Description = "Testt2", Lat = 10.2M, Long = 10.3M, StartTime = new DateTime(), EndTime = new DateTime() };
            var evt3 = new Event { Id = Guid.NewGuid(), Category = 0, Name = "Test3", Description = "Testt3", Lat = 10.3M, Long = 10.4M, StartTime = new DateTime(), EndTime = new DateTime() };
            var evt4 = new Event { Id = Guid.NewGuid(), Category = 5, Name = "Test4", Description = "Testt4", Lat = 10.4M, Long = 10.5M, StartTime = new DateTime(), EndTime = new DateTime() };

            db.Add(evt1);
            db.Add(evt2);
            db.Add(evt3);
            db.Add(evt4);

            var expectedDb = new List<IEvent>();
            evt1.Categories = new List<int>() { 2, 8 };
            evt2.Categories = new List<int>() { 1, 2 };
            evt3.Categories = new List<int>();
            evt4.Categories = new List<int>() { 1, 4 };

            expectedDb.Add(evt1);
            expectedDb.Add(evt2);
            expectedDb.Add(evt3);
            expectedDb.Add(evt4);

            var mockEventRepository = new Mock<IEventRepository>();
            mockEventRepository
                .Setup(er => er.GetEventsAsync(filter.Object))
                .ReturnsAsync(() => db);
            var eventService = new EventService(mockEventRepository.Object);
            var result = await eventService.GetEventsAsync(filter.Object);
            var expected = await new Task<List<IEvent>>(() => expectedDb);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetEventCountReturnsCount()
        {
            var filter = new Mock<IFilter>();
            var repoReturn = 7;

            var mockEventRepository = new Mock<IEventRepository>();
            mockEventRepository
                .Setup(er => er.GetEventCountAsync(filter.Object))
                .ReturnsAsync(() => repoReturn);

            var eventService = new EventService(mockEventRepository.Object);
            var result = await eventService.GetEventCountAsync(filter.Object);
            var expected = await new Task<int>(() => repoReturn);

            Assert.Equal(expected, result);
        }
    }
}