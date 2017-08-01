using GeoEvents.Common;

namespace GeoEvents.DAL
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IPostgresConnection>().To<PostgresConnection>();
        }
    }

    #endregion Methods
}