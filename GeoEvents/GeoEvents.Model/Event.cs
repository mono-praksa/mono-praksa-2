using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Model.Common;

namespace GeoEvents.Model
{
    /// <summary>
    /// Event model.
    /// </summary>
    /// <seealso cref="IEvent"/>
    public class Event : IEvent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier
        /// </value>
        public Guid Id { get; set; }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }


        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }


        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public decimal Lat { get; set; }


        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public decimal Long { get; set; }


        /// <summary>
        /// Gets or sets the categories of the event.
        /// </summary>
        /// <value>
        /// The categories.
        /// </value>
        public int Category { get; set; }


        /// <summary>
        /// Gets or sets the start time of the event.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartTime { get; set; }


        /// <summary>
        /// Gets or sets the end time of the event.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime EndTime { get; set; }


        /// <summary>
        /// Gets or sets the integer representing the categories.
        /// The integer is the sum of all values in the Categories list.
        /// </summary>
        /// <value>
        /// The integer.
        /// </value>
        public List<int> Categories { get; set; }

        #endregion Properties
    }
}
