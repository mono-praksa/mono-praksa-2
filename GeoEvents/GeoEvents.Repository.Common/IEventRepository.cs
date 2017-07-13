using GeoEvents.Common;
using GeoEvents.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository.Common
{
    public interface IEventRepository
    {
        #region Methods
        /// <summary>
        /// Creates Event asynchronously.
        /// </summary>
        /// <param name="evt"></param>
        /// <returns>  
        /// return Created event (IEvent)
        /// </returns>
        Task<IEnumerable<IEvent>> GetEventsAsync (IFilter filter);

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

        #endregion Methods
    }
}
