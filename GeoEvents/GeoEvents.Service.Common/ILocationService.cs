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
        Task<ILocation> UpdateRatingLocationAsync(Guid eventId, decimal rating);

        Task<ILocation> CreateLocationAsync(ILocation location);

    }
}
