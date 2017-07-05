using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Common
{
    public class Filter
    {
        #region Properties
        public decimal ULat { get; set; }
        public decimal ULong { get; set; }
        public decimal Radius { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Category { get; set; }
        #endregion Properties

        public Filter(decimal uLat, decimal uLong, decimal radius, DateTime startTime, DateTime endTime, int category)
        {
            ULat = uLat;
            ULong = uLong;
            Radius = radius;
            StartTime = startTime;
            EndTime = endTime;
            Category = category;
        }
    }
}
