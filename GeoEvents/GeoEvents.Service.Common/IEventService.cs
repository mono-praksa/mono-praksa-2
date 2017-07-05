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
        List<IEvent> GetEvents(Filter filter);

        /// <summary>
        /// Creates event.
        /// </summary>
        /// <returns></returns>
        bool CreateEvent(IEvent evt);

        #endregion Methods
    }
}
