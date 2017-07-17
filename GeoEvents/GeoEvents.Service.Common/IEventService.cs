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
        /// Gets the events asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<IEnumerable<IEvent>> GetEventsAsync(IFilter filter);


        /// <summary>
        /// Gets the event count asynchronous.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        Task<Int64> GetEventCountAsync(IFilter filter);


        /// <summary>
        /// Creates the event asynchronous.
        /// </summary>
        /// <param name="evt">The evt.</param>
        /// <returns></returns>
        Task<IEvent> CreateEventAsync(IEvent evt);

        #endregion Methods
    }
}
