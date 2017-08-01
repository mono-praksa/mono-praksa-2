using GeoEvents.Model.Common;
using System;
using System.Threading.Tasks;

namespace GeoEvents.Service.Common
{
    public interface ILocationService
    {
        /// <summary>
        /// Gets location or creates if there is non  asynchronous.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>
        /// Location
        /// </returns>
        Task<ILocation> GetLocationAsync(string address);

        /// <summary>
        /// Create Location asynchronously
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
        Task<ILocation> UpdateLocationRatingAsync(Guid eventId, double rating, double currentRating, int rateCount);
    }
}