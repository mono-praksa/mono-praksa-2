using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository.Common
{
    public interface IImageRepository
    {

        List<IImageEntity> GetImages(Guid eventID);

        bool CreateImages(List<IImageEntity> img);

    }
}
