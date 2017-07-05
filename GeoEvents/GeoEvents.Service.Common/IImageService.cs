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
            List<IImage> GetImages(Guid eventId);

            /// <summary>
            /// Adds images to an event.
            /// </summary>
            /// <param name="eventId">The event's id.</param>
            /// <param name="images">The list of images to add.</param>
            /// <returns></returns>
            bool CreateImages(Guid eventId, List<IImage> images);

            #endregion Methods
    }
}
