using GeoEvents.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Common;
using GeoEvents.DAL;
using Npgsql;
using GeoEvents.Model.Common;
using AutoMapper;
using System.Data.Common;

namespace GeoEvents.Repository
{
    public class EventRepository : IEventRepository
    {
        #region Properties
        protected IPostgresConnection Connection { get; private set; }

        protected IMapper Mapper { get; private set; }
        #endregion Properties

        #region Constructors
        public EventRepository(IPostgresConnection connection, IMapper mapper)
        {
            this.Mapper = mapper;
            this.Connection = connection;
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Creates Event asynchronously.
        /// </summary>
        /// <param name="evt"></param>
        /// <returns>  
        /// return Created event (IEvent)
        /// </returns>
        public async Task<IEvent> CreateEventAsync(IEvent evt)
        {


            using (Connection.CreateConnection())
            using (NpgsqlCommand commandInsert = new NpgsqlCommand(QueryHelper.GetInsertEventString(), Connection.CreateConnection()))
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectEventStringById(), Connection.CreateConnection()))

            {
                commandInsert.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlTypes.NpgsqlDbType.Uuid, evt.Id);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParCategory, NpgsqlTypes.NpgsqlDbType.Integer, evt.Category);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParDescription, NpgsqlTypes.NpgsqlDbType.Text, evt.Description);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParStartTime, NpgsqlTypes.NpgsqlDbType.Timestamp, evt.StartTime);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParEndTime, NpgsqlTypes.NpgsqlDbType.Timestamp, evt.EndTime);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParLat, NpgsqlTypes.NpgsqlDbType.Double, evt.Lat);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParLong, NpgsqlTypes.NpgsqlDbType.Double, evt.Long);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParName, NpgsqlTypes.NpgsqlDbType.Text, evt.Name);
                commandSelect.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlTypes.NpgsqlDbType.Uuid, evt.Id);
                commandSelect.Parameters.AddWithValue(QueryHelper.ParPrice, NpgsqlTypes.NpgsqlDbType.Double, evt.Price);
                commandSelect.Parameters.AddWithValue(QueryHelper.ParCapacity, NpgsqlTypes.NpgsqlDbType.Integer, evt.Capacity);
                commandSelect.Parameters.AddWithValue(QueryHelper.ParReserved, NpgsqlTypes.NpgsqlDbType.Integer, evt.Reserved);
                commandSelect.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlTypes.NpgsqlDbType.Double, evt.Rating);
                commandSelect.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlTypes.NpgsqlDbType.Integer, evt.RateCount);

                await Connection.CreateConnection().OpenAsync();
                await commandInsert.ExecuteNonQueryAsync();

                DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                while (dr.Read())
                {
                    EventEntity evtR = new EventEntity
                    {
                        Id = new Guid(dr[0].ToString()),
                        StartTime = Convert.ToDateTime(dr[1]),
                        EndTime = Convert.ToDateTime(dr[2]),
                        Lat = Convert.ToDecimal(dr[3]),
                        Long = Convert.ToDecimal(dr[4]),
                        Name = dr[5].ToString(),
                        Description = dr[6].ToString(),
                        Category = Convert.ToInt32(dr[7]),
                        Price = Convert.ToDecimal(dr[8]),
                        Capacity = Convert.ToInt32(dr[9]),
                        Reserved = Convert.ToInt32(dr[10]),
                        Rating = Convert.ToDecimal(dr[11]),
                        RateCount = Convert.ToInt32(dr[12])
                    };
                }


            };


            return evt;


        }

        /// <summary>
        /// Gets filtered Events asynchronously.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// list of Events.
        /// </returns>
        public async Task<IEnumerable<IEvent>> GetEventsAsync(IFilter filter)
        {
            EventEntity tmp;
            List<IEvent> SelectEvents = new List<IEvent>();

            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetSelectEventString(filter), Connection.CreateConnection()))
            {

                SetParametersSearchEvents(filter, command);

                await Connection.CreateConnection().OpenAsync();
                DbDataReader dr = await command.ExecuteReaderAsync();

                while (dr.Read())
                {
                    tmp = new EventEntity
                    {
                        Id = new Guid(dr[0].ToString()),
                        StartTime = Convert.ToDateTime(dr[1]),
                        EndTime = Convert.ToDateTime(dr[2]),
                        Lat = Convert.ToDecimal(dr[3]),
                        Long = Convert.ToDecimal(dr[4]),
                        Name = dr[5].ToString(),
                        Description = dr[6].ToString(),
                        Category = Convert.ToInt32(dr[7]),
                        Price = Convert.ToDecimal(dr[8]),
                        Capacity = Convert.ToInt32(dr[9]),
                        Reserved = Convert.ToInt32(dr[10]),
                        Rating = Convert.ToDecimal(dr[11]),
                        RateCount = Convert.ToInt32(dr[12])

                    };

                    SelectEvents.Add(Mapper.Map<IEvent>(tmp));
                }


            }

            return SelectEvents;

        }


        /// <summary>
        /// Get event count asynchronously.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// number of events .
        /// </returns>
        public async Task<Int64> GetEventCountAsync(IFilter filter)
        {
            Int64 Count;

            using (Connection.CreateConnection())
            using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetSelectCountEventString(filter), Connection.CreateConnection()))
            {

                SetParametersSearchEvents(filter, command);

                await Connection.CreateConnection().OpenAsync();
                object dr = await command.ExecuteScalarAsync();

                Count = Convert.ToInt64(dr);

            }




            return Count;

        }

        /// <summary>
        /// Sets Parameters of NpgsqlCommand by Filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="command"></param>
        private void SetParametersSearchEvents(IFilter filter, NpgsqlCommand command)
        {

            if (filter.ULat != null) { command.Parameters.AddWithValue(QueryHelper.ParLat, NpgsqlTypes.NpgsqlDbType.Double, filter.ULat); }
            if (filter.ULong != null) { command.Parameters.AddWithValue(QueryHelper.ParLong, NpgsqlTypes.NpgsqlDbType.Double, filter.ULong); }
            if (filter.Radius != null) { command.Parameters.AddWithValue(QueryHelper.ParRadius, NpgsqlTypes.NpgsqlDbType.Double, filter.Radius * 1000); }
            if (filter.StartTime != null) { command.Parameters.AddWithValue(QueryHelper.ParUserStartTime, NpgsqlTypes.NpgsqlDbType.Timestamp, filter.StartTime); }
            if (filter.EndTime != null) { command.Parameters.AddWithValue(QueryHelper.ParUserEndTime, NpgsqlTypes.NpgsqlDbType.Timestamp, filter.EndTime); }
            if (filter.Category != null) { command.Parameters.AddWithValue(QueryHelper.ParCategory, NpgsqlTypes.NpgsqlDbType.Integer, filter.Category); }
            if (!string.IsNullOrWhiteSpace(filter.SearchString)) { command.Parameters.AddWithValue(QueryHelper.ParSearchString, NpgsqlTypes.NpgsqlDbType.Varchar, "%" + filter.SearchString + "%"); }


        }


        /// <summary>
        /// Updates the rating asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="rating">The rating.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<IEvent> UpdateRatingAsync(Guid eventId, decimal rating)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the reservation asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task<IEvent> UpdateReservationAsync(Guid eventId)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}