using System;

namespace GeoEvents.DAL
{
    /// <summary>
    /// Database model EventEntity
    /// </summary>
    public class EventEntity
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
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public DateTime EndTime { get; set; }

        /// <summary>
        /// Gets or sets the lat.
        /// </summary>
        /// <value>
        /// The lat.
        /// </value>
        public Decimal Lat { get; set; }

        /// <summary>
        /// Gets or sets the long.
        /// </summary>
        /// <value>
        /// The long.
        /// </value>
        public Decimal Long { get; set; }

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
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public int Category { get; set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EventEntity"/> class.
        /// </summary>
        public EventEntity() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventEntity"/> class.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="StartTime">The start time.</param>
        /// <param name="EndTime">The end time.</param>
        /// <param name="Lat">The lat.</param>
        /// <param name="Long">The long.</param>
        /// <param name="Name">The name.</param>
        /// <param name="Description">The description.</param>
        /// <param name="Category">The category.</param>
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