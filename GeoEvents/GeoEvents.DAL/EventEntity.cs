
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        /// Gets or sets the lat.
        /// </summary>
        /// <value>
        /// The lat.
        /// </value>
        public Decimal Latitude { get; set; }


        /// <summary>
        /// Gets or sets the long.
        /// </summary>
        /// <value>
        /// The long.
        /// </value>
        public Decimal Longitude { get; set; }


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
        /// The rating location.
        /// </value>
        public decimal RatingLocation { get; set; }

        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        public Guid LocationId { get; set; }
        #endregion Properties


        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="EventEntity"/> class.
        /// </summary>
        public EventEntity() { }




        /// <summary>
        /// Initializes a new instance of the <see cref="EventEntity"/> class.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="StartTime">The start time.</param>
        /// <param name="EndTime">The end time.</param>
        /// <param name="Lat">The lat.</param>
        /// <param name="Long">The long.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Category">The category.</param>
        /// <param name="Price">The price.</param>
        /// <param name="Capacity">The capacity.</param>
        /// <param name="Reserved">The reserved.</param>
        /// <param name="Rating">The rating.</param>
        /// <param name="RateCount">The rate count.</param>
        /// <param name="RatingLocation">The rating location.</param>
        /// <param name="LocationId">The location identifier.</param>
        public EventEntity(Guid id, DateTime startTime, DateTime endTime, Decimal latitude,
            Decimal longitude, string name, string description, int category, decimal price, int capacity, int reserved, decimal rating, int rateCount, decimal ratingLocation, Guid locationId)
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
            this.RatingLocation = ratingLocation;
            this.LocationId = locationId;
        }
        #endregion Constructors

    }
}

