using GeoEvents.Service.Common;

namespace GeoEvents.Service
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IEventService>().To<EventService>();
            Bind<IImageService>().To<ImageService>();
        }
    }

    #endregion Methods
}