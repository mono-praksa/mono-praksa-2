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
    [RoutePrefix("api/event")]
    public class EventsController : ApiController
    {
        protected IEventService Service { get; private set; }

        public EventsController(IEventService service)
        {
            this.Service = service;
        }

        //[HttpPost]
        //[Route("create/{name}/{description}/{longitude}/{latitude}/{categories}/{startTime}/{endTime}")]
        //public bool CreateEvent(string name, string description, decimal longitude, decimal latitude, List<int> categories, DateTime startTime, DateTime endTime)
        [HttpPost]
        [Route("create")]
        public bool CreateEvent([FromBody] EventsViewModel evt)
        {
            //EventsViewModel evt = new EventsViewModel(Guid.NewGuid(), "Name", "desc", new DateTime(2017,5,7), new DateTime(2017,5,10), 45, 45, new List<int>() { 1,2,4 });
            evt.Id = Guid.NewGuid();
            return Service.CreateEvent(Mapper.Map<IEvent>(evt));
            //return true;
        }

        [HttpGet]
        //[Route("get/{latitude}/{longitude}/{radius}/{startTime}/{endTime}")]
        [Route("search/{ULat}/{ULong}/{Radius}/{Category}/{StartTime}/{EndTime}")]
        public List<EventsViewModel> GetEvents(decimal ULat, decimal ULong, decimal Radius, int Category, string StartTime, string EndTime)
        {
            Filter filter = new Filter(ULat, ULong, Radius,  DateTime.Parse(StartTime.Replace('h', ':')), DateTime.Parse(EndTime.Replace('h', ':')), Category);

            return Mapper.Map<List<EventsViewModel>>(Service.GetEvents(filter));
        }
    }
}
