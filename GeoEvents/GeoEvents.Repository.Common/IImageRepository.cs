using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository.Common
{
    interface IImageRepository
    {

        List<IImageEntity> GetImages(Guid eventID);

        bool CreateImages(Guid eventId, List<IImageEntity> img);

    }
}
