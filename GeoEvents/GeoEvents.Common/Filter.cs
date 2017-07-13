using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Common
{
    /// <summary>
    /// The filter model.
    /// </summary>
    public class Filter : IFilter
    {
        #region Properties
        /// <summary>
        /// Gets or sets the latitude of the filter's location.
        /// </summary>
        /// <value>The latitude.</value>
        public decimal? ULat { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the filter's location.
        /// </summary>
        /// <value>The longitude.</value>
        public decimal? ULong { get; set; }

        /// <summary>
        /// Gets or sets the filter's radius.
        /// </summary>
        /// <value>The radius.</value>
        public decimal? Radius { get; set; }

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
        public int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the filter's desired number of events on one page.
        /// </summary>
        /// <value>The page size.</value>
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets the filter's search string. 
        /// </summary>
        /// <value>The search string.</value>
        public string SearchString { get; set; }

        /// <summary>
        /// Gets or sets the boolean value representing whether the filter should search by name only or
        /// by both name and description.
        /// </summary>
        /// <value>The boolean.</value>
        public bool? NameOnly { get; set; }

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

        #endregion Properties

        #region Constructors

        public Filter(decimal? uLat, decimal? uLong, decimal? radius, DateTime? startTime, DateTime? endTime, int? category, int? pageNumber, int? pageSize, string searchString, string orderBy, bool? orderAscending, bool? nameOnly)
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
            NameOnly = nameOnly;
        }

        #endregion Constructors

    }
}
