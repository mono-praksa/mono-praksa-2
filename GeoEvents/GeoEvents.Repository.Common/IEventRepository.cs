using GeoEvents.Common;
using GeoEvents.Model.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;

namespace GeoEvents.Repository.Common
{
    public interface IEventRepository
    {
        #region Methods

        /// <summary>
        /// Gets filtered Events asynchronously without paging and ordering.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The list of filtered events.</returns>
        Task<List<IEvent>> GetAllEventsAsync(IFilter filter);

        /// <summary>
        /// Gets an event by its id asynchronously.
        /// </summary>
        /// <param name="eventId">The event's id.</param>
        /// <returns>The event.</returns>
        Task<IEvent> GetEventByIdAsync(Guid eventId);

        /// <summary>
        /// Creates Event asynchronously.
        /// </summary>
        /// <param name="evt"></param>
        /// <returns>
        /// Created event (IEvent)
        /// </returns>
        Task<StaticPagedList<IEvent>> GetEventsAsync(IFilter filter);

        /// <summary>
        /// Gets filtered Events asynchronously.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// The list of filtered events.
        /// </returns>
        Task<IEvent> CreateEventAsync(IEvent evt);

        /// <summary>
        /// Updates the reservation asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns>
        /// Updated event
        /// </returns>
        Task<IEvent> UpdateReservationAsync(Guid eventId);

        /// <summary>
        /// Updates the rating asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="rating">The rating.</param>
        /// <returns>
        /// Updated event
        /// </returns>
        Task<IEvent> UpdateRatingAsync(Guid eventId, double rating, double CurrentRating, int RateCount);

        #endregion Methods
    }
}