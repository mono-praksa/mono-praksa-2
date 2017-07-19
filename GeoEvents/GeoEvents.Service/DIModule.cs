using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Service
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IEventService>().To<EventService>();
            Bind<IImageService>().To<ImageService>();
            Bind<ILocationService>().To<LocationService>();
        }
    }

    #endregion Methods
}