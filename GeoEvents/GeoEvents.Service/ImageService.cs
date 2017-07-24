using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeoEvents.Service
{
    /// <summary>
    /// The image service.
    /// </summary>
    /// <seealso cref="IImageService"/>
    public class ImageService : IImageService
    {
        #region Properties

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        protected IImageRepository Repository { get; private set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public ImageService(IImageRepository repository)
        {
            this.Repository = repository;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets images attached to an event.
        /// </summary>
        /// <param name="eventId">The event's id.</param>
        /// <returns></returns>
        public Task<IEnumerable<IImage>> GetImagesAsync(Guid eventId)
        {
            return Repository.GetImagesAsync(eventId);
        }

        /// <summary>
        /// Adds an image to an event.
        /// </summary>
        /// <param name="image">The list of images to add.</param>
        /// <returns></returns>
        public Task<IImage> CreateImageAsync(IImage image)
        {
            image.Id = Guid.NewGuid();
            return Repository.CreateImageAsync(image);
        }

        #endregion Methods
    }
}