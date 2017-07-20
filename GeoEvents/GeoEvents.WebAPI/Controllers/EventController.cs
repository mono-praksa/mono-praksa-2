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

        //[HttpPost]
        //[Route("create")]
        //public bool CreateEvent([FromBody] EventModel evt)
        //{
        //    evt.Id = Guid.NewGuid();
        //    return Service.CreateEvent(Mapper.Map<IEvent>(evt));
        //}

        //async
        [HttpPost]
        [Route("create")]
        public async Task<EventModel> CreateEvent([FromBody] EventModel evt)
        {
            evt.Id = new Guid();
            return Mapper.Map<EventModel>(await Service.CreateEventAsync(Mapper.Map<IEvent>(evt)));
        }

        //[HttpGet]
        //[Route(@"search/{ULat:decimal}/{ULong:decimal}/{Radius:decimal}/{Category:int}/{StartTime:regex(^\d{4}-\d{2}-\d{2} \d{2}h\d{2}$)}/{EndTime:regex(^\d{4}-\d{2}-\d{2} \d{2}h\d{2}$)}")]
        //public List<EventModel> SearchEvents(decimal ULat, decimal ULong, decimal Radius, int Category, string StartTime, string EndTime)
        //{
        //    Filter filter = new Filter(ULat, ULong, Radius, DateTime.Parse(StartTime.Replace('h', ':')), DateTime.Parse(EndTime.Replace('h', ':')), Category);

        //    return Mapper.Map<List<EventModel>>(Service.GetEvents(filter));
        //}

        //async

        [HttpGet]
        [Route("search")]
        public async Task<IEnumerable<EventModel>> GetEventsAsync(int pageNumber = 1, int pageSize = 25, string orderBy = "", bool orderAscending = false, int category = 0, decimal uLat = 1000M, decimal uLong = 1000M, decimal radius = 0, string startTime = "", string endTime = "", string searchString = "", decimal? price = null, decimal? ratingEvent = null)
        {
            Filter filter = new Filter(uLat, uLong, radius, null, null, category, pageNumber, pageSize, searchString, orderBy, orderAscending, price, ratingEvent);
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

        //[HttpGet]
        //[Route("get")]
        //public async Task<EventModel> GetEventByIdAsync(Guid eventId) {
        //    return Mapper.Map<EventModel>(await Service.GetEventByIdAsync(eventId));
        //}

        //[HttpGet]
        //[Route(@"search/{pageNumber:int}/{pageSize:int}/{orderBy}/{orderAscending:bool}/{category:int}/{uLat:decimal}/{uLong:decimal}/{radius:decimal}/{startTime:regex(^s\d{4}-\d{2}-\d{2} \d{2}h\d{2}$)?}/{endTime:regex(^e\d{4}-\d{2}-\d{2} \d{2}h\d{2}$)?}/{searchString?}/{nameOnly:bool?}")]
        //public async Task<IEnumerable<EventModel>> GetEventsAsync(int pageNumber, int pageSize, string orderBy, bool orderAscending, int category, decimal uLat, decimal uLong, decimal radius, string startTime = "", string endTime = "", string searchString = "", bool? nameOnly = true )
        //{
        //    Filter filter;
        //    if (startTime != "" && endTime != "")
        //    {
        //        filter = new Filter(uLat, uLong, radius, DateTime.Parse(startTime.Replace('h', ':').Remove(0, 1)), DateTime.Parse(endTime.Replace('h', ':').Remove(0, 1)), category, pageNumber, pageSize, searchString, orderBy, orderAscending);
        //    }
        //    else
        //    {
        //        filter = new Filter(uLat, uLong, radius, new DateTime(), new DateTime(), category, pageNumber, pageSize, searchString, orderBy, orderAscending);
        //    }
        //    return Mapper.Map<List<EventModel>>(await Service.GetEventsAsync(filter));
        //}

        [HttpGet]
        [Route("search/count")]
        public Task<Int64> GetEventCountAsync(int pageNumber = 1, int pageSize = 25, string orderBy = "", bool orderAscending = false, int category = 0, decimal uLat = 1000M, decimal uLong = 1000M, decimal radius = 0, string startTime = "", string endTime = "", string searchString = "", decimal? price = null, decimal? ratingEvent = null)
        {
            Filter filter = new Filter(uLat, uLong, radius, null, null, category, pageNumber, pageSize, searchString, orderBy, orderAscending, price, ratingEvent);
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
        public Task<IEvent> UpdateRatingAsync(Guid eventId, decimal rating)
        {
            return Service.UpdateRatingAsync(eventId, rating);
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
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public List<int> Categories { get; set; }
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public int Reserved { get; set; }
        public decimal Rating { get; set; }
        public int RateCount { get; set; }
        public decimal RatingLocation { get; set; }

        public EventModel(Guid id, string name, string description, DateTime starttime, DateTime endtime, decimal uLat, decimal uLong, List<int> categories, decimal price, int capacity, int reserved, decimal rating, int rateCount, decimal ratingLocation)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.StartTime = starttime;
            this.EndTime = endtime;
            this.Lat = uLat;
            this.Long = uLong;
            this.Categories = categories;
            this.Price = price;
            this.Capacity = capacity;
            this.Reserved = reserved;
            this.Rating = rating;
            this.RateCount = rateCount;
            this.RatingLocation = ratingLocation;
        }

        public EventModel()
        {
        }
    }
}

