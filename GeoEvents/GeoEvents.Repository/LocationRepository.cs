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
        /// <param name="EventRating"></param>
        /// <param name="EventRatingCount"></param>
        /// <returns>
        /// Location
        /// </returns>
        public async Task<ILocation> GetOrCreateLocationAsync(string address,double EventRating,int EventRatingCount)
        {
            LocationEntity location = new LocationEntity();

            StringBuilder selectLocationString = new StringBuilder();
            selectLocationString.AppendFormat("SELECT * FROM {0} WHERE {1}={2}","location","address","@address");        

            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(selectLocationString.ToString(), Connection.CreateConnection()))
            {
                command.Parameters.AddWithValue("@address", NpgsqlTypes.NpgsqlDbType.Text, address);

                await Connection.CreateConnection().OpenAsync();

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

                    return Mapper.Map<ILocation>(location); 
                }
                else
                {
                    location = new LocationEntity(Guid.NewGuid(),EventRating,EventRatingCount,address);

                    return await CreateLocationAsync(Mapper.Map<ILocation>(location));

                }


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

            StringBuilder selectString = new StringBuilder();
            selectString.AppendFormat("SELECT * FROM {0} WHERE {1}={2}","location","id","@Id");
            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(selectString.ToString(), Connection.CreateConnection()))
            {

                command.Parameters.AddWithValue("@Id", NpgsqlTypes.NpgsqlDbType.Uuid, id);

                await Connection.CreateConnection().OpenAsync();

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
            StringBuilder insertString = new StringBuilder();
            insertString.AppendFormat("INSERT INTO {0} VALUES ({1},{2},{3},{4})",
                "location","@Id","@address","@rating","@ratecount");

            StringBuilder selectString = new StringBuilder();
            selectString.AppendFormat("SELECT * FROM {0} WHERE {1}={2}", "location", "id", "@Id");

            LocationEntity Createdlocation = new LocationEntity();
            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(insertString.ToString(), Connection.CreateConnection()))
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(selectString.ToString(), Connection.CreateConnection()))
            {

                #region Create Location
                command.Parameters.AddWithValue("@Id", NpgsqlTypes.NpgsqlDbType.Uuid, location.Id);
                command.Parameters.AddWithValue("@address", NpgsqlTypes.NpgsqlDbType.Text, location.Address);
                command.Parameters.AddWithValue("@rating", NpgsqlTypes.NpgsqlDbType.Uuid, location.Rating);
                command.Parameters.AddWithValue("@ratecount", NpgsqlTypes.NpgsqlDbType.Uuid, location.RateCount);

                await Connection.CreateConnection().OpenAsync();
                await command.ExecuteNonQueryAsync();
                #endregion


                #region Select Location
                command.Parameters.AddWithValue("@Id", NpgsqlTypes.NpgsqlDbType.Uuid, location.Id);
                DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                if (dr.Read())
                {
                    Createdlocation = new LocationEntity()
                    {
                        Id = new Guid(dr[0].ToString()),
                        Address = dr[1].ToString(),
                        Rating = Convert.ToDouble(dr[2]),
                        RateCount = Convert.ToInt32(dr[3])
                    };
                }
                #endregion
            }


            return Mapper.Map<ILocation>(Createdlocation);
        }


        /// <summary>
        /// Get Location by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Rating"></param>
        /// <returns>
        /// Location
        /// </returns>
        public async Task<ILocation> UpdateLocationRating(Guid id, double Rating)
        {
            StringBuilder UpdateString = new StringBuilder();
            UpdateString.AppendFormat("UPDATE {0} SET {1}={2},{3}={4} WHERE  {5}={6} ",
                "location", "rating", "@rating", "ratecount",
                "@ratecount", "id", "@Id");


            LocationEntity location = new LocationEntity();


            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(UpdateString.ToString(), Connection.CreateConnection()))
            {

                await Connection.CreateConnection().OpenAsync();

                location = Mapper.Map<LocationEntity>(GetLocationByIdAsync(id));

                double NewRating = 0;
                int NewRateCount = location.RateCount+1;

                NewRating = (location.Rating * location.RateCount + Rating) / NewRateCount;

                command.Parameters.AddWithValue("@Id", NpgsqlTypes.NpgsqlDbType.Double, id);
                command.Parameters.AddWithValue("@ratecount", NpgsqlTypes.NpgsqlDbType.Double,NewRateCount);
                command.Parameters.AddWithValue("@rating", NpgsqlTypes.NpgsqlDbType.Integer, NewRating);

                await command.ExecuteNonQueryAsync();

               

            }

            return await GetLocationByIdAsync(id);
        }

        #endregion Methods
    }
}
