using GeoEvents.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository.Common
{
    public interface IEventRepository
    {
        void EventRepo(IPostgresConnection postConn);
        bool CreateEvent(IEventEntity evt);
        List<IEventEntity> GetEvents(Filter filter);

    }
}
