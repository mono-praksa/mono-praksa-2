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
            Filter filter = new Filter(40.71M,74.00M,3000000,new DateTime(2017-07-05),new DateTime(2017-07-07),1);

            return Mapper.Map<List<EventsViewModel>>(Service.GetEvents(filter));

            //return Data.GenerateMockData();
        }
    }
}
