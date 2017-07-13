using GeoEvents.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoEvents.WebAPI.DbConnection
{
    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IGeoEventsConfiguration>().To<GeoEventsConfiguration>().InSingletonScope();

        }
    }
}