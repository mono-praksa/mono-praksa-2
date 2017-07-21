using GeoEvents.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IEventRepository>().To<EventRepository>();
            Bind<IImageRepository>().To<ImageRepository>();
            Bind<ILocationRepository>().To<LocationRepository>();
        }
    }

    #endregion Methods
}