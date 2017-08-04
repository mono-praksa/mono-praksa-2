using AutoMapper;
using GeoEvents.Common;
using GeoEvents.DAL;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using log4net;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading.Tasks;

namespace GeoEvents.Repository
{
    public class LocationRepository : ILocationRepository
    {
        #region Properties

        protected IPostgresConnection Connection { get; private set; }
        protected IMapper Mapper { get; private set; }
        private static readonly ILog _log = LogManager.GetLogger(typeof(LocationRepository));

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
        /// Gets the location by adress or craetes it if it's not found asynchronously.
        /// </summary>
        /// <param name="address">Adress of the location to be retrieved or created.</param>
        /// <returns>
        /// The location.
        /// </returns>
        public async Task<ILocation> GetLocationAsync(string address)
        {
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                LocationEntity location = null;
                LocationEntity NewLocation = new LocationEntity(Guid.NewGuid(), 0, 0, address);

                using (var connection = Connection.CreateConnection())
                using (NpgsqlCommand commandSelect = new NpgsqlCommand(LocationQueryHelper.GetSelectLocationQueryString(), connection))
                {
                    commandSelect.Parameters.AddWithValue(QueryConstants.ParAddress, NpgsqlDbType.Text, address);
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
                    dr.Close();

                    if (location == null)
                    {
                        return await CreateLocationAsync(Mapper.Map<ILocation>(NewLocation));
                    }
                    return await GetLocationByIdAsync(location.Id);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
        }

        /// <summary>
        /// Get Location by Id asynchronously.
        /// </summary>
        /// <param name="id">Identifier of the location to be retrieved.</param>
        /// <returns>
        /// The location.
        /// </returns>
        public async Task<ILocation> GetLocationByIdAsync(Guid id)
        {
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                LocationEntity location = new LocationEntity();
                using (var connection = Connection.CreateConnection())
                using (NpgsqlCommand command = new NpgsqlCommand(LocationQueryHelper.GetSelectLocationByIdQueryString(), connection))
                {
                    command.Parameters.AddWithValue(QueryConstants.ParId, NpgsqlDbType.Uuid, id);
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
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
        }

        /// <summary>
        /// Creates Location asynchronously.
        /// </summary>
        /// <param name="location">Location that will be created.</param>
        /// <returns>
        /// The created location.
        /// </returns>
        public async Task<ILocation> CreateLocationAsync(ILocation location)
        {
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                using (var connection = Connection.CreateConnection())
                using (NpgsqlCommand command = new NpgsqlCommand(LocationQueryHelper.GetInsertCreateLocationQueryString(), connection))
                {
                    await connection.OpenAsync();

                    command.Parameters.AddWithValue(QueryConstants.ParId, NpgsqlDbType.Uuid, location.Id);
                    command.Parameters.AddWithValue(QueryConstants.ParAddress, NpgsqlDbType.Text, location.Address);
                    command.Parameters.AddWithValue(QueryConstants.ParRating, NpgsqlDbType.Double, location.Rating);
                    command.Parameters.AddWithValue(QueryConstants.ParRateCount, NpgsqlDbType.Double, location.RateCount);

                    await command.ExecuteNonQueryAsync();
                    return await GetLocationByIdAsync(location.Id);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
        }

        /// <summary>
        /// Update rating of the location asynchronously.
        /// </summary>
        /// <param name="id">Identifier of the location to have it's rating updated</param>
        /// <param name="rating">Rating from the user input(1-5)</param>
        /// <param name="currentRating">Current rating of the location</param>
        /// <param name="rateCount">Current rate count of the location. will be increased by 1</param>
        /// <returns>
        /// The updated location.
        /// </returns>
        public async Task<ILocation> UpdateLocationRatingAsync(Guid id, double rating, double currenRating, int rateCount)
        {
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                LocationEntity location = new LocationEntity();
                using (var connection = Connection.CreateConnection())
                using (NpgsqlCommand command = new NpgsqlCommand(LocationQueryHelper.GetUpdateLocationRatingQueryString(), connection))
                {
                    await connection.OpenAsync();
                    double NewRating = 0;
                    int NewRateCount = rateCount + 1;

                    NewRating = (currenRating * rateCount + rating) / NewRateCount;

                    command.Parameters.AddWithValue(QueryConstants.ParId, NpgsqlDbType.Uuid, id);
                    command.Parameters.AddWithValue(QueryConstants.ParRateCount, NpgsqlDbType.Integer, NewRateCount);
                    command.Parameters.AddWithValue(QueryConstants.ParRating, NpgsqlDbType.Double, NewRating);

                    await command.ExecuteNonQueryAsync();
                    return await GetLocationByIdAsync(id);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
        }
        #endregion Methods
    }
}