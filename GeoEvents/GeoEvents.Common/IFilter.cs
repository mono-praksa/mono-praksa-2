using System;

namespace GeoEvents.Common
{
    public interface IFilter
    {
        /// <summary>
        /// Gets or sets the latitude of the filter's location.
        /// </summary>
        /// <value>The latitude.</value>
        decimal? ULat { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the filter's location.
        /// </summary>
        /// <value>The longitude.</value>
        decimal? ULong { get; set; }

        /// <summary>
        /// Gets or sets the filter's radius.
        /// </summary>
        /// <value>The radius.</value>
        decimal? Radius { get; set; }

        /// <summary>
        /// Gets or sets the start time of the filter's timespan.
        /// </summary>
        /// <value>The start time.</value>
        DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the filter's timespan.
        /// </summary>
        /// <value>The end time.</value>
        DateTime? EndTime { get; set; }

        /// <summary>
        /// Gets or sets the integer representing the filter's categories.
        /// </summary>
        /// <value>The integer.</value>
        int? Category { get; set; }

        /// <summary>
        /// Gets or sets the filter's desired page.
        /// </summary>
        /// <value>The page number.</value>
        int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the filter's desired number of events on one page.
        /// </summary>
        /// <value>The page size.</value>
        int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets the filter's search string.
        /// </summary>
        /// <value>The search string.</value>
        string SearchString { get; set; }

        /// <summary>
        /// Gets or sets the attribute by which the result should be sorted.
        /// </summary>
        /// <value>The attribute.</value>
        string OrderBy { get; set; }

        /// <summary>
        /// Gets or sets the boolean value representing whether the result should be sorted in ascending order or not.
        /// </summary>
        /// <value>The boolean.</value>
        bool? OrderAscending { get; set; }

        /// <summary>
        /// Gets or sets the price.
        /// </summary>
        /// <value>
        /// The price.
        /// </value>
        decimal? Price { get; set; }

        /// <summary>
        /// Gets or sets the rating event.
        /// </summary>
        /// <value>
        /// The rating event.
        /// </value>
        decimal? RatingEvent { get; set; }

        /// <summary>
        /// Gets or sets the custom.
        /// </summary>
        /// <value>
        /// The custom attributes.
        /// </value>
        string Custom { get; set; }
    }
}