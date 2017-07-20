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
        /// Getslocation or creates if there is non  asynchronous.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="EventRating"></param>
        /// <param name="EventRatingCount"></param>
        /// <returns>
        /// Location
        /// </returns>
        Task<ILocation> GetOrCreateLocationAsync(string address,double EventRating,int EventRateCount);

        /// <summary>
        /// Create Location asynchronous
        /// </summary>
        /// <param name="location"></param>
        /// <returns>
        /// Location
        /// </returns>
        Task<ILocation> CreateLocationAsync(ILocation location);

        /// <summary>
        /// Get Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Rating"></param>
        /// <returns>
        /// Location
        /// </returns>
        Task<ILocation> GetLocationByIdAsync(Guid id,double Rating);
    }
}
