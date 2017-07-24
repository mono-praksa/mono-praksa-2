using AutoMapper;
using GeoEvents.Common;
using GeoEvents.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Model.Common;
using GeoEvents.DAL;
using Npgsql;
using System.Data.Common;
using System.Data;
using NpgsqlTypes;

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
        /// Location
        /// </returns>
        public async Task<ILocation> GetLocationAsync(string address)
        {
            LocationEntity location = null;
            LocationEntity NewLocation = new LocationEntity(Guid.NewGuid(), 0, 0, address);

            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetSelectLocationQueryString(), Connection.CreateConnection()))
            using (NpgsqlCommand commandInsert = new NpgsqlCommand(QueryHelper.GetInsertCreateLocationQueryString(),Connection.CreateConnection()))
            {
                command.Parameters.AddWithValue(QueryHelper.ParAddress, NpgsqlTypes.NpgsqlDbType.Text, address);
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
                else
                {
                    Connection.CreateConnection().Close();
                    await Connection.CreateConnection().OpenAsync();

                    commandInsert.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlDbType.Uuid, NewLocation.Id);
                    commandInsert.Parameters.AddWithValue(QueryHelper.ParAddress, NpgsqlDbType.Text, NewLocation.Address);
                    commandInsert.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlDbType.Double, NewLocation.Rating);
                    commandInsert.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlDbType.Integer, NewLocation.RateCount);
                    await commandInsert.ExecuteNonQueryAsync();
                }
            }
            if (location == null)
            {
                return await GetLocationByIdAsync(NewLocation.Id);

            }
            else
            {
                return Mapper.Map<ILocation>(location);
            }
        }


        /// <summary>
        /// Get Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// Location
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
        /// Location
        /// </returns>
        public async Task<ILocation> CreateLocationAsync(ILocation location)
        {

            ILocation NewLocation;
            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetInsertCreateLocationQueryString(), Connection.CreateConnection()))
            {
                #region Create Location
                if (Connection.CreateConnection().FullState == ConnectionState.Closed)
                {
                    await Connection.CreateConnection().OpenAsync();
                }
                command.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlTypes.NpgsqlDbType.Uuid, location.Id);
                command.Parameters.AddWithValue(QueryHelper.ParAddress, NpgsqlTypes.NpgsqlDbType.Text, location.Address);
                command.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlTypes.NpgsqlDbType.Uuid, location.Rating);
                command.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlTypes.NpgsqlDbType.Uuid, location.RateCount);
                await command.ExecuteNonQueryAsync();
                #endregion
            }
            using (Connection.CreateConnection())
            {
                NewLocation = await GetLocationByIdAsync(location.Id);
            }
            return NewLocation;

        }
        /// <summary>
        /// Get Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rating"></param>
        /// <returns>
        /// Location
        /// </returns>
        public async Task<ILocation> UpdateLocationRatingAsync(Guid id, double rating)
        {
            LocationEntity location = new LocationEntity();
            ILocation NewLocation;
            using (Connection.CreateConnection())
            {
                location = Mapper.Map<LocationEntity>(await GetLocationByIdAsync(id));
            }

            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetUpdateLocationRatingQueryString(), Connection.CreateConnection()))
            {
                if (Connection.CreateConnection().FullState == ConnectionState.Closed)
                {
                    await Connection.CreateConnection().OpenAsync();
                }
                double NewRating = 0;
                int NewRateCount = location.RateCount + 1;
                NewRating = (location.Rating * location.RateCount + rating) / NewRateCount;

                command.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlTypes.NpgsqlDbType.Double, id);
                command.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlTypes.NpgsqlDbType.Double, NewRateCount);
                command.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlTypes.NpgsqlDbType.Integer, NewRating);
                await command.ExecuteNonQueryAsync();
            }
            using (Connection.CreateConnection()) { 
            NewLocation = await GetLocationByIdAsync(id);
            }
            return NewLocation;
        }
        #endregion Methods
    }
}
