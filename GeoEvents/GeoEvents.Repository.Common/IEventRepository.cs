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

        Task<IEnumerable<IEvent>> GetEventsAsync (IFilter filter);
        Task<IEvent> CreateEventAsync(IEvent evt);


    }
}
