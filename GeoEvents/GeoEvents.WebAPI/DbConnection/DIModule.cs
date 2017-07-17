using GeoEvents.Common;

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