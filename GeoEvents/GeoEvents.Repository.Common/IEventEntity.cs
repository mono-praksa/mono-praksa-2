using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository.Common
{
    public interface IEventEntity
    {
        Guid Id { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        Decimal Lat { get; set; }
        Decimal Long { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        int Category { get; set; }
    }
}
