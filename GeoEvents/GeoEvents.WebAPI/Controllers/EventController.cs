﻿using GeoEvents.Model.Common;
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
        [Route(@"search/{ULat:decimal}/{ULong:decimal}/{Radius:decimal}/{Category:int}/{StartTime:regex(^\d{4}-\d{2}-\d{2} \d{2}h\d{2}$)}/{EndTime:regex(^\d{4}-\d{2}-\d{2} \d{2}h\d{2}$)}")]
        public async Task<IEnumerable<EventModel>> GetEventsAsync(decimal? ULat, decimal? ULong, decimal? Radius, int? Category, string StartTime, string EndTime)
        {
            Filter filter = new Filter(ULat, ULong, Radius, DateTime.Parse(StartTime.Replace('h', ':')), DateTime.Parse(EndTime.Replace('h', ':')), Category, 1, 10, "", "Name", true);

            return Mapper.Map<List<EventModel>>(await Service.GetEventsAsync(filter));
        }

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

    public EventModel(Guid id, string name, string description, DateTime starttime, DateTime endtime, decimal uLat, decimal uLong, List<int> categories)
    {
        this.Id = id;
        this.Name = name;
        this.Description = description;
        this.StartTime = starttime;
        this.EndTime = endtime;
        this.Lat = uLat;
        this.Long = uLong;
        this.Categories = categories;
    }

    public EventModel() { }
}
