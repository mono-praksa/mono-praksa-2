using GeoEvents.Model.Common;
using GeoEvents.Service.Common;
using GeoEvents.Common;
using GeoEvents.WebAPI.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeoEvents.WebAPI.Controllers
{
    [RoutePrefix("api/event")]
    public class EventsController : ApiController
    {
        public MockData Data;
        public IEventService Service { get; set; }
        public EventsController(IEventService service)
        {
            this.Service = service;
            Data = new MockData();
        }

        //[HttpPost]
        //[Route("create/{name}/{description}/{longitude}/{latitude}/{categories}/{startTime}/{endTime}")]
        //public bool CreateEvent(string name, string description, decimal longitude, decimal latitude, List<int> categories, DateTime startTime, DateTime endTime)
        public bool CreateEvent()
        {
            //Event evt = new Event(0, name, description, longitude, latitude, categories, startTime, endTime);
            //return Service.CreateEvent(evt);
            return true;
        }

        [HttpGet]
        //[Route("get/{latitude}/{longitude}/{radius}/{startTime}/{endTime}")]
        [Route("mockdata")]
        //public List<IEvent> GetEvents(decimal latitude, decimal longitude, decimal radius, DateTime startTime, DateTime endTime)
        public List<EventsViewModel> GetEvents()
        {
            //Filter filter = new Filter(latitude, longitude, radius, startTime, endTime);
            //return Service.GetEvents(filter);

            return Data.GenerateMockData();
        }
    }
}
