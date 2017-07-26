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

            using (var connection = Connection.CreateConnection())
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectLocationQueryString(), connection))

            {
                commandSelect.Parameters.AddWithValue(QueryHelper.ParAddress, NpgsqlDbType.Text, address);

                await connection.OpenAsync();

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

            using (var connection = Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetSelectLocationByIdQueryString(), connection))
            {
                command.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlDbType.Uuid, id);

                await connection.OpenAsync();

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

            using (var connection = Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetInsertCreateLocationQueryString(), connection))
            {
                await connection.OpenAsync();

                command.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlDbType.Uuid, location.Id);
                command.Parameters.AddWithValue(QueryHelper.ParAddress, NpgsqlDbType.Text, location.Address);
                command.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlDbType.Double, location.Rating);
                command.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlDbType.Double, location.RateCount);
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
        public async Task<ILocation> UpdateLocationRatingAsync(Guid id, double rating, double currenRating, int rateCount)
        {
            LocationEntity location = new LocationEntity();

            using (var connection = Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetUpdateLocationRatingQueryString(), connection))
            {
                await connection.OpenAsync();

                double NewRating = 0;
                int NewRateCount = rateCount + 1;

                NewRating = (currenRating * rateCount + rating) / NewRateCount;

                command.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlDbType.Uuid, id);
                command.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlDbType.Integer, NewRateCount);
                command.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlDbType.Double, NewRating);
                await command.ExecuteNonQueryAsync();

                return await GetLocationByIdAsync(id);

            }


        }

        #endregion Methods
    }
}