using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Model.Common;

namespace GeoEvents.Service.Common
{
    /// <summary>
    /// The image service interface.
    /// </summary>
    public interface IImageService
    {
        #region Methods

        /// <summary>
        /// Gets images attached to an event.
        /// </summary>
        /// <param name="eventId">The event's id.</param>
        /// <returns></returns>
        Task<IEnumerable<IImage>> GetImagesAsync(Guid eventId);

        /// <summary>
        /// Adds image to an event.
        /// </summary>
        /// <param name="image">The list of images to add.</param>
        /// <returns></returns>
        Task<IImage> CreateImageAsync(IImage image);

            #endregion Methods
    }
}
