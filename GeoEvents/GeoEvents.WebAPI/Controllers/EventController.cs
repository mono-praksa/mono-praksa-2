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
using X.PagedList;

namespace GeoEvents.WebAPI.Controllers
{
    [RoutePrefix("api/events")]
    public class EventController : ApiController
    {
        #region Parameters

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        protected IEventService Service { get; private set; }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        protected IMapper Mapper { get; private set; }

        #endregion Parameters

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="EventController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="mapper">The mapper.</param>
        public EventController(IEventService service, IMapper mapper)
        {
            this.Service = service;
            this.Mapper = mapper;
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Creates the event asynchronously.
        /// </summary>
        /// <param name="evt">The event to be created.</param>
        /// <returns>
        /// Returns the created event.
        /// </returns>
        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateEvent([FromBody] EventModel evt)
        {
            evt.Id = new Guid();

            var result = await Service.CreateEventAsync(Mapper.Map<IEvent>(evt));

            if (result.Id == new Guid())
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "failed to create the event");
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.Created, result);
            }
        }

        /// <summary>
        /// Gets the events asynchronously.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// Returns StaticPagedList of events.
        /// </returns>
        [HttpGet]
        [Route("search")]
        public async Task<HttpResponseMessage> GetEventsAsync([FromUri] FilterModel filter)
        {
            var result = await Service.GetEventsAsync(filter);

            var temp = Mapper.Map<IEnumerable<IEvent>, IEnumerable<EventModel>>(result);

            var responseData = new StaticPagedList<EventModel>(temp, result.GetMetaData());

            var response = new { data = responseData, metaData = responseData.GetMetaData() };

            return Request.CreateResponse(HttpStatusCode.OK, response);
        }

        /// <summary>
        /// Gets the event by identifier asynchronously.
        /// </summary>
        /// <param name="eventId">Identifier of the event to be retrieved.</param>
        /// <returns>
        /// Returns the event.
        /// </returns>
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "invalid guid");
            }

            var result = Mapper.Map<EventModel>(await Service.GetEventByIdAsync(eventIdGuid));

            if (result.Id == eventIdGuid)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "could not find an event with the requested id");
            }
        }

        /// <summary>
        /// Updates the event rating asynchronously.
        /// </summary>
        /// <param name="eventId">Identifier of the event which will have it's rating updated.</param>
        /// <param name="rating">Rating of the event that the user submitted(1-5)</param>
        /// <param name="CurrentRating">Current event rating</param>
        /// <param name="RateCount">Current rate count which will be increased by 1 each time function is called.</param>
        /// <returns>
        /// Returns the updated event
        /// </returns>
        [HttpPut]
        [Route("update/rating")]
        public async Task<HttpResponseMessage> UpdateRatingAsync(Guid eventId, double rating, double CurrentRating, int RateCount)
        {
            var result = await Service.UpdateRatingAsync(eventId, rating, CurrentRating, RateCount);

            if (result.RateCount == RateCount + 1)
            {
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "failed to update rating");
            }
        }

        /// <summary>
        /// Updates the reservation asynchronously.
        /// Each time this function is called number of reserved spaces increases by 1.
        /// </summary>
        /// <param name="eventId">Identifier of the event which will have it's reservation updated</param>
        /// <returns>
        /// Returns the updated event.
        /// </returns>
        [HttpPut]
        [Route("update/reservation")]
        public async Task<HttpResponseMessage> UpdateReservationAsync(Guid eventId)
        {
            var result = await Service.UpdateReservationAsync(eventId);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Gets the clustered events used for map view.
        /// </summary>
        /// <param name="filter">Filter used for retrieving the events from the database.</param>
        /// <param name="clusteringFilter">Filter used for clustering</param>
        /// <returns>Clustered events</returns>
        [HttpGet]
        [Route("clustered")]
        public async Task<HttpResponseMessage> GetEventsClusteredAsync([FromUri] Filter filter, [FromUri] ClusteringFilter clusteringFilter)
        {
            /*
            if(filter.ULat == null || filter.ULong == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "incorrect filter coordinates. filter coordinates are required");
            }
            if(filter.Radius == null || filter.Radius == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "incorrect filter radius. radius is required. radius must be larger that zero");
            }
            */
            var result = await Service.GetClusteredEventsAsync(filter, clusteringFilter);

            if(result == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "sorry :(");
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        #endregion Methods

        #region Model

        public class EventModel
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public Guid Id { get; set; }

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>
            /// The name.
            /// </value>
            public string Name { get; set; }

            /// <summary>
            /// Gets or sets the description.
            /// </summary>
            /// <value>
            /// The description.
            /// </value>
            public string Description { get; set; }

            /// <summary>
            /// Gets or sets the start time.
            /// </summary>
            /// <value>
            /// The start time.
            /// </value>
            public DateTime StartTime { get; set; }

            /// <summary>
            /// Gets or sets the end time.
            /// </summary>
            /// <value>
            /// The end time.
            /// </value>
            public DateTime EndTime { get; set; }

            /// <summary>
            /// Gets or sets the latitude.
            /// </summary>
            /// <value>
            /// The latitude.
            /// </value>
            public decimal Latitude { get; set; }

            /// <summary>
            /// Gets or sets the longitude.
            /// </summary>
            /// <value>
            /// The longitude.
            /// </value>
            public decimal Longitude { get; set; }

            /// <summary>
            /// Gets or sets the categories.
            /// </summary>
            /// <value>
            /// The categories list.
            /// </value>
            public List<int> Categories { get; set; }

            /// <summary>
            /// Gets or sets the price.
            /// </summary>
            /// <value>
            /// The price.
            /// </value>
            public decimal Price { get; set; }

            /// <summary>
            /// Gets or sets the capacity.
            /// </summary>
            /// <value>
            /// The capacity.
            /// </value>
            public int Capacity { get; set; }

            /// <summary>
            /// Gets or sets the reserved.
            /// </summary>
            /// <value>
            /// The reserved.
            /// </value>
            public int Reserved { get; set; }

            /// <summary>
            /// Gets or sets the rating.
            /// </summary>
            /// <value>
            /// The rating.
            /// </value>
            public decimal Rating { get; set; }

            /// <summary>
            /// Gets or sets the rate count.
            /// </summary>
            /// <value>
            /// The rate count.
            /// </value>
            public int RateCount { get; set; }

            /// <summary>
            /// Gets or sets the rating location.
            /// </summary>
            /// <value>
            /// The location rating.
            /// </value>
            public decimal RatingLocation { get; set; }

            /// <summary>
            /// Gets or sets the custom.
            /// </summary>
            /// <value>
            /// The custom atribute.
            /// </value>
            public string Custom { get; set; }

            /// <summary>
            /// Gets or sets the location identifier.
            /// </summary>
            /// <value>
            /// The location identifier.
            /// </value>
            public Guid LocationId { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="EventModel"/> class.
            /// </summary>
            /// <param name="id">The identifier.</param>
            /// <param name="name">The name.</param>
            /// <param name="description">The description.</param>
            /// <param name="starttime">The starttime.</param>
            /// <param name="endtime">The endtime.</param>
            /// <param name="uLat">The u lat.</param>
            /// <param name="uLong">The u long.</param>
            /// <param name="categories">The categories.</param>
            /// <param name="price">The price.</param>
            /// <param name="capacity">The capacity.</param>
            /// <param name="reserved">The reserved.</param>
            /// <param name="rating">The rating.</param>
            /// <param name="rateCount">The rate count.</param>
            /// <param name="ratingLocation">The rating location.</param>
            /// <param name="custom">The custom.</param>
            /// <param name="locationId">The location identifier.</param>
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

            /// <summary>
            /// Initializes a new instance of the <see cref="EventModel"/> class.
            /// </summary>
            public EventModel() { }
        }

        #endregion Model

        #region Filter Model
        public class FilterModel : IFilter
        {
            /// <summary>
            /// Gets or sets the latitude of the filter's location.
            /// </summary>
            /// <value>The latitude.</value>
            public double? ULat { get; set; }

            /// <summary>
            /// Gets or sets the longitude of the filter's location.
            /// </summary>
            /// <value>The longitude.</value>
            public double? ULong { get; set; }

            /// <summary>
            /// Gets or sets the filter's radius.
            /// </summary>
            /// <value>The radius.</value>
            public double? Radius { get; set; }

            /// <summary>
            /// Gets or sets the start time of the filter's timespan.
            /// </summary>
            /// <value>The start time.</value>
            public DateTime? StartTime { get; set; }

            /// <summary>
            /// Gets or sets the end time of the filter's timespan.
            /// </summary>
            /// <value>The end time.</value>
            public DateTime? EndTime { get; set; }

            /// <summary>
            /// Gets or sets the integer representing the filter's categories.
            /// </summary>
            /// <value>The integer.</value>
            public int? Category { get; set; }

            /// <summary>
            /// Gets or sets the filter's desired page.
            /// </summary>
            /// <value>The page number.</value>
            public int PageNumber { get; set; }

            /// <summary>
            /// Gets or sets the filter's desired number of events on one page.
            /// </summary>
            /// <value>The page size.</value>
            public int PageSize { get; set; }

            /// <summary>
            /// Gets or sets the filter's search string.
            /// </summary>
            /// <value>The search string.</value>
            public string SearchString { get; set; }

            /// <summary>
            /// Gets or sets the attribute by which the result should be sorted.
            /// </summary>
            /// <value>The attribute.</value>
            public string OrderBy { get; set; }

            /// <summary>
            /// Gets or sets the boolean value representing whether the result should be sorted in ascending order or not.
            /// </summary>
            /// <value>The boolean.</value>
            public bool? OrderAscending { get; set; }

            /// <summary>
            /// Gets or sets the price.
            /// </summary>
            /// <value>
            /// The price.
            /// </value>
            public double? Price { get; set; }

            /// <summary>
            /// Gets or sets the rating event.
            /// </summary>
            /// <value>
            /// The rating event.
            /// </value>
            public double? RatingEvent { get; set; }

            /// <summary>
            /// Gets or sets the custom.
            /// </summary>
            /// <value>
            /// The custom.
            /// </value>
            public string Custom { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="Filter"/> class.
            /// </summary>
            /// <param name="uLat">The u lat.</param>
            /// <param name="uLong">The u long.</param>
            /// <param name="radius">The radius.</param>
            /// <param name="startTime">The start time.</param>
            /// <param name="endTime">The end time.</param>
            /// <param name="category">The category.</param>
            /// <param name="pageNumber">The page number.</param>
            /// <param name="pageSize">Size of the page.</param>
            /// <param name="searchString">The search string.</param>
            /// <param name="orderBy">The order by.</param>
            /// <param name="orderAscending">The order ascending.</param>
            /// <param name="price">The price.</param>
            /// <param name="ratingEvent">The rating event.</param>
            /// <param name="custom">The custom.</param>
            public FilterModel(double? uLat = null, double? uLong = null, double? radius = null, DateTime? startTime = null, DateTime? endTime = null, int? category = 0, int pageNumber = 1, int pageSize = 25, string searchString = "", string orderBy = "", bool? orderAscending = false, double? price = null, double? ratingEvent = null, string custom = "")
            {
                ULat = uLat;
                ULong = uLong;
                Radius = radius;
                StartTime = startTime;
                EndTime = endTime;
                Category = category;
                PageNumber = pageNumber;
                PageSize = pageSize;
                SearchString = searchString;
                OrderBy = orderBy;
                OrderAscending = orderAscending;
                Price = price;
                RatingEvent = ratingEvent;
                Custom = custom;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="FilterModel"/> class.
            /// </summary>
            public FilterModel()
            {

            }
        }
        #endregion Filter Model

    }
}