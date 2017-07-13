﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Service.Common;
using GeoEvents.Model.Common;
using GeoEvents.Common;
using GeoEvents.Repository.Common;

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
        /// <value>The repository.</value>
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
        public async Task<IEnumerable<IImage>> GetImagesAsync(Guid eventId)
        {
            return await Repository.GetImagesAsync(eventId);
        }

        /// <summary>
        /// Adds image to an event.
        /// </summary>
        /// <param name="image">The list of images to add.</param>
        /// <returns></returns>
        public async Task<IImage> CreateImageAsync(IImage image)
        {
            image.Id = Guid.NewGuid();
            return await Repository.CreateImageAsync(image);
        }

        #endregion Methods
    }
}
