using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using GeoEvents.Service.Common;
using System;
using System.Threading.Tasks;

namespace GeoEvents.Service
{
    public class LocationService : ILocationService
    {
        #region Properties

        /// <summary>
        /// Gets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        protected ILocationRepository Repository { get; private set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LocationService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public LocationService(ILocationRepository repository)
        {
            this.Repository = repository;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets location or creates if there is non  asynchronous.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>
        /// Location
        /// </returns>
        public Task<ILocation> GetLocationAsync(string address)
        {
            return Repository.GetLocationAsync(address);
        }

        /// <summary>
        /// Create Location asynchronously
        /// </summary>
        /// <param name="location"></param>
        /// <returns>
        /// Location
        /// </returns>
        public Task<ILocation> CreateLocationAsync(ILocation location)
        {
            return Repository.CreateLocationAsync(location);
        }

        /// <summary>
        /// Get Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Rating"></param>
        /// <returns>
        /// Location
        /// </returns>
        public Task<ILocation> GetLocationByIdAsync(Guid id)
        {
            return Repository.GetLocationByIdAsync(id);
        }

        /// <summary>
        /// Get Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Rating"></param>
        /// <returns>
        /// Location
        /// </returns>
        public Task<ILocation> UpdateLocationRatingAsync(Guid eventId, double rating, double currentRating, int rateCount)
        {
            return Repository.UpdateLocationRatingAsync(eventId, rating, currentRating, rateCount);
        }

        #endregion Methods
    }
}