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
            var evt = new Event { Id = new Guid("11112222-3333-4444-5555-666677778888"), Name = "TestEvent",
                Description = "Testni opis", StartTime = new DateTime(2017, 7, 6), EndTime = new DateTime(2017, 7, 7),
                Latitude = 40.2, Longitude = 23.1, Categories = new List<int>() { 1, 4 }, Category = 5, Price = 12,
                Rating = 3.4,RateCount=10,Custom="Custom atributes",Capacity=120,Reserved=20,
                LocationId = new Guid("88887777-3333-4444-5555-666611112222")
            };

            mockEventRepository
                .Setup(er => er.CreateEventAsync(evt))
                .ReturnsAsync(evt);

            var eventService = new EventService(mockEventRepository.Object);
            var result = await eventService.CreateEventAsync(evt);
            var expected = await new Task<IEvent>(() => new Event { Id = new Guid("11112222-3333-4444-5555-666677778888"), Name = "TestEvent",
                Description = "Testni opis", StartTime = new DateTime(2017, 7, 6), EndTime = new DateTime(2017, 7, 7),
                Latitude = 40.2, Longitude = 23.1, Categories = new List<int>() { 1, 4 }, Category = 5, Price = 12,
                Rating = 3.4, RateCount = 10, Custom = "Custom atributes", Capacity = 120, Reserved = 20,
                LocationId = new Guid("88887777-3333-4444-5555-666611112222")
            });

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetEventsReturnsEvents()
        {
            var filter = new Mock<Filter>();
            var db = new List<IEvent>();
            var evt1 = new Event { Id = Guid.NewGuid(), Category = 10, Name = "Test1", Description = "Testt1", Latitude = 10.1, Longitude = 10.2, StartTime = new DateTime(), EndTime = new DateTime(),Price=10.1,Capacity=101,Rating=3.1,RateCount=11,Reserved=11,Custom="11",LocationId = Guid.NewGuid()};
            var evt2 = new Event { Id = Guid.NewGuid(), Category = 3, Name = "Test2", Description = "Testt2", Latitude = 10.2, Longitude = 10.3, StartTime = new DateTime(), EndTime = new DateTime(), Price = 10.2, Capacity = 102, Rating = 3.2, RateCount = 12, Reserved = 12, Custom = "12", LocationId = Guid.NewGuid() };
            var evt3 = new Event { Id = Guid.NewGuid(), Category = 0, Name = "Test3", Description = "Testt3", Latitude = 10.3, Longitude = 10.4, StartTime = new DateTime(), EndTime = new DateTime(), Price = 10.3, Capacity = 103, Rating = 3.3, RateCount = 13, Reserved = 13, Custom = "13", LocationId = Guid.NewGuid() };
            var evt4 = new Event { Id = Guid.NewGuid(), Category = 5, Name = "Test4", Description = "Testt4", Latitude = 10.4, Longitude = 10.5, StartTime = new DateTime(), EndTime = new DateTime(), Price = 10.4, Capacity = 104, Rating = 3.4, RateCount = 14, Reserved = 14, Custom = "14", LocationId = Guid.NewGuid() };

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