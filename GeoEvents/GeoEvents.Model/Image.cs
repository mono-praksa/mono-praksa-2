using GeoEvents.Model.Common;
using System;

namespace GeoEvents.Model
{
    /// <summary>
    /// The image.
    /// </summary>
    /// <seealso cref="IImage" />
    public class Image : IImage
    {
        #region Properties

        /// <summary>
        /// Gets or sets the image identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>
        /// The event identifier.
        /// </value>
        public Guid EventId { get; set; }

        /// <summary>
        /// Gets or sets the content of the image.
        /// </summary>
        /// <value>
        /// The content of the image.
        /// </value>
        public byte[] Content { get; set; }

        #endregion Properties
    }
}