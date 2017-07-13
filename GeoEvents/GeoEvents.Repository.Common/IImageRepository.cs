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

        Task<IEnumerable<IImage>> GetImagesAsync(Guid eventId);
        Task<IImage> CreateImageAsync(IImage image);

    }
}
