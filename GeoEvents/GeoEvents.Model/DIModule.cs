using GeoEvents.Model.Common;

namespace GeoEvents.Model
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IEvent>().To<Event>();
            Bind<IImage>().To<Image>();
            Bind<ILocation>().To<Location>();
        }
    }

    #endregion Methods
}