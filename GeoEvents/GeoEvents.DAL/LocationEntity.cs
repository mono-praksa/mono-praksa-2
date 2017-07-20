using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GeoEvents.DAL
{
    public class LocationEntity
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
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        public string Address { get; set; }
        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationEntity"/> class.
        /// </summary>
        public LocationEntity() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationEntity"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="rateCount">The rate count.</param>
        /// <param name="address">The address.</param>
        public LocationEntity(Guid id, double rating, int rateCount, string address)
        {
            this.Id = id;
            this.Rating = rating;
            this.RateCount = rateCount;
            this.Address = address;

        }
        #endregion Constructors

    }
}
