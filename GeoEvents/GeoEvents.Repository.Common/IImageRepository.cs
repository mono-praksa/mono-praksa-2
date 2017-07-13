using GeoEvents.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository.Common
{
    public interface IImageRepository
    {
        #region Methods
        /// <summary>
        /// Creates Image asynchronously.
        /// </summary>
        /// <param name="img"></param>
        /// <returns>  
        /// return Created image (IEvent)
        /// </returns>
        Task<IImage> CreateImageAsync(IImage image);

        /// <summary>
        /// Gets Images asynchronously.
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns>
        /// list of Images.
        /// </returns>
        Task<IEnumerable<IImage>> GetImagesAsync(Guid eventId);
        #endregion Methods
    }
}
