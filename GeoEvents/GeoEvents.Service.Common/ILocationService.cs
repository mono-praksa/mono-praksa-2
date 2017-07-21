using GeoEvents.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Service;

namespace GeoEvents.Service.Common
{
    public interface ILocationService
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
        Task<ILocation> GetLocationAsync(string address, double eventRating, int eventRateCount);

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
        Task<ILocation> GetLocationByIdAsync(Guid id);

        /// <summary>
        /// Get Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Rating"></param>
        /// <returns>
        /// Location
        /// </returns>
        Task<ILocation> UpdateLocationRatingAsync(Guid id, double rating);
    }
}
