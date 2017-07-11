using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Common
{
    public interface IFilter
    {
        decimal? ULat { get; set; }
        
        decimal? ULong { get; set; }

        decimal? Radius { get; set; }

        DateTime? StartTime { get; set; }

        DateTime? EndTime { get; set; }

        int? Category { get; set; }

        int? PageNumber { get; set; }

        int? PageSize { get; set; }

        string SearchString { get; set; }

        string OrderBy { get; set; }

        string OrderType { get; set; }

    }
}
