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
        /// Gets the images asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<IImage>> GetImagesAsync(Guid eventId);


        /// <summary>
        /// Creates the image asynchronous.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        Task<IImage> CreateImageAsync(IImage image);

            #endregion Methods
    }
}
