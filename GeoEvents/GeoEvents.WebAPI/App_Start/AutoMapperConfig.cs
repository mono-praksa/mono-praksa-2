using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoEvents.WebAPI.App_Start
{
    public class AutoMapperConfig
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