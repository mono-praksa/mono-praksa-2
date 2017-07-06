using GeoEvents.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.DAL
{
    #region Methods

    public class DIModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IEventEntity>().To<EventEntity>();
            Bind<IImageEntity>().To<ImageEntity>();
            Bind<IPostgresConnection>().To<PostgresConnection>();
        }
    }

    #endregion Methods
}