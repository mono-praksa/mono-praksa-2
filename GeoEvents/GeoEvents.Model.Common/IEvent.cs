using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Model.Common
{
    public interface IEvent
    {
        #region Properties

        Guid Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        decimal Lat { get; set; }
        decimal Long { get; set; }
        List<int> Categories { get; set; }
        DateTime StartTime { get; set; }
        DateTime EndTime { get; set; }
        int Category { get; set; }

        #endregion Properties
    }
}
