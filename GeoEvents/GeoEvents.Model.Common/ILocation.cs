using System;

namespace GeoEvents.Model.Common
{
    public interface ILocation
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        /// <value>
        /// The rating.
        /// </value>
        double Rating { get; set; }

        /// <summary>
        /// Gets or sets the rate count.
        /// </summary>
        /// <value>
        /// The rate count.
        /// </value>
        int RateCount { get; set; }

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        string Address { get; set; }

        #endregion Properties
    }
}