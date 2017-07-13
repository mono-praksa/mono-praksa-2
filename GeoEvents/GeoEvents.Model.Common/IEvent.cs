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

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier</value>
        Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; set; }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>The latitude.</value>
        decimal Lat { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>The longitude.</value>
        decimal Long { get; set; }

        /// <summary>
        /// Gets or sets the integer representing the categories.
        /// The integer is the sum of all values in the Categories list.
        /// </summary>
        /// <value>The integer.</value>
        List<int> Categories { get; set; }

        /// <summary>
        /// Gets or sets the start time of the event.
        /// </summary>
        /// <value>The start time.</value>
        DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time of the event.
        /// </summary>
        /// <value>The end time.</value>
        DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the categories of the event.
        /// </summary>
        /// <value>The categories.</value>
        int Category { get; set; }

        #endregion Properties
    }
}
