using GeoEvents.Model.Common;
using GeoEvents.Service.Common;
using GeoEvents.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;

namespace GeoEvents.WebAPI.Controllers
{
    [RoutePrefix("api/events")]
    public class EventController : ApiController
    {
        protected IEventService Service { get; private set; }

        public EventController(IEventService service)
        {
            this.Service = service;
        }

        [HttpPost]
        [Route("create")]
        public bool CreateEvent([FromBody] EventModel evt)
        {
            evt.Id = Guid.NewGuid();
            return Service.CreateEvent(Mapper.Map<IEvent>(evt));
        }

        //async
        //[HttpPost]
        //[Route("create")]
        //public async Task<EventModel> CreateEvent([FromBody] EventModel evt)
        //{
        //    evt.Id = new Guid();
        //    return await Service.CreateEvent(Mapper.Map<IEvent>(evt));
        //}

        [HttpGet]
        [Route("search/{ULat}/{ULong}/{Radius}/{Category}/{StartTime}/{EndTime}")]
        public List<EventModel> SearchEvents(decimal ULat, decimal ULong, decimal Radius, int Category, string StartTime, string EndTime)
        {
            Filter filter = new Filter(ULat, ULong, Radius, DateTime.Parse(StartTime.Replace('h', ':')), DateTime.Parse(EndTime.Replace('h', ':')), Category);

            return Mapper.Map<List<EventModel>>(Service.GetEvents(filter));
        }

        //async
        //[HttpGet]
        //[Route("search/{ULat}/{ULong}/{Radius}/{Category}/{StartTime}/{EndTime}")]
        //public async Task<IEnumerable<EventModel>> GetEventsAsync(decimal ULat, decimal ULong, decimal Radius, int Category, string StartTime, string EndTime)
        //{
        //    Filter filter = new Filter(ULat, ULong, Radius, DateTime.Parse(StartTime.Replace('h', ':')), DateTime.Parse(EndTime.Replace('h', ':')), Category);

        //    return Mapper.Map<List<EventModel>>(await Service.GetEvents(filter));
        //}
    }
}
