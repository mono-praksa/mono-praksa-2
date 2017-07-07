﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Common
{
    /// <summary>
    /// The filter model.
    /// </summary>
    public class Filter
    {
        #region Properties
        /// <summary>
        /// Gets or sets the latitude of the filter's location.
        /// </summary>
        /// <value>The latitude.</value>
        public decimal ULat { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the filter's location.
        /// </summary>
        /// <value>The longitude.</value>
        public decimal ULong { get; set; }

        /// <summary>
        /// Gets or sets the filter's radius.
        /// </summary>
        /// <value>The radius.</value>
        public decimal Radius { get; set; }

        /// <summary>
        /// Gets or sets the start time of the filter's timespan.
        /// </summary>
        /// <value>The start time.</value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the filter's timespan.
        /// </summary>
        /// <value>The end time.</value>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the integer representing the filter's categories.
        /// </summary>
        /// <value>The integer.</value>
        public int Category { get; set; }

        #endregion Properties

        public Filter(decimal uLat, decimal uLong, decimal radius, DateTime startTime, DateTime endTime, int category)
        {
            ULat = uLat;
            ULong = uLong;
            Radius = radius;
            StartTime = startTime;
            EndTime = endTime;
            Category = category;
        }
    }
}
