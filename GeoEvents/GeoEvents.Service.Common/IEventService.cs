using GeoEvents.Common;
using GeoEvents.Model.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace GeoEvents.Service.Common
{
    /// <summary>
    /// The event service interface.
    /// </summary>
    public interface IEventService
    {
        #region Methods

        /// <summary>
        /// Gets an event asynchronously using its id.
        /// </summary>
        /// <param name="id">The id of the event.</param>
        /// <returns>The event.</returns>
        Task<IEvent> GetEventByIdAsync(Guid id);

        /// <summary>
        /// Gets events that satisfy a filter asynchronously.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// List of events.
        /// </returns>
        Task<StaticPagedList<IEvent>> GetEventsAsync(IFilter filter);


        /// <summary>
        /// Creates an event asynchronously.
        /// </summary>
        /// <param name="evt">The evt.</param>
        /// <returns></returns>
        Task<IEvent> CreateEventAsync(IEvent evt);

        /// <summary>
        /// Updates the reservation asynchronously.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        Task<IEvent> UpdateReservationAsync(Guid eventId);

        /// <summary>
        /// Updates the rating asynchronously.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="Rating">The rating.</param>
        /// <returns></returns>
        Task<IEvent> UpdateRatingAsync(Guid eventId, double rating, double CurrentRating, int RateCount);

        #endregion Methods
    }
}