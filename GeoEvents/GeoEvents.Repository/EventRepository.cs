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
using log4net;
using System.Reflection;


[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace GeoEvents.Repository
{
    public class EventRepository : IEventRepository
    {
        #region Properties
        protected IPostgresConnection Connection { get; private set; }
        private static readonly ILog _log = LogManager.GetLogger(typeof(EventRepository));
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
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                EventEntity evtR = new EventEntity();

                using (var connection = Connection.CreateConnection())
                using (NpgsqlCommand commandSelect = new NpgsqlCommand(EventQueryHelper.GetSelectEventByIdQueryString(), connection))
                using (NpgsqlCommand commandInsert = new NpgsqlCommand(EventQueryHelper.GetInsertEventQueryString(), connection))
                {
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParId, NpgsqlDbType.Uuid, evt.Id);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParCategory, NpgsqlDbType.Integer, evt.Category);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParDescription, NpgsqlDbType.Text, evt.Description);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParStartTime, NpgsqlDbType.Timestamp, evt.StartTime);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParEndTime, NpgsqlDbType.Timestamp, evt.EndTime);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParLatitude, NpgsqlDbType.Double, evt.Latitude);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParLongitude, NpgsqlDbType.Double, evt.Longitude);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParName, NpgsqlDbType.Text, evt.Name);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParPrice, NpgsqlDbType.Double, evt.Price);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParCapacity, NpgsqlDbType.Integer, evt.Capacity);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParReserved, NpgsqlDbType.Integer, evt.Reserved);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParRating, NpgsqlDbType.Double, evt.Rating);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParRateCount, NpgsqlDbType.Integer, evt.RateCount);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParLocationId, NpgsqlDbType.Uuid, evt.LocationId);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParCustom, NpgsqlDbType.Jsonb, evt.Custom);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParOccurrence, NpgsqlDbType.Text, evt.Occurrence);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParRepeatEvery, NpgsqlDbType.Integer, evt.RepeatEvery);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParRepeatOn, NpgsqlDbType.Integer, evt.RepeatOn);
                    commandInsert.Parameters.AddWithValue(QueryConstants.ParRepeatCount, NpgsqlDbType.Integer, evt.RepeatCount);

                    await connection.OpenAsync();
                    await commandInsert.ExecuteNonQueryAsync();

                    commandSelect.Parameters.AddWithValue(QueryConstants.ParEventId, NpgsqlDbType.Uuid, evt.Id);

                    DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                    while (dr.Read())
                    {
                        evtR = SetEventEntityFromReader(dr);
                    }
                }
                return Mapper.Map<IEvent>(evtR);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
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
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                EventEntity tmp;
                int count;
                List<IEvent> SelectEvents = new List<IEvent>();
                StaticPagedList<IEvent> result;

                using (var connection = Connection.CreateConnection())
                using (NpgsqlCommand commandCount = new NpgsqlCommand(EventQueryHelper.GetSelectCountEventQueryString(filter), connection))
                using (NpgsqlCommand commandSelect = new NpgsqlCommand(EventQueryHelper.GetSelectEventQueryString(filter), connection))
                {
                    await connection.OpenAsync();
                    SetParametersSearchEvents(filter, commandSelect);
                    DbDataReader dr = await commandSelect.ExecuteReaderAsync();

                    while (dr.Read())
                    {
                        tmp = SetEventEntityFromReader(dr);

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
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
        }

        /// <summary>
        /// Gets all filtered events asynchronously
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// List of Events
        /// </returns>
        public async Task<List<IEvent>> GetAllEventsAsync(IFilter filter)
        {
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                EventEntity tmp;
                List<IEvent> SelectEvents = new List<IEvent>();

                using (var connection = Connection.CreateConnection())
                using (NpgsqlCommand commandSelect = new NpgsqlCommand(EventQueryHelper.GetSelectAllEventsQueryString(filter), connection))
                {
                    await connection.OpenAsync();
                    SetParametersSearchEvents(filter, commandSelect);
                    DbDataReader dr = await commandSelect.ExecuteReaderAsync();

                    while (dr.Read())
                    {
                        tmp = SetEventEntityFromReader(dr);
                        SelectEvents.Add(Mapper.Map<IEvent>(tmp));
                    }
                    dr.Close();
                }
                return SelectEvents;
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
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
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                EventEntity evtR = new EventEntity();

                using (var connection = Connection.CreateConnection())
                using (NpgsqlCommand commandSelectUpdated = new NpgsqlCommand(EventQueryHelper.GetSelectEventByIdQueryString(), connection))
                using (NpgsqlCommand commandUpdateRating = new NpgsqlCommand(EventQueryHelper.GetsInsertUpdateRatingQueryString(), connection))
                {
                    await connection.OpenAsync();

                    int NewRateCount = RateCount + 1;
                    double NewRating = (RateCount * CurrentRating + rating) / Convert.ToDouble(NewRateCount);

                    commandUpdateRating.Parameters.AddWithValue(QueryConstants.ParEventId, NpgsqlDbType.Uuid, eventId);
                    commandUpdateRating.Parameters.AddWithValue(QueryConstants.ParRating, NpgsqlDbType.Double, NewRating);
                    commandUpdateRating.Parameters.AddWithValue(QueryConstants.ParRateCount, NpgsqlDbType.Integer, NewRateCount);

                    await commandUpdateRating.ExecuteNonQueryAsync();

                    commandSelectUpdated.Parameters.AddWithValue(QueryConstants.ParEventId, NpgsqlDbType.Uuid, eventId);
                    DbDataReader drSelect = await commandSelectUpdated.ExecuteReaderAsync();

                    while (drSelect.Read())
                    {
                        evtR = SetEventEntityFromReader(drSelect);
                    }

                    return Mapper.Map<IEvent>(evtR);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
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
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                int parReserved = 0;
                EventEntity evtR = new EventEntity();

                using (var connection = Connection.CreateConnection())
                using (NpgsqlCommand commandSelect = new NpgsqlCommand(EventQueryHelper.GetSelectEventByIdQueryString(), connection))
                using (NpgsqlCommand commandUpdate = new NpgsqlCommand(EventQueryHelper.GetInsertUpdateReservationQueryString(), connection))
                using (NpgsqlCommand commandGetReserved = new NpgsqlCommand(EventQueryHelper.GetSelectCurrentReservationQueryString(), connection))
                {
                    commandGetReserved.Parameters.AddWithValue(QueryConstants.ParEventId, NpgsqlDbType.Uuid, eventId);

                    await connection.OpenAsync();

                    object reservedObj = await commandGetReserved.ExecuteScalarAsync();
                    parReserved = Convert.ToInt32(reservedObj);
                    parReserved++;

                    commandUpdate.Parameters.AddWithValue(QueryConstants.ParEventId, NpgsqlDbType.Uuid, eventId);
                    commandUpdate.Parameters.AddWithValue(QueryConstants.ParReserved, NpgsqlDbType.Integer, parReserved);

                    await commandUpdate.ExecuteNonQueryAsync();

                    commandSelect.Parameters.AddWithValue(QueryConstants.ParEventId, NpgsqlDbType.Uuid, eventId);
                    DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                    while (dr.Read())
                    {
                        evtR = SetEventEntityFromReader(dr);
                    }
                }
                return Mapper.Map<IEvent>(evtR);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
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
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                EventEntity evtR = new EventEntity();
                using (var connection = Connection.CreateConnection())
                using (NpgsqlCommand commandSelect = new NpgsqlCommand(EventQueryHelper.GetSelectEventByIdQueryString(), connection))
                {
                    await connection.OpenAsync();
                    commandSelect.Parameters.AddWithValue(QueryConstants.ParEventId, NpgsqlDbType.Uuid, eventId);
                    DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                    if (dr.Read())
                    {
                        evtR = SetEventEntityFromReader(dr);
                    }
                }
                return Mapper.Map<IEvent>(evtR);
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
        }

        /// <summary>
        /// Sets Parameters of NpgsqlCommand by Filter
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="command"></param>
        private void SetParametersSearchEvents(IFilter filter, NpgsqlCommand command)
        {
            log4net.GlobalContext.Properties["AppName"] = Assembly.GetExecutingAssembly().FullName;
            try
            {
                if (filter.ULat != null)
                {
                    command.Parameters.AddWithValue(QueryConstants.ParLatitude, NpgsqlDbType.Double, filter.ULat);
                }
                if (filter.ULong != null)
                {
                    command.Parameters.AddWithValue(QueryConstants.ParLongitude, NpgsqlDbType.Double, filter.ULong);
                }
                if (filter.Radius != null)
                {
                    command.Parameters.AddWithValue(QueryConstants.ParRadius, NpgsqlDbType.Double, filter.Radius * 1000);
                }
                if (filter.Category != null)
                {
                    command.Parameters.AddWithValue(QueryConstants.ParCategory, NpgsqlDbType.Integer, filter.Category);
                }
                if (filter.Price != null)
                {
                    command.Parameters.AddWithValue(QueryConstants.ParPrice, NpgsqlDbType.Double, filter.Price);
                }
                if (filter.RatingEvent != null)
                {
                    command.Parameters.AddWithValue(QueryConstants.ParRating, NpgsqlDbType.Double, filter.RatingEvent);
                }
                if (!string.IsNullOrWhiteSpace(filter.SearchString))
                {
                    command.Parameters.AddWithValue(QueryConstants.ParSearchString, NpgsqlDbType.Varchar, "%" + filter.SearchString + "%");
                }
                if (!string.IsNullOrWhiteSpace(filter.Custom))
                {
                    command.Parameters.AddWithValue(QueryConstants.ParSearchString, NpgsqlDbType.Jsonb, filter.Custom);
                }
                if (!string.IsNullOrWhiteSpace(filter.Custom))
                {
                    command.Parameters.AddWithValue(QueryConstants.ParCustom, NpgsqlDbType.Jsonb, filter.Custom);
                }
                if (filter.StartTime != null)
                {
                    command.Parameters.AddWithValue(QueryConstants.ParUserStartTime, NpgsqlDbType.Timestamp, filter.StartTime);
                }
                if (filter.EndTime != null)
                {
                    command.Parameters.AddWithValue(QueryConstants.ParUserEndTime, NpgsqlDbType.Timestamp, filter.EndTime);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex.StackTrace, ex);
                throw new Exception(ex.StackTrace);
            }
        }

        /// <summary>
        /// SetsParametrs form reader to new EventEntity
        /// </summary>
        /// <param name="dr"></param>
        /// <returns>
        /// EventEntity
        /// </returns>
        private EventEntity SetEventEntityFromReader(DbDataReader dr)
        {
            return new EventEntity
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
                Occurrence = dr[14].ToString(),
                RepeatEvery = Convert.ToInt32(dr[15]),
                RepeatOn = Convert.ToInt32(dr[16]),
                RepeatCount = Convert.ToInt32(dr[17]),
                LocationId = new Guid(dr[18].ToString())
            };
        }

        #endregion Methods
    }
}