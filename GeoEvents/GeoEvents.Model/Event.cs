using GeoEvents.Model.Common;
using System;
using System.Collections.Generic;

namespace GeoEvents.Model
{
    /// <summary>
    /// Event model.
    /// </summary>
    /// <seealso cref="IEvent"/>
    public class Event : IEvent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier
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
        /// Gets or sets the categories of the event.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public int Category { get; set; }

        /// <summary>
        /// Gets or sets the start time of the event.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the event.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the integer representing the categories.
        /// The integer is the sum of all values in the Categories list.
        /// </summary>
        /// <value>
        /// The integer.
        /// </value>
        public List<int> Categories { get; set; }

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
        /// Gets or sets the json string representing custom attributes
        /// </summary>
        /// <value>
        /// The string.
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

        /// <summary>
        /// Gets or sets the repeat on list.
        /// </summary>
        /// <value>
        /// The repeat on list.
        /// </value>
        public List<int> RepeatOnList { get; set; }
        #endregion Properties
    }
}