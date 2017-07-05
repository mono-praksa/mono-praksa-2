using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;

namespace GeoEvents.Model.Mapping
{
    public static class AutoMapperMaps
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<IEventEntity, IEvent>().ReverseMap();
            });

        }
    }
}
