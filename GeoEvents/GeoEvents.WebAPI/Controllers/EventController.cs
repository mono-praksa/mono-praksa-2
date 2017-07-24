using AutoMapper;
using GeoEvents.Common;
using GeoEvents.Model.Common;
using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeoEvents.WebAPI.Controllers
{
    [RoutePrefix("api/events")]
    public class EventController : ApiController
    {
        protected IEventService Service { get; private set; }
        protected IMapper Mapper { get; private set; }

        public EventController(IEventService service, IMapper mapper)
        {
            this.Service = service;
            this.Mapper = mapper;
        }

        //async
        [HttpPost]
        [Route("create")]
        public async Task<EventModel> CreateEvent([FromBody] EventModel evt)
        {
            evt.Id = new Guid();
            
            return Mapper.Map<EventModel>(await Service.CreateEventAsync(Mapper.Map<IEvent>(evt)));
        }

        //async

        [HttpGet]
        [Route("search")]
        public async Task<IEnumerable<EventModel>> GetEventsAsync(int pageNumber = 1, int pageSize = 25, string orderBy = "", bool orderAscending = false, int category = 0, decimal uLat = 1000M, decimal uLong = 1000M, decimal radius = 0, string startTime = "", string endTime = "", string searchString = "", decimal? price = null, decimal ratingEvent = 1, string custom = "")
        {
            Filter filter = new Filter(uLat, uLong, radius, null, null, category, pageNumber, pageSize, searchString, orderBy, orderAscending, price, ratingEvent, custom);
            DateTime dateValue;
            if (startTime != "")
            {
                DateTime.TryParse(startTime.Replace('h', ':'), out dateValue);
                filter.StartTime = dateValue;
            }
            if (endTime != "")
            {
                DateTime.TryParse(endTime.Replace('h', ':'), out dateValue);
                filter.EndTime = dateValue;
            }
            if (uLat == 1000M || uLong == 1000M)
            {
                filter.ULat = null;
                filter.ULong = null;
                filter.Radius = null;
            }

            return Mapper.Map<IEnumerable<EventModel>>(await Service.GetEventsAsync(filter));
        }

        [HttpGet]
        [Route("get")]
        public async Task<EventModel> GetEventByIdAsync(string eventId)
        {
            Guid eventIdGuid = new Guid(eventId);
            return Mapper.Map<EventModel>(await Service.GetEventByIdAsync(eventIdGuid));
        }

        [HttpGet]
        [Route("search/count")]
        public Task<Int64> GetEventCountAsync(int pageNumber = 1, int pageSize = 25, string orderBy = "", bool orderAscending = false, int category = 0, decimal uLat = 1000M, decimal uLong = 1000M, decimal radius = 0, string startTime = "", string endTime = "", string searchString = "", decimal? price = null, decimal ratingEvent = 1, string custom = "")
        {

            Filter filter = new Filter(uLat, uLong, radius, null, null, category, pageNumber, pageSize, searchString, orderBy, orderAscending, price, ratingEvent, custom);
            DateTime dateValue;
            if (startTime != "")
            {
                DateTime.TryParse(startTime.Replace('h', ':'), out dateValue);
                filter.StartTime = dateValue;
            }
            if (endTime != "")
            {
                DateTime.TryParse(endTime.Replace('h', ':'), out dateValue);
                filter.EndTime = dateValue;
            }
            if (uLat == 1000M || uLong == 1000M)
            {
                filter.ULat = null;
                filter.ULong = null;
                filter.Radius = null;
            }

            return Service.GetEventCountAsync(filter);
        }

        [HttpPut]
        [Route("update/rating")]
        public Task<IEvent> UpdateRatingAsync(Guid eventId, double rating, double CurrentRating, int RateCount)
        {
            return Service.UpdateRatingAsync(eventId, rating,CurrentRating,RateCount);
        }

        [HttpPut]
        [Route("update/reservation")]
        public Task<IEvent> UpdateReservationAsync(Guid eventId)
        {
            return Service.UpdateReservationAsync(eventId);
        }
    }

    public class EventModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public List<int> Categories { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int Reserved { get; set; }
        public decimal Rating { get; set; }
        public int RateCount { get; set; }
        public decimal RatingLocation { get; set; }
        public string Custom { get; set; }
        public Guid LocationId { get; set; }

        public EventModel(Guid id, string name, string description, DateTime starttime, DateTime endtime, decimal uLat, decimal uLong, List<int> categories, decimal price, int capacity, int reserved, decimal rating, int rateCount, decimal ratingLocation, string custom, Guid locationId)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.StartTime = starttime;
            this.EndTime = endtime;
            this.Latitude = uLat;
            this.Longitude = uLong;
            this.Categories = categories;
            this.Price = price;
            this.Capacity = capacity;
            this.Reserved = reserved;
            this.Rating = rating;
            this.RateCount = rateCount;
            this.RatingLocation = ratingLocation;
            this.Custom = custom;
            this.LocationId = locationId;
        }

        public EventModel()
        {
        }
    }
}

