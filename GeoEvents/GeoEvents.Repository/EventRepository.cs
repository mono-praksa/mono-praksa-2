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
        protected IPostgresConnection PostgresConn { get; private set; }

        protected IMapper Mapper { get; private set; }

        public EventRepository(IPostgresConnection connection, IMapper mapper)
        {
            this.Mapper = mapper;
            this.PostgresConn = connection;
        }

        public async Task<IEvent> CreateEventAsync(IEvent evt)
        {
            EventEntity evte = Mapper.Map<EventEntity>(evt);
            evte = null;

            try

            {
                using (PostgresConn.NpgConn())
                using (NpgsqlCommand commandInsert = new NpgsqlCommand(QueryHelper.GetInsertStringEvent(), PostgresConn.NpgConn()))
                using (NpgsqlCommand commandSelect = new NpgsqlCommand("Select * from \"Events\" where \"Events\".\"Id\" = @evteId Limit (1)", PostgresConn.NpgConn()))

                {
                    commandInsert.Parameters.AddWithValue("@Id", NpgsqlTypes.NpgsqlDbType.Uuid, evte.Id);
                    commandInsert.Parameters.AddWithValue("@Category", NpgsqlTypes.NpgsqlDbType.Integer, evte.Category);
                    commandInsert.Parameters.AddWithValue("@Description", NpgsqlTypes.NpgsqlDbType.Text, evte.Description);
                    commandInsert.Parameters.AddWithValue("@StartTime", NpgsqlTypes.NpgsqlDbType.Timestamp, evte.StartTime);
                    commandInsert.Parameters.AddWithValue("@EndTime", NpgsqlTypes.NpgsqlDbType.Timestamp, evte.EndTime);
                    commandInsert.Parameters.AddWithValue("@Lat", NpgsqlTypes.NpgsqlDbType.Double, evte.Lat);
                    commandInsert.Parameters.AddWithValue("@Long", NpgsqlTypes.NpgsqlDbType.Double, evte.Long);
                    commandInsert.Parameters.AddWithValue("@Name", NpgsqlTypes.NpgsqlDbType.Text, evte.Name);

                    commandSelect.Parameters.AddWithValue("@EvteId", NpgsqlTypes.NpgsqlDbType.Uuid, evte.Id);

                    await PostgresConn.NpgConn().OpenAsync();
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
                            Category = Convert.ToInt32(dr[7])
                        };
                    }


                };
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }

            return Mapper.Map<IEvent>(evte);


        }


        public async Task<IEnumerable<IEvent>> GetEventsAsync(IFilter filter)
        {
            EventEntity tmp;
            List<IEvent> SelectEvents = new List<IEvent>();
            try
            {
                using (PostgresConn.NpgConn())
                using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetSelectStringEvent(filter), PostgresConn.NpgConn()))
                {
                    command.Parameters.AddWithValue("@Lat", NpgsqlTypes.NpgsqlDbType.Double, filter.ULat);
                    command.Parameters.AddWithValue("@Long", NpgsqlTypes.NpgsqlDbType.Double, filter.ULong);
                    command.Parameters.AddWithValue("@Radius", NpgsqlTypes.NpgsqlDbType.Double, filter.Radius * 1000);
                    command.Parameters.AddWithValue("@UserStartTime", NpgsqlTypes.NpgsqlDbType.Timestamp, filter.StartTime);
                    command.Parameters.AddWithValue("@UserEndTime", NpgsqlTypes.NpgsqlDbType.Timestamp, filter.EndTime);
                    command.Parameters.AddWithValue("@Category", NpgsqlTypes.NpgsqlDbType.Integer, filter.Category);


                    await PostgresConn.NpgConn().OpenAsync();
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
                            Category = Convert.ToInt32(dr[7])
                        };

                        SelectEvents.Add(Mapper.Map<IEvent>(tmp));
                    }


                }
            }
            catch (NpgsqlException ex)
            {
                throw ex;
            }
            return SelectEvents;

        }

    }
}