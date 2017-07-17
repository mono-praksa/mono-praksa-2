using System;

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
        /// Gets or sets the content.
        /// </summary>
        /// <value>
        /// The content.
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEntity"/> class.
        /// </summary>
        public ImageEntity() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageEntity"/> class.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Content">The content.</param>
        /// <param name="EventId">The event identifier.</param>
        public ImageEntity(Guid Id, byte[] Content, Guid EventId)
        {
            this.Id = Id;
            this.Content = Content;
            this.EventId = EventId;
        }

        #endregion Constructors
    }
}