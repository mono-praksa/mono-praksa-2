
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.DAL
{
    /// <summary>
    /// Database model EventEntity
    /// </summary>
    public class EventEntity 
    {
        #region Properties
        /// <summary>
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the start time of the event.
        /// </summary>
        /// <value>The start time.</value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the event.
        /// </summary>
        /// <value>The end time.</value>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        public Decimal Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        public Decimal Long { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the integer representing the categories.
        /// </summary>
        /// <value>The integer.</value>
        public int Category { get; set; }
        #endregion Properties

        #region Constructors
        public EventEntity() { }
        public EventEntity(Guid Id, DateTime StartTime, DateTime EndTime, Decimal Lat,
            Decimal Long, string Name, string Description, int Category)
        {
            this.Id = Id;
            this.StartTime = StartTime;
            this.EndTime = EndTime;
            this.Lat = Lat;
            this.Long = Long;
            this.Name = Name;
            this.Description = Description;
            this.Category = Category;
        }
        #endregion Constructors

    }
}

