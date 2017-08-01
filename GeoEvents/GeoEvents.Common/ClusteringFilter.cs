using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Common
{
    public class ClusteringFilter: IClusteringFIlter
    {
        #region Properties

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

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteringFilter"/> class 
        /// </summary>
        /// <param name="nELatitude">Latitude of the northeast boundary point.</param>
        /// <param name="nELongitude">Longitude of the northeast boundary points.</param>
        /// <param name="sWLatitude">Latitude of the southwest boundary point.</param>
        /// <param name="sWLongitude">Longitude of the southwest boundary point.</param>
        /// <param name="zoomLevel">Level of zoom on the map.</param>
        public ClusteringFilter (double nELatitude, double nELongitude, double sWLatitude, double sWLongitude, int zoomLevel)
        {
            this.NELatitude = nELatitude;
            this.NELongitude = nELongitude;
            this.SWLatitude = sWLatitude;
            this.SWLongitude = sWLongitude;
            this.ZoomLevel = zoomLevel;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClusteringFilter"/> class
        /// </summary>
        public ClusteringFilter ()
        {

        }

        #endregion Constructors
    }
}
