using System;
using System.Collections.Generic;

namespace GeoEvents.WebAPI
{
    public class EventsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public List<int> Categories { get; set; }
    }
}