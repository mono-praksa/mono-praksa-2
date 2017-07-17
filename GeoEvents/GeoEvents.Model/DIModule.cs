using GeoEvents.Model.Common;

namespace GeoEvents.Model
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IEvent>().To<Event>();
            Bind<IImage>().To<Image>();
        }
    }

    #endregion Methods
}