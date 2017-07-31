using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Common
{
    public class ClusteringFilter: IClusteringFIlter
    {
        #region properties

        /// <summary>
        /// Gets or sets the latitude of the north-east conrner of the map viewport boundary.
        /// </summary>
        /// <value>The Latitude</value>
        public double NELatitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the north-east corner of the map viewport boundary.
        /// </summary>
        /// <value>The Longitude.</value>
        public double NELongitude { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the south-west corner of the map viewport boundary.
        /// </summary>
        /// <value>The Latitude.</value>
        public double SWLatitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the south-west corner of the map viewport boundary.
        /// </summary>
        /// <value>The Longitude.</value>
        public double SWLongitude { get; set; }

        /// <summary>
        /// gets or sets the zoom level of the map.
        /// </summary>
        /// <value>The Zoom level</value>
        public int ZoomLevel { get; set; }

        #endregion

        #region constructors

        public ClusteringFilter (double NELatitude, double NELongitude, double SWLatitude, double SWLongitude, int ZoomLevel)
        {
            this.NELatitude = NELatitude;
            this.NELongitude = NELongitude;
            this.SWLatitude = SWLatitude;
            this.SWLongitude = SWLongitude;
            this.ZoomLevel = ZoomLevel;
        }

        public ClusteringFilter ()
        {

        }

        #endregion constructors
    }
}
