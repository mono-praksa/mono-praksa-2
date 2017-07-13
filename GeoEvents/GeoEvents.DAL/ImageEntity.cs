using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GeoEvents.DAL
{
    /// <summary>
    /// Database model ImageEntity
    /// </summary>
    public class ImageEntity 
    {
        #region Properties
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the content of the image.
        /// </summary>
        /// <value>
        /// The content of the image.
        /// </value>
        public byte[] Content { get; set; }

        /// <summary>
        /// Gets or sets the event identifier.
        /// </summary>
        /// <value>
        /// The event identifier.
        /// </value>
        public Guid EventId { get; set; }
        #endregion Properties

        #region Constructors
        public ImageEntity() { }
        public ImageEntity(Guid Id, byte[] Content, Guid EventId)
        {
            this.Id = Id;
            this.Content = Content;
            this.EventId = EventId;
        }
        #endregion Constructors

    }
}
