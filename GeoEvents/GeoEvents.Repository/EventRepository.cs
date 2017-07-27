using AutoMapper;
using GeoEvents.Common;
using GeoEvents.DAL;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using X.PagedList;

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
            EventEntity evtR = new EventEntity();

            using (var connection = Connection.CreateConnection())
            using (NpgsqlCommand commandInsert = new NpgsqlCommand(QueryHelper.GetInsertEventQueryString(), connection))
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectEventByIdQueryString(), connection))
            {
                commandInsert.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlDbType.Uuid, evt.Id);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParCategory, NpgsqlDbType.Integer, evt.Category);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParDescription, NpgsqlDbType.Text, evt.Description);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParStartTime, NpgsqlDbType.Timestamp, evt.StartTime);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParEndTime, NpgsqlDbType.Timestamp, evt.EndTime);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParLatitude, NpgsqlDbType.Double, evt.Latitude);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParLongitude, NpgsqlDbType.Double, evt.Longitude);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParName, NpgsqlDbType.Text, evt.Name);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParPrice, NpgsqlDbType.Double, evt.Price);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParCapacity, NpgsqlDbType.Integer, evt.Capacity);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParReserved, NpgsqlDbType.Integer, evt.Reserved);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlDbType.Double, evt.Rating);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlDbType.Integer, evt.RateCount);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParLocationId, NpgsqlDbType.Uuid, evt.LocationId);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParCustom, NpgsqlDbType.Jsonb, evt.Custom);


                await connection.OpenAsync();

                await commandInsert.ExecuteNonQueryAsync();

                commandSelect.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlDbType.Uuid, evt.Id);

                DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                while (dr.Read())
                {
                    evtR = new EventEntity
                    {
                        Id = new Guid(dr[0].ToString()),
                        Name = dr[1].ToString(),
                        Description = dr[2].ToString(),
                        Category = Convert.ToInt32(dr[3]),
                        Latitude = Convert.ToDouble(dr[4]),
                        Longitude = Convert.ToDouble(dr[5]),
                        StartTime = Convert.ToDateTime(dr[6]),
                        EndTime = Convert.ToDateTime(dr[7]),
                        Rating = Convert.ToDouble(dr[8]),
                        RateCount = Convert.ToInt32(dr[9]),
                        Price = Convert.ToDouble(dr[10]),
                        Capacity = Convert.ToInt32(dr[11]),
                        Reserved = Convert.ToInt32(dr[12]),
                        Custom = dr[13].ToString(),
                        LocationId = new Guid(dr[14].ToString())
                    };
                }
            };
            return Mapper.Map<IEvent>(evtR);
        }

        /// <summary>
        /// Gets filtered Events asynchronously.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// list of Events.
        /// </returns>
        public async Task<StaticPagedList<IEvent>> GetEventsAsync(IFilter filter)
        {
            EventEntity tmp;
            int count;
            List<IEvent> SelectEvents = new List<IEvent>();
            StaticPagedList<IEvent> result;

            using (var connection = Connection.CreateConnection())
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectEventQueryString(filter), connection))
            using (NpgsqlCommand commandCount = new NpgsqlCommand(QueryHelper.GetSelectCountEventQueryString(filter), connection))
            {
                await connection.OpenAsync();
                SetParametersSearchEvents(filter, commandSelect);
                DbDataReader dr = await commandSelect.ExecuteReaderAsync();

                while (dr.Read())
                {
                    tmp = new EventEntity
                    {
                        Id = new Guid(dr[0].ToString()),
                        Name = dr[1].ToString(),
                        Description = dr[2].ToString(),
                        Category = Convert.ToInt32(dr[3]),
                        Latitude = Convert.ToDouble(dr[4]),
                        Longitude = Convert.ToDouble(dr[5]),
                        StartTime = Convert.ToDateTime(dr[6]),
                        EndTime = Convert.ToDateTime(dr[7]),
                        Rating = Convert.ToDouble(dr[8]),
                        RateCount = Convert.ToInt32(dr[9]),
                        Price = Convert.ToDouble(dr[10]),
                        Capacity = Convert.ToInt32(dr[11]),
                        Reserved = Convert.ToInt32(dr[12]),
                        Custom = dr[13].ToString(),
                        LocationId = new Guid(dr[14].ToString())
                    };
                    SelectEvents.Add(Mapper.Map<IEvent>(tmp));
                }
                dr.Close();

                SetParametersSearchEvents(filter, commandCount);
                object sc = await commandCount.ExecuteScalarAsync();
                count = Convert.ToInt32(sc);

                result = new StaticPagedList<IEvent>(SelectEvents, filter.PageNumber, filter.PageSize, count);
            }
            return result;
        }



        /// <summary>
        /// Sets Parameters of NpgsqlCommand by Filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="command"></param>
        private void SetParametersSearchEvents(IFilter filter, NpgsqlCommand command)
        {
            if (filter.ULat != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParLatitude, NpgsqlDbType.Double, filter.ULat);
            }
            if (filter.ULong != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParLongitude, NpgsqlDbType.Double, filter.ULong);
            }
            if (filter.Radius != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParRadius, NpgsqlDbType.Double, filter.Radius * 1000);
            }
            if (filter.StartTime != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParUserStartTime, NpgsqlDbType.Timestamp, filter.StartTime);
            }
            if (filter.EndTime != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParUserEndTime, NpgsqlDbType.Timestamp, filter.EndTime);
            }
            if (filter.Category != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParCategory, NpgsqlDbType.Integer, filter.Category);
            }
            if (filter.Price != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParPrice, NpgsqlDbType.Double, filter.Price);
            }
            if (filter.RatingEvent != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlDbType.Double, filter.RatingEvent);
            }
            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                command.Parameters.AddWithValue(QueryHelper.ParSearchString, NpgsqlDbType.Varchar, "%" + filter.SearchString + "%");
            }
            if (!string.IsNullOrWhiteSpace(filter.Custom))
            {
                command.Parameters.AddWithValue(QueryHelper.ParSearchString, NpgsqlDbType.Jsonb, filter.Custom);
            }
            if (!string.IsNullOrWhiteSpace(filter.Custom))
            {
                command.Parameters.AddWithValue(QueryHelper.ParCustom, NpgsqlDbType.Jsonb, filter.Custom);
            }
        }

        /// <summary>
        /// Updates the rating asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="rating">The rating.</param>
        /// <param name="CurrentRating"></param>
        /// <param name="RateCount"></param>
        /// <returns>
        /// Updated Event
        /// </returns>
        public async Task<IEvent> UpdateRatingAsync(Guid eventId, double rating, double CurrentRating, int RateCount)
        {
            EventEntity evtR = new EventEntity();

            using (var connection = Connection.CreateConnection())
            using (NpgsqlCommand commandUpdateRating = new NpgsqlCommand(QueryHelper.GetsInsertUpdateRatingQueryString(), connection))
            using (NpgsqlCommand commandSelectUpdated = new NpgsqlCommand(QueryHelper.GetSelectEventByIdQueryString(), connection))
            {
                await connection.OpenAsync();

                int NewRateCount = RateCount + 1;
                double NewRating = (RateCount * CurrentRating + rating) / Convert.ToDouble(NewRateCount);

                commandUpdateRating.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlDbType.Uuid, eventId);
                commandUpdateRating.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlDbType.Double, NewRating);
                commandUpdateRating.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlDbType.Integer, NewRateCount);

                await commandUpdateRating.ExecuteNonQueryAsync();

                commandSelectUpdated.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlDbType.Uuid, eventId);
                DbDataReader drSelect = await commandSelectUpdated.ExecuteReaderAsync();

                while (drSelect.Read())
                {
                    evtR = new EventEntity
                    {
                        Id = new Guid(drSelect[0].ToString()),
                        Name = drSelect[1].ToString(),
                        Description = drSelect[2].ToString(),
                        Category = Convert.ToInt32(drSelect[3]),
                        Latitude = Convert.ToDouble(drSelect[4]),
                        Longitude = Convert.ToDouble(drSelect[5]),
                        StartTime = Convert.ToDateTime(drSelect[6]),
                        EndTime = Convert.ToDateTime(drSelect[7]),
                        Rating = Convert.ToDouble(drSelect[8]),
                        RateCount = Convert.ToInt32(drSelect[9]),
                        Price = Convert.ToDouble(drSelect[10]),
                        Capacity = Convert.ToInt32(drSelect[11]),
                        Reserved = Convert.ToInt32(drSelect[12]),
                        Custom = drSelect[13].ToString(),
                        LocationId = new Guid(drSelect[14].ToString())
                    };
                }
            }
            return Mapper.Map<IEvent>(evtR);
        }

        /// <summary>
        /// Updates the reservation asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns>
        /// Updated Event
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<IEvent> UpdateReservationAsync(Guid eventId)
        {
            int parReserved = 0;
            EventEntity evtR = new EventEntity();

            using (var connection = Connection.CreateConnection())
            using (NpgsqlCommand commandGetReserved = new NpgsqlCommand(QueryHelper.GetSelectCurrentReservationQueryString(), connection))
            using (NpgsqlCommand commandUpdate = new NpgsqlCommand(QueryHelper.GetInsertUpdateReservationQueryString(), connection))
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectEventByIdQueryString(), connection))
            {
                commandGetReserved.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlDbType.Uuid, eventId);

                await connection.OpenAsync();

                object reservedObj = await commandGetReserved.ExecuteScalarAsync();
                parReserved = Convert.ToInt32(reservedObj);
                parReserved++;

                commandUpdate.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlDbType.Uuid, eventId);
                commandUpdate.Parameters.AddWithValue(QueryHelper.ParReserved, NpgsqlDbType.Integer, parReserved);

                await commandUpdate.ExecuteNonQueryAsync();

                commandSelect.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlDbType.Uuid, eventId);
                DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                while (dr.Read())
                {
                    evtR = new EventEntity
                    {
                        Id = new Guid(dr[0].ToString()),
                        Name = dr[1].ToString(),
                        Description = dr[2].ToString(),
                        Category = Convert.ToInt32(dr[3]),
                        Latitude = Convert.ToDouble(dr[4]),
                        Longitude = Convert.ToDouble(dr[5]),
                        StartTime = Convert.ToDateTime(dr[6]),
                        EndTime = Convert.ToDateTime(dr[7]),
                        Rating = Convert.ToDouble(dr[8]),
                        RateCount = Convert.ToInt32(dr[9]),
                        Price = Convert.ToDouble(dr[10]),
                        Capacity = Convert.ToInt32(dr[11]),

                        Reserved = Convert.ToInt32(dr[12]),
                        Custom = dr[13].ToString(),
                        LocationId = new Guid(dr[14].ToString())
                    };
                }
            }
            return Mapper.Map<IEvent>(evtR);
        }

        /// <summary>
        /// Get event by Id
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns>
        /// Event
        /// </returns>
        public async Task<IEvent> GetEventByIdAsync(Guid eventId)
        {
            EventEntity evtR = new EventEntity();

            using (var connection = Connection.CreateConnection())
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectEventByIdQueryString(), connection))
            {

                await connection.OpenAsync();

                commandSelect.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlDbType.Uuid, eventId);

                DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                if (dr.Read())
                {
                    evtR = new EventEntity
                    {
                        Id = new Guid(dr[0].ToString()),
                        Name = dr[1].ToString(),
                        Description = dr[2].ToString(),
                        Category = Convert.ToInt32(dr[3]),
                        Latitude = Convert.ToDouble(dr[4]),
                        Longitude = Convert.ToDouble(dr[5]),
                        StartTime = Convert.ToDateTime(dr[6]),
                        EndTime = Convert.ToDateTime(dr[7]),
                        Rating = Convert.ToDouble(dr[8]),
                        RateCount = Convert.ToInt32(dr[9]),
                        Price = Convert.ToDouble(dr[10]),
                        Capacity = Convert.ToInt32(dr[11]),

                        Reserved = Convert.ToInt32(dr[12]),
                        Custom = dr[13].ToString(),
                        LocationId = new Guid(dr[14].ToString())
                    };
                }
            }
            return Mapper.Map<IEvent>(evtR);
        }

        #endregion Methods
    }
}