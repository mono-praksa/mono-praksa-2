using GeoEvents.Service.Common;

namespace GeoEvents.Service
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IEventService>().To<EventService>();
            Bind<IImageService>().To<ImageService>();
            Bind<ILocationService>().To<LocationService>();
        }
    }

    #endregion Methods
}