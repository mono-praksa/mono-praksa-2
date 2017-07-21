using GeoEvents.Common;
using GeoEvents.Model.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoEvents.Repository.Common
{
    public interface IEventRepository
    {
        #region Methods

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
        /// return Created event (IEvent)
        /// </returns>
        Task<IEnumerable<IEvent>> GetEventsAsync(IFilter filter);

        /// <summary>
        /// Gets filtered Events asynchronously.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// list of Events.
        /// </returns>
        Task<IEvent> CreateEventAsync(IEvent evt);

        /// <summary>
        /// Get event count asynchronously.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// number of events .
        /// </returns>
        Task<Int64> GetEventCountAsync(IFilter filter);

        /// <summary>
        /// Updates the reservation asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        Task<IEvent> UpdateReservationAsync(Guid eventId);

        /// <summary>
        /// Updates the rating asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="rating">The rating.</param>
        /// <returns></returns>
        Task<IEvent> UpdateRatingAsync(Guid eventId, double rating,double CurrentRating,int RateCount);

        #endregion Methods
    }
}

