using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        public Task<ILocation> GetLocationAsync(string address, double eventRating, int eventRateCount)
        {
            return Repository.GetLocationAsync(address, eventRating, eventRateCount);
        }

        public Task<ILocation> CreateLocationAsync(ILocation location)
        {
            return Repository.CreateLocationAsync(location);
        }

        public Task<ILocation> GetLocationByIdAsync(Guid id)
        {
            return Repository.GetLocationByIdAsync(id);
        }

        public Task<ILocation> UpdateLocationRatingAsync(Guid eventId, double rating)
        {
            return Repository.UpdateLocationRatingAsync(eventId, rating);
        }

        #endregion Methods
    }
}
