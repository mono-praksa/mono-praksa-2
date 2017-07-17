using GeoEvents.Common;

namespace GeoEvents.DAL
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IPostgresConnection>().To<PostgresConnection>();
        }
    }

    #endregion Methods
}