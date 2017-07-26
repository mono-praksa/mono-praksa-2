using AutoMapper;
using GeoEvents.Common;
using GeoEvents.Model.Common;
using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeoEvents.WebAPI.Controllers
{
    [RoutePrefix("api/locations")]
    public class LocationController : ApiController
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        protected ILocationService Service { get; private set; }
        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        protected IMapper Mapper { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="mapper">The mapper.</param>
        public LocationController(ILocationService service, IMapper mapper)
        {
            this.Service = service;
            this.Mapper = mapper;
        }

        /// <summary>
        /// Gets the location asynchronous.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="HttpResponseException"></exception>
        [HttpGet]
        [Route("get")]
        public async Task<HttpResponseMessage> GetLocationAsync(string address = "", string id = "")
        {
            if(address != "" && id == "")
            {
                var result = Mapper.Map<LocationModel>(await Service.GetLocationAsync(address));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else if(address == "" && id != "")
            {
                var result = Mapper.Map<LocationModel>(await Service.GetLocationByIdAsync(new Guid(id)));
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "invalid adress and/or id");
            }
            
        }

        /// <summary>
        /// Updates the rating asynchronous.
        /// </summary>
        /// <param name="locationId">The location identifier.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="currentRating">The current rating.</param>
        /// <param name="rateCount">The rate count.</param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/rating")]
        public async Task<HttpResponseMessage> UpdateRatingAsync(Guid locationId, double rating, double currentRating, int rateCount)
        {
            var result =  await Service.UpdateLocationRatingAsync(locationId, rating, currentRating, rateCount);

            if(result.RateCount == rateCount +1)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }

    public class LocationModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }
        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        public double Rating { get; set; }
        /// <summary>
        /// Gets or sets the rate count.
        /// </summary>
        /// <value>
        /// The rate count.
        /// </value>
        public int RateCount { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationModel"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="address">The address.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="rateCount">The rate count.</param>
        public LocationModel(Guid id, string address, double rating, int rateCount)
        {
            this.Id = id;
            this.Address = address;
            this.Rating = rating;
            this.RateCount = rateCount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationModel"/> class.
        /// </summary>
        public LocationModel()
        {

        }
    }
}