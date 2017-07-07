﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Service.Common;
using GeoEvents.Model.Common;
using GeoEvents.Common;
using GeoEvents.Repository.Common;
using AutoMapper;

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
        public List<IImage> GetImages(Guid eventId)
        {
            return Mapper.Map<List<IImage>>(Repository.GetImages(eventId));
        }

        /// <summary>
        /// Adds images to an event.
        /// </summary>
        /// <param name="eventId">The event's id.</param>
        /// <param name="images">The list of images to add.</param>
        /// <returns></returns>
        public bool CreateImages(List<IImage> images)
        {
            return Repository.CreateImages( Mapper.Map<List<IImageEntity>>(images));
        }

        #endregion Methods
    }
}
