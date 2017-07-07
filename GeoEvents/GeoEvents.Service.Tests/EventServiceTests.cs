using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Common;
using GeoEvents.Model;
using GeoEvents.Model.Mapping;
using Xunit;
using Moq;
using AutoMapper;
using GeoEvents.Repository.Common;

namespace GeoEvents.Service.Tests
{
    public class EventServiceTests
    {

        public EventServiceTests()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<ModelProfile>();
            });
        }
        
        [Theory]
        [InlineData("11112222-3333-4444-5555-666677778888", false)]
        [InlineData("51112222-3333-4444-5555-666677778888", true)]
        public void CreateEventReturnsFalseIfIdExists(string id, bool expected)
        {
            var mockEventRepository = new Mock<IEventRepository>();
            var evt = new Event { Id = new Guid(id), Name = "TestEvent", Description = "Testni opis", StartTime = new DateTime(2017, 7, 6), EndTime = new DateTime(2017, 7, 7), Lat = 40.2M, Long = 23.1M, Categories = new List<int>() { 1, 4 }, Category = 5 };
            var db = new List<IEventEntity>();
            db.Add(Mapper.Map<IEventEntity>(new Event { Id = new Guid("11112222-3333-4444-5555-666677778888"), Name = "TestEvent", Description = "Testni opis", StartTime = new DateTime(2017, 7, 6), EndTime = new DateTime(2017, 7, 7), Lat = 40.2M, Long = 23.1M, Categories = new List<int>() { 1, 4 }, Category = 5 }));

            mockEventRepository
                .Setup(er => er.CreateEvent(It.IsAny<IEventEntity>()))
                .Returns<IEventEntity>(x => !db.Exists(ee => ee.Id == x.Id));
            var eventService = new EventService(mockEventRepository.Object);
            var result = eventService.CreateEvent(evt);

            Assert.Equal(expected, result);

        }

        [Fact]
        public void GetEventsReturnsCorrectEvents()
        {
            var filter = new Filter(10.0M, 10.0M, 10.0M, new DateTime(), new DateTime(), 10);
            var evt1 = new Event { Category = 3 };
            var evt2 = new Event { Category = 10 };
            var evt3 = new Event { Category = 8 };
            var evt4 = new Event { Category = 127 };

            var db = new List<IEventEntity>();
            db.Add(Mapper.Map<IEventEntity>(evt1));
            db.Add(Mapper.Map<IEventEntity>(evt2));
            db.Add(Mapper.Map<IEventEntity>(evt3));
            db.Add(Mapper.Map<IEventEntity>(evt4));
            var mockEventRepository = new Mock<IEventRepository>();
            mockEventRepository
                .Setup(er => er.GetEvents(filter))
                .Returns(new List<IEventEntity>() { db[1], db[2], db[3] });
            var eventService = new EventService(mockEventRepository.Object);
            var results = eventService.GetEvents(filter);

            Assert.Equal( evt2.Category, results[0].Category);
            Assert.Equal(evt3.Category, results[1].Category);
            Assert.Equal(evt4.Category, results[2].Category);
        }

        [Fact]
        public void GetEventsCategoriesConversionIsCorrect()
        {
            var filter = new Filter(10.0M, 10.0M, 10.0M, new DateTime(), new DateTime(), 127);
            var db = new List<IEventEntity>();
            db.Add(Mapper.Map<IEventEntity>(new Event { Category = 3 }));
            db.Add(Mapper.Map<IEventEntity>(new Event { Category = 10 }));
            db.Add(Mapper.Map<IEventEntity>(new Event { Category = 8 }));
            db.Add(Mapper.Map<IEventEntity>(new Event { Category = 0 }));
            db.Add(Mapper.Map<IEventEntity>(new Event { Category = 127 }));
            var mockEventRepository = new Mock<IEventRepository>();
            mockEventRepository
                .Setup(er => er.GetEvents(It.IsAny<Filter>()))
                .Returns(db);
            var eventService = new EventService(mockEventRepository.Object);
            var results = eventService.GetEvents(filter);

            Assert.Equal(new List<int>() { 1, 2 }, results[0].Categories);
            Assert.Equal(new List<int>() { 2, 8 }, results[1].Categories);
            Assert.Equal(new List<int>() { 8 }, results[2].Categories);
            Assert.Equal(new List<int>() { }, results[3].Categories);
            Assert.Equal(new List<int>() { 1, 2, 4, 8, 16, 32, 64 }, results[4].Categories);
        }
    }
}
