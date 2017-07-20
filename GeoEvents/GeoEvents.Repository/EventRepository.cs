﻿using AutoMapper;
using GeoEvents.Common;
using GeoEvents.DAL;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

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
            using (Connection.CreateConnection())
            using (NpgsqlCommand commandInsert = new NpgsqlCommand(QueryHelper.GetInsertEventString(), Connection.CreateConnection()))
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectEventStringById(), Connection.CreateConnection()))
            {
                commandInsert.Parameters.AddWithValue(QueryHelper.ParId, NpgsqlDbType.Uuid, evt.Id);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParCategory, NpgsqlDbType.Integer, evt.Category);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParDescription, NpgsqlDbType.Text, evt.Description);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParStartTime, NpgsqlDbType.Timestamp, evt.StartTime);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParEndTime, NpgsqlDbType.Timestamp, evt.EndTime);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParLat, NpgsqlDbType.Double, evt.Latitude);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParLong, NpgsqlDbType.Double, evt.Longitude);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParName, NpgsqlDbType.Text, evt.Name);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParPrice, NpgsqlDbType.Double, evt.Price);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParCapacity, NpgsqlDbType.Integer, evt.Capacity);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParReserved, NpgsqlDbType.Integer, evt.Reserved);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlDbType.Double, evt.Rating);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlDbType.Integer, evt.RateCount);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParLocationId, NpgsqlDbType.Uuid, evt.LocationId);
                commandInsert.Parameters.AddWithValue(QueryHelper.ParCustom, NpgsqlDbType.Jsonb, evt.Custom);

                commandSelect.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlDbType.Uuid, evt.Id);



                await Connection.CreateConnection().OpenAsync();




                await commandInsert.ExecuteNonQueryAsync();

                DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                while (dr.Read())
                {
                    evtR = new EventEntity
                    {
                        Id = new Guid(dr[0].ToString()),
                        Name = dr[1].ToString(),
                        Description = dr[2].ToString(),
                        Category = Convert.ToInt32(dr[3]),
                        Latitude = Convert.ToDecimal(dr[4]),
                        Longitude = Convert.ToDecimal(dr[5]),
                        StartTime = Convert.ToDateTime(dr[6]),
                        EndTime = Convert.ToDateTime(dr[7]),
                        Rating = Convert.ToDecimal(dr[8]),
                        RateCount = Convert.ToInt32(dr[9]),
                        Price = Convert.ToDecimal(dr[10]),
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
                        Name = dr[1].ToString(),
                        Description = dr[2].ToString(),
                        Category = Convert.ToInt32(dr[3]),
                        Latitude = Convert.ToDecimal(dr[4]),
                        Longitude = Convert.ToDecimal(dr[5]),
                        StartTime = Convert.ToDateTime(dr[6]),
                        EndTime = Convert.ToDateTime(dr[7]),
                        Rating = Convert.ToDecimal(dr[8]),
                        RateCount = Convert.ToInt32(dr[9]),
                        Price = Convert.ToDecimal(dr[10]),
                        Capacity = Convert.ToInt32(dr[11]),

                        Reserved = Convert.ToInt32(dr[12]),
                        Custom = dr[13].ToString(),
                        LocationId = new Guid(dr[14].ToString())

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
            if (filter.ULat != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParLat, NpgsqlTypes.NpgsqlDbType.Double, filter.ULat);
            }
            if (filter.ULong != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParLong, NpgsqlTypes.NpgsqlDbType.Double, filter.ULong);
            }

            if (filter.Radius != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParRadius, NpgsqlTypes.NpgsqlDbType.Double, filter.Radius * 1000);
            }

            if (filter.StartTime != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParUserStartTime, NpgsqlTypes.NpgsqlDbType.Timestamp, filter.StartTime);
            }

            if (filter.EndTime != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParUserEndTime, NpgsqlTypes.NpgsqlDbType.Timestamp, filter.EndTime);
            }

            if (filter.Category != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParCategory, NpgsqlTypes.NpgsqlDbType.Integer, filter.Category);
            }

            if (filter.Price != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParPrice, NpgsqlTypes.NpgsqlDbType.Double, filter.Price);
            }

            if (filter.RatingEvent != null)
            {
                command.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlTypes.NpgsqlDbType.Double, filter.RatingEvent);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                command.Parameters.AddWithValue(QueryHelper.ParSearchString, NpgsqlTypes.NpgsqlDbType.Varchar, "%" + filter.SearchString + "%");
            }
        
        }

        /// <summary>
        /// Updates the rating asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <param name="rating">The rating.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<IEvent> UpdateRatingAsync(Guid eventId, decimal rating)
        {

            EventEntity evtR = new EventEntity();
            decimal CurrentRating=0;
            int CurrentRateCount=0;
            decimal LatToUpdate = 0;
            decimal LongToUpdate = 0;
            StringBuilder RatingLocationString = new StringBuilder();
            RatingLocationString.AppendFormat("SELECT {0},{1} FROM {2} WHERE ll_to_earth({3},{4}) = ll_to_earth({5}, {6}) ",
                "\"Rating\"","\"RateCount\"","\"Events\"","\"Events\".\"Lat\"",
                "\"Events\".\"Long\"","@Lat","@Long");

            StringBuilder UpdateAllLocationRating = new StringBuilder();
            UpdateAllLocationRating.AppendFormat("UPDATE {0} SET {1}={2} WHERE ll_to_earth({3},{4}) = ll_to_earth({5}, {6}) ",
                "\"Events\"","\"RatingLocation\"","@RatingLocation", "\"Events\".\"Lat\"",
                "\"Events\".\"Long\"", "@Lat", "@Long");


            using (Connection.CreateConnection())
            using (NpgsqlCommand commandGetRating = new NpgsqlCommand(QueryHelper.GetSelectUpdateRatingString(), Connection.CreateConnection()))
            using (NpgsqlCommand commandUpdateRating = new NpgsqlCommand(QueryHelper.GetsInsertUpdateRatingString(), Connection.CreateConnection()))
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectEventStringById(), Connection.CreateConnection()))
            using (NpgsqlCommand commandRatingLocation = new NpgsqlCommand(RatingLocationString.ToString(), Connection.CreateConnection()))
            using (NpgsqlCommand commandUpdateAllRatingLocation = new NpgsqlCommand(UpdateAllLocationRating.ToString(),Connection.CreateConnection()))
            {

                await Connection.CreateConnection().OpenAsync();

                #region Update rating for event

                commandGetRating.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlTypes.NpgsqlDbType.Uuid, eventId);

                DbDataReader drGetEventToRateInfo = await commandGetRating.ExecuteReaderAsync();

                if (drGetEventToRateInfo.Read())
                {
                    CurrentRating = Convert.ToInt32(drGetEventToRateInfo[0]);///// promjeni query helper da dohvatis lat i long
                    CurrentRateCount = Convert.ToInt32(drGetEventToRateInfo[1]);
                    LatToUpdate = Convert.ToDecimal(drGetEventToRateInfo[2]);
                    LongToUpdate = Convert.ToDecimal(drGetEventToRateInfo[3]);
                }

                int NewRateCount = CurrentRateCount + 1;
                decimal NewRating = (CurrentRateCount * CurrentRating + rating) / Convert.ToDecimal(NewRateCount);


                commandUpdateRating.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlTypes.NpgsqlDbType.Uuid, eventId);
                commandUpdateRating.Parameters.AddWithValue(QueryHelper.ParRating, NpgsqlTypes.NpgsqlDbType.Double, NewRating);
                commandUpdateRating.Parameters.AddWithValue(QueryHelper.ParRateCount, NpgsqlTypes.NpgsqlDbType.Integer, NewRateCount);

                Connection.CreateConnection().Close();
                await Connection.CreateConnection().OpenAsync();
                await commandUpdateRating.ExecuteNonQueryAsync();
                #endregion

                Connection.CreateConnection().Close();
                await Connection.CreateConnection().OpenAsync();

                #region Update Location Rating

                commandRatingLocation.Parameters.AddWithValue("@Lat", NpgsqlTypes.NpgsqlDbType.Double, LatToUpdate);
                commandRatingLocation.Parameters.AddWithValue("@Long", NpgsqlTypes.NpgsqlDbType.Double, LongToUpdate);

                DbDataReader drUpdateLocationRating = await commandRatingLocation.ExecuteReaderAsync();
                decimal SumOfCount = 0;
                decimal SumOfMultiplication = 0;
                decimal RatingLocation = 0;

                while (drUpdateLocationRating.Read())
                {
                    SumOfMultiplication += Convert.ToDecimal(drUpdateLocationRating[0]) * Convert.ToDecimal(drUpdateLocationRating[1]);
                    SumOfCount += Convert.ToDecimal(drUpdateLocationRating[1]);

                }
                if (SumOfCount > 0)
                {
                    RatingLocation = SumOfMultiplication / SumOfCount;
                }
                #endregion

                Connection.CreateConnection().Close();
                await Connection.CreateConnection().OpenAsync();

                #region Update all events

                commandUpdateAllRatingLocation.Parameters.AddWithValue("@RatingLocation", NpgsqlTypes.NpgsqlDbType.Double,RatingLocation);
                commandUpdateAllRatingLocation.Parameters.AddWithValue("@Lat", NpgsqlTypes.NpgsqlDbType.Double, LatToUpdate);
                commandUpdateAllRatingLocation.Parameters.AddWithValue("@Long", NpgsqlTypes.NpgsqlDbType.Double, LongToUpdate);

                 await commandUpdateAllRatingLocation.ExecuteNonQueryAsync();

                #endregion


                Connection.CreateConnection().Close();
                await Connection.CreateConnection().OpenAsync();


                #region return updated event
                commandSelect.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlTypes.NpgsqlDbType.Uuid, eventId);
                DbDataReader drSelect = await commandSelect.ExecuteReaderAsync();
                while (drSelect.Read())
                {
                    evtR = new EventEntity
                    {
                        Id = new Guid(drSelect[0].ToString()),
                        StartTime = Convert.ToDateTime(drSelect[1]),
                        EndTime = Convert.ToDateTime(drSelect[2]),
                        Latitude = Convert.ToDecimal(drSelect[3]),
                        Longitude = Convert.ToDecimal(drSelect[4]),
                        Name = drSelect[5].ToString(),
                        Description = drSelect[6].ToString(),
                        Category = Convert.ToInt32(drSelect[7]),
                        Price = Convert.ToDecimal(drSelect[8]),
                        Capacity = Convert.ToInt32(drSelect[9]),
                        Reserved = Convert.ToInt32(drSelect[10]),
                        Rating = Convert.ToDecimal(drSelect[11]),
                        RateCount = Convert.ToInt32(drSelect[12]),
                        RatingLocation=Convert.ToDecimal(drSelect[14])
                    };
                }
            }
     
            return Mapper.Map<IEvent>(evtR);
            #endregion
        }

        /// <summary>
        /// Updates the reservation asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<IEvent> UpdateReservationAsync(Guid eventId)
        {

            int parReserved = 0;
            EventEntity evtR = new EventEntity(); 

            using (Connection.CreateConnection())
            using (NpgsqlCommand commandGetReserved = new NpgsqlCommand (QueryHelper.GetSelectCurrentReservationString(), Connection.CreateConnection()))
            using (NpgsqlCommand commandUpdate = new NpgsqlCommand( QueryHelper.GetInsertUpdateReservationString(), Connection.CreateConnection()))
            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectEventStringById(), Connection.CreateConnection()))
            {
                commandGetReserved.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlTypes.NpgsqlDbType.Uuid, eventId);

                await Connection.CreateConnection().OpenAsync();
                object reservedObj = await commandGetReserved.ExecuteScalarAsync();
                parReserved=Convert.ToInt32(reservedObj);
                parReserved++;

                commandUpdate.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlTypes.NpgsqlDbType.Uuid, eventId);
                commandUpdate.Parameters.AddWithValue(QueryHelper.ParReserved, NpgsqlTypes.NpgsqlDbType.Integer, parReserved);

                await commandUpdate.ExecuteNonQueryAsync();

                commandSelect.Parameters.AddWithValue(QueryHelper.ParEventId, NpgsqlTypes.NpgsqlDbType.Uuid, eventId);
                DbDataReader dr = await commandSelect.ExecuteReaderAsync();
                while (dr.Read())
                {
                    evtR = new EventEntity
                    {
                        Id = new Guid(dr[0].ToString()),
                        StartTime = Convert.ToDateTime(dr[1]),
                        EndTime = Convert.ToDateTime(dr[2]),
                        Latitude = Convert.ToDecimal(dr[3]),
                        Longitude = Convert.ToDecimal(dr[4]),
                        Name = dr[5].ToString(),
                        Description = dr[6].ToString(),
                        Category = Convert.ToInt32(dr[7]),
                        Price = Convert.ToDecimal(dr[8]),
                        Capacity = Convert.ToInt32(dr[9]),
                        Reserved = Convert.ToInt32(dr[10]),
                        Rating = Convert.ToDecimal(dr[11]),
                        RateCount = Convert.ToInt32(dr[12]),
                        RatingLocation=Convert.ToDecimal(dr[14])
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

            using (NpgsqlCommand commandSelect = new NpgsqlCommand(QueryHelper.GetSelectEventStringById(), Connection.CreateConnection()))
            {
                await Connection.CreateConnection().OpenAsync();

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
                        Latitude = Convert.ToDecimal(dr[4]),
                        Longitude = Convert.ToDecimal(dr[5]),
                        StartTime = Convert.ToDateTime(dr[6]),
                        EndTime = Convert.ToDateTime(dr[7]),
                        Rating = Convert.ToDecimal(dr[8]),
                        RateCount = Convert.ToInt32(dr[9]),
                        Price = Convert.ToDecimal(dr[10]),
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