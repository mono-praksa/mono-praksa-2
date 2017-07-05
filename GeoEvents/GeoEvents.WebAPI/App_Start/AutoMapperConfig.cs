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
            GeoEvents.Model.Mapping.AutoMapperMaps.Initialize();
            AutoMapper.Mapper.Initialize(config =>
            {
                config.CreateMap<IEvent, EventsViewModel>().ReverseMap();
                config.CreateMap<IImage, ImagesViewModel>().ReverseMap();
            });
        }
    }
}