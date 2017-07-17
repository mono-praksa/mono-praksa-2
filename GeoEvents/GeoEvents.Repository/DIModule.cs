using GeoEvents.Repository.Common;

namespace GeoEvents.Repository
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IEventRepository>().To<EventRepository>();
            Bind<IImageRepository>().To<ImageRepository>();
        }
    }

    #endregion Methods
}