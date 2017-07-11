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
        /// Gets events that satisfy the filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<IEnumerable<IEvent>> GetEventsAsync(IFilter filter);

        /// <summary>
        /// Creates event.
        /// </summary>
        /// <returns></returns>
        Task<IEvent> CreateEventAsync(IEvent evt);

        #endregion Methods
    }
}
