using AutoMapper;
using GeoEvents.Common;
using GeoEvents.Model.Common;
using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Net;


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

        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateEvent([FromBody] EventModel evt)
        {
            evt.Id = new Guid();

            var result = await Service.CreateEventAsync(Mapper.Map<IEvent>(evt));

            if (result.Id == new Guid())
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "failed to create the event");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Created, result);
            }
        }

        [HttpGet]
        [Route("search")]
        public async Task<HttpResponseMessage> GetEventsAsync([FromUri] IFilter filter)
        {
            var result = Mapper.Map<IEnumerable<EventModel>>(await Service.GetEventsAsync(filter));

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> GetEventByIdAsync(string eventId)
        {
            Guid eventIdGuid;
            if (Guid.TryParse(eventId, out eventIdGuid))
            {
                eventIdGuid = Guid.Parse(eventId);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid guid");
            }
            var result = Mapper.Map<EventModel>(await Service.GetEventByIdAsync(eventIdGuid));

            if (result.Id == eventIdGuid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "could not find an event with the requested id");
            }
        }

        [HttpGet]
        [Route("search/count")]
        public async Task<HttpResponseMessage> GetEventCountAsync(int pageNumber = 1, int pageSize = 25, string orderBy = "", bool orderAscending = false, int category = 0, decimal uLat = 1000M, decimal uLong = 1000M, decimal radius = 0, string startTime = "", string endTime = "", string searchString = "", decimal? price = null, decimal ratingEvent = 0, string custom = "")
        {

            Filter filter = new Filter(uLat, uLong, radius, null, null, category, pageNumber, pageSize, searchString, orderBy, orderAscending, price, ratingEvent, custom);
            DateTime dateValue;
            if (startTime != "")
            {
                if (DateTime.TryParse(startTime.Replace('h', ':'), out dateValue))
                {
                    filter.StartTime = dateValue;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Incorrect start time format");
                }
            }
            if (endTime != "")
            {
                if (DateTime.TryParse(endTime.Replace('h', ':'), out dateValue))
                {
                    filter.EndTime = dateValue;
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Incorrect end time format");
                }
            }
            if (uLat == 1000M || uLong == 1000M)
            {
                filter.ULat = null;
                filter.ULong = null;
                filter.Radius = null;
            }

            var result = await Service.GetEventCountAsync(filter);
            return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [HttpPut]
        [Route("update/rating")]
        public async Task<HttpResponseMessage> UpdateRatingAsync(Guid eventId, double rating, double CurrentRating, int RateCount)
        {
            var result = await Service.UpdateRatingAsync(eventId, rating, CurrentRating, RateCount);

            if(result.RateCount == RateCount + 1)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "failed to update rating");
            }
        }

        [HttpPut]
        [Route("update/reservation")]
        public async Task<HttpResponseMessage> UpdateReservationAsync(Guid eventId)
        {
            var result = await Service.UpdateReservationAsync(eventId);

            return Request.CreateResponse(HttpStatusCode.OK, result);
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

