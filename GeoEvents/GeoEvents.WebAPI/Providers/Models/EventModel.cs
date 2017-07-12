using System;
using System.Collections.Generic;

namespace GeoEvents.WebAPI
{
    public class EventModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public List<int> Categories { get; set; }

        public EventModel(Guid id, string name, string description, DateTime starttime, DateTime endtime, decimal uLat, decimal uLong, List<int> categories) {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.StartTime = starttime;
            this.EndTime = endtime;
            this.Lat = uLat;
            this.Long = uLong;
            this.Categories = categories;
        }

        public EventModel() { }
    }
}