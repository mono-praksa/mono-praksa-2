using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GeoEvents.Model.Mapping;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using AutoMapper;

namespace GeoEvents.WebAPI.App_Start
{
    public class AutoMapperConfig : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>().ToConstant(Initialize());
        }

        private IMapper Initialize()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<ModelProfile>();
                config.AddProfile<WebProfile>();
            });

            return Mapper.Instance;
        }
    }
}