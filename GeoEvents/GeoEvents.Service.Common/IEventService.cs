using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Model.Common;
using GeoEvents.Common;

namespace GeoEvents.Service.Common
{
    /// <summary>
    /// The event service interface.
    /// </summary>
    public interface IEventService
    {
        #region Methods
        /// <summary>
        /// Gets events that satisfy a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>List of events.</returns>
        Task<IEnumerable<IEvent>> GetEventsAsync(IFilter filter);

        /// <summary>
        /// Gets the number of events that satisfy a filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>The number.</returns>
        Task<int> GetEventCountAsync(IFilter filter);

        /// <summary>
        /// Creates an event.
        /// </summary>
        /// <param name="evt">The event.</param>
        /// <returns>The event that was created.</returns>
        Task<IEvent> CreateEventAsync(IEvent evt);

        #endregion Methods
    }
}
