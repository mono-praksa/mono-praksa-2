using GeoEvents.Common;

namespace GeoEvents.WebAPI.DbConnection
{
    public class DIModule : Ninject.Modules.NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IGeoEventsConfiguration>().To<GeoEventsConfiguration>().InSingletonScope();
        }
    }
}