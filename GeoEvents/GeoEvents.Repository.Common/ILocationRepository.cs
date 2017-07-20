using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Common;
using GeoEvents.Model.Common;

namespace GeoEvents.Repository.Common
{
    public interface ILocationRepository
    {

        /// <summary>
        /// Updates the rating location asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="rating">The rating.</param>
        /// <returns></returns>
        Task<ILocation> UpdateRatingLocationAsync(Guid eventId, decimal rating);

        /// <summary>
        /// Creates the location asynchronous.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        Task<ILocation> CreateLocationAsync(ILocation location);
    }
}
