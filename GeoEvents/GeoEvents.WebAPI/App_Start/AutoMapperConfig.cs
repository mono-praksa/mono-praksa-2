using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;

namespace GeoEvents.WebAPI.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<IEventEntity, IEvent>().ReverseMap();
                config.CreateMap<IImageEntity, IImage>().ReverseMap();
            });
        }
    }
}