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
        [HttpGet]
        [Route("create")]
        public bool CreateEvent()
        {
            EventsViewModel evt = new EventsViewModel(Guid.NewGuid(), "Name", "desc", new DateTime(2017,5,7), new DateTime(2017,5,10), 45, 45, new List<int>() { 1,2,4 });
            return Service.CreateEvent(Mapper.Map<IEvent>(evt));
        }

        [HttpGet]
        //[Route("get/{latitude}/{longitude}/{radius}/{startTime}/{endTime}")]
        [Route("mockdata")]
        //public List<IEvent> GetEvents(decimal latitude, decimal longitude, decimal radius, DateTime startTime, DateTime endTime)
        public List<EventsViewModel> GetEvents()
        {
            //Filter filter = new Filter(latitude, longitude, radius, startTime, endTime);
            Filter filter = new Filter(40.71M,18.7M,3000,new DateTime(2017, 07, 05),new DateTime(2017, 07, 07),2);

            return Mapper.Map<List<EventsViewModel>>(Service.GetEvents(filter));

            //return Data.GenerateMockData();
        }
    }
}
