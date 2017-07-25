using AutoMapper;
using GeoEvents.Common;
using GeoEvents.Model.Common;
using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeoEvents.WebAPI.Controllers
{
    [RoutePrefix("api/locations")]
    public class LocationController : ApiController
    {
        protected ILocationService Service { get; private set; }
        protected IMapper Mapper { get; private set; }

        public LocationController(ILocationService service, IMapper mapper)
        {
            this.Service = service;
            this.Mapper = mapper;
        }

        [HttpGet]
        [Route("get")]
        public async Task<LocationModel> GetLocationAsync(string address = "", string id = "")
        {
            if(address != "" && id == "")
            {
                return Mapper.Map<LocationModel>(await Service.GetLocationAsync(address));
            }
            else if(address == "" && id != "")
            {
                return Mapper.Map<LocationModel>(await Service.GetLocationByIdAsync(new Guid(id)));
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            
        }

        [HttpPut]
        [Route("update/rating")]
        public Task<ILocation> UpdateRatingAsync(Guid locationId, double rating, double currentRating, int rateCount)
        {
            return Service.UpdateLocationRatingAsync(locationId, rating, currentRating, rateCount);
        }
    }

    public class LocationModel
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public double Rating { get; set; }
        public int RateCount { get; set; }

        public LocationModel(Guid id, string address, double rating, int rateCount)
        {
            this.Id = id;
            this.Address = address;
            this.Rating = rating;
            this.RateCount = rateCount;
        }

        public LocationModel()
        {

        }
    }
}