using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoEvents.Model.Mapping;
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
                config.AddProfile<ModelProfile>();
                config.AddProfile<WebProfile>();
            });
        }
    }
}