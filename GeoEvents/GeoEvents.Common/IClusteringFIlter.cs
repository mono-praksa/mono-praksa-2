using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Common
{
    public interface IClusteringFIlter
    {
        #region Properties

        /// <summary>
        /// Gets or sets the latitude of the north-east conrner of the map viewport boundary.
        /// </summary>
        /// <value>The Latitude</value>
        double NELatitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the north-east corner of the map viewport boundary.
        /// </summary>
        /// <value>The Longitude.</value>
        double NELongitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the south-west corner of the map viewport boundary.
        /// </summary>
        /// <value>The Latitude.</value>
        double SWLatitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the south-west corner of the map viewport boundary.
        /// </summary>
        /// <value>The Longitude.</value>
        double SWLongitude { get; set; }

        /// <summary>
        /// gets or sets the zoom level of the map.
        /// </summary>
        /// <value>The Zoom level</value>
        int ZoomLevel { get; set; }

        #endregion Properties
    }
}
