using GeoEvents.Repository.Common;

namespace GeoEvents.Repository
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IEventRepository>().To<EventRepository>();
            Bind<IImageRepository>().To<ImageRepository>();
            Bind<ILocationRepository>().To<LocationRepository>();
        }
    }

    #endregion Methods
}