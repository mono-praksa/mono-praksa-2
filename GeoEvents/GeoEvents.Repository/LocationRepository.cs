using AutoMapper;
using GeoEvents.Common;
using GeoEvents.DAL;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

namespace GeoEvents.Repository
{
    public class LocationRepository : ILocationRepository
    {
        #region Properties

        protected IPostgresConnection Connection { get; private set; }
        protected IMapper Mapper { get; private set; }

        #endregion Properties

        #region Constructors

        public LocationRepository(IPostgresConnection connection, IMapper mapper)
        {
            this.Connection = connection;
            this.Mapper = mapper;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Getslocation or creates if there is non  asynchronous.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>
        /// ILocation
        /// </returns>
        public async Task<ILocation> GetLocationAsync(string address)
        {
            LocationEntity location = null;
            LocationEntity NewLocation = new LocationEntity(Guid.NewGuid(), 0, 0, address);

            using (Connection.CreateConnection())
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectLocationQueryString(), Connection.CreateConnection()))

            {
                commandSelect.Parameters.AddWithValue(QueryHelper.ParAddress, NpgsqlTypes.NpgsqlDbType.Text, address);
                if (Connection.CreateConnection().FullState == ConnectionState.Closed)
                {
                    await Connection.CreateConnection().OpenAsync();
                }

                DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                if (dr.Read())
                {
                    location = new LocationEntity()
                    {
                        Id = new Guid(dr[0].ToString()),
                        Address = dr[1].ToString(),
                        Rating = Convert.ToDouble(dr[2]),
                        RateCount = Convert.ToInt32(dr[3])
                    };
                }
                Connection.CreateConnection().Close();

                if (location == null)
                {
                   return await CreateLocationAsync(Mapper.Map<ILocation>(NewLocation));   
                }

                return await GetLocationByIdAsync(location.Id);


            }
         
        }

        /// <summary>
        /// Get Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// ILocation
        /// </returns>
        public async Task<ILocation> GetLocationByIdAsync(Guid id)
        {
            LocationEntity location = new LocationEntity();

            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetSelectLocationByIdQueryString(), Connection.CreateConnection()))
            {
                command.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlTypes.NpgsqlDbType.Uuid, id);

                if (Connection.CreateConnection().FullState == ConnectionState.Closed)
                {
                    await Connection.CreateConnection().OpenAsync();
                }
                DbDataReader dr = await command.ExecuteReaderAsync();
                if (dr.Read())
                {
                    location = new LocationEntity()
                    {
                        Id = new Guid(dr[0].ToString()),
                        Address = dr[1].ToString(),
                        Rating = Convert.ToDouble(dr[2]),
                        RateCount = Convert.ToInt32(dr[3])
                    };
                }
            }

            return Mapper.Map<ILocation>(location);
        }

        /// <summary>
        /// Create Location asynchronous
        /// </summary>
        /// <param name="location"></param>
        /// <returns>
        /// ILocation
        /// </returns>
        public async Task<ILocation> CreateLocationAsync(ILocation location)
        {

            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetInsertCreateLocationQueryString(), Connection.CreateConnection()))
            {
                if (Connection.CreateConnection().FullState == ConnectionState.Closed)
                {
                    await Connection.CreateConnection().OpenAsync();
                }
                command.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlTypes.NpgsqlDbType.Uuid, location.Id);
                command.Parameters.AddWithValue(QueryHelper.ParAddress, NpgsqlTypes.NpgsqlDbType.Text, location.Address);
                command.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlTypes.NpgsqlDbType.Double, location.Rating);
                command.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlTypes.NpgsqlDbType.Double, location.RateCount);
                await command.ExecuteNonQueryAsync();
            

              return await GetLocationByIdAsync(location.Id);
            }

        }

        /// <summary>
        /// Get Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rating"></param>
        /// <returns>
        /// ILocation
        /// </returns>
        public async Task<ILocation> UpdateLocationRatingAsync(Guid id, double rating,double currenRating, int rateCount)
        {
            LocationEntity location = new LocationEntity();

            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetUpdateLocationRatingQueryString(), Connection.CreateConnection()))
            {
                if (Connection.CreateConnection().FullState == ConnectionState.Closed)
                {
                    await Connection.CreateConnection().OpenAsync();
                }

                double NewRating = 0;
                int NewRateCount =rateCount + 1;

                NewRating = (currenRating * rateCount + rating) / NewRateCount;

                command.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlTypes.NpgsqlDbType.Double, id);
                command.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlTypes.NpgsqlDbType.Double, NewRateCount);
                command.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlTypes.NpgsqlDbType.Integer, NewRating);
                await command.ExecuteNonQueryAsync();

                Connection.CreateConnection().Close();


                return await GetLocationByIdAsync(id);

            }

          
        }

        #endregion Methods
    }
}