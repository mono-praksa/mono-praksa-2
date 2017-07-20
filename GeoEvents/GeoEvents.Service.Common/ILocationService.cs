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
        Task<ILocation> GetLocationAsync(string address, double eventRating, int eventRateCount);

        Task<ILocation> CreateLocationAsync(ILocation location);

        Task<ILocation> GetLocationByIdAsync(Guid id);

        Task<ILocation> UpdateLocationRatingAsync(Guid eventId, double rating);

        }
}
