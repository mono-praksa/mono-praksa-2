using AutoMapper;
using GeoEvents.Common;
using GeoEvents.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository
{
    public class LocationRepository : ILocationRepository
    {
        #region Properties

        protected IPostgresConnection PostgresConn { get; private set; }
        protected IMapper Mapper { get; private set; }

        #endregion Properties

        #region Constructors

        public LocationRepository(IPostgresConnection connection, IMapper mapper)
        {
            this.PostgresConn = connection;
            this.Mapper = mapper;
        }

        #endregion Constructors

        #region Methods

        public async Task<ILocation> UpdateRatingLocationAsync(Guid eventId, decimal rating)
        {
            return Mapper.Map<ILocation>(location);
        }

        public async Task<ILocation> CreateLocationAsync(ILocation location)
        {
            return Mapper.Map<ILocation>(location);
        }

        #endregion Methods
    }
}
