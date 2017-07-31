using System;

namespace GeoEvents.DAL
{
    /// <summary>
    /// Database model EventEntity
    /// </summary>
    public class EventEntity
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

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
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude { get; set; }

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
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public int Category { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        public double Price { get; set; }

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
        public double Rating { get; set; }

        /// <summary>
        /// Gets or sets the rate count.
        /// </summary>
        /// <value>
        /// The rate count.
        /// </value>
        public int RateCount { get; set; }

        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        public Guid LocationId { get; set; }

        /// <summary>
        /// Gets or sets the custom.
        /// </summary>
        /// <value>
        /// The custom.
        /// </value>
        public string Custom { get; set; }

        /// <summary>
        /// Gets or sets the occurrence.
        /// </summary>
        /// <value>
        /// The occurrence.
        /// </value>
        public string Occurrence { get; set; }

        /// <summary>
        /// Gets or sets the repeat every.
        /// </summary>
        /// <value>
        /// The repeat every.
        /// </value>
        public int RepeatEvery { get; set; }

        /// <summary>
        /// Gets or sets the repeat on.
        /// </summary>
        /// <value>
        /// The repeat on.
        /// </value>
        public int RepeatOn { get; set; }

        /// <summary>
        /// Gets or sets the repeat count.
        /// </summary>
        /// <value>
        /// The repeat count.
        /// </value>
        public int RepeatCount { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventEntity"/> class.
        /// </summary>
        public EventEntity() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventEntity" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="startTime">The start time.</param>
        /// <param name="endTime">The end time.</param>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="category">The category.</param>
        /// <param name="price">The price.</param>
        /// <param name="capacity">The capacity.</param>
        /// <param name="reserved">The reserved.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="rateCount">The rate count.</param>
        /// <param name="locationId">The location identifier.</param>
        /// <param name="custom">The custom.</param>
        /// <param name="occurrence">The occurrence.</param>
        /// <param name="repeatevery">The repeatevery.</param>
        /// <param name="repeaton">The repeaton.</param>
        /// <param name="repeatcount">The repeatcount.</param>
        public EventEntity(Guid id, DateTime startTime, DateTime endTime, double latitude,
            double longitude, string name, string description, int category, double price, int capacity, int reserved, double rating, int rateCount, Guid locationId, string custom, string occurrence, int repeatevery, int repeaton, int repeatcount)
        {
            this.Id = id;
            this.StartTime = startTime;
            this.EndTime = endTime;
            this.Latitude = latitude;
            this.Longitude = longitude;
            this.Name = name;
            this.Description = description;
            this.Category = category;
            this.Price = price;
            this.Capacity = capacity;
            this.Reserved = reserved;
            this.Rating = rating;
            this.RateCount = rateCount;
            this.LocationId = locationId;
            this.Custom = custom;
            this.Occurrence = occurrence;
            this.RepeatEvery = repeatevery;
            this.RepeatOn = repeaton;
            this.RepeatCount = repeatcount;
        }

        #endregion Constructors
    }
}