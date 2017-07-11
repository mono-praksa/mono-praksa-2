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

namespace GeoEvents.Repository
{
    public class EventRepository 
    {
        protected IPostgresConnection PostgresConn { get; private set; }


        public EventRepository(IPostgresConnection connection)
        {
            this.PostgresConn = connection;
        }

        public bool CreateEvent(IEvent evt)
        {
            ////////////////// mapirat iz IEvent u IeventEntity i onda insert radit ??? 

            PostgresConn.OpenConnection();
            bool Flag = false;


            NpgsqlCommand command = PostgresConn.NpgComm();
            command = new NpgsqlCommand
                (ConstRepository.GetInsertStringEvent(),
                PostgresConn.NpgConn());

            command.Parameters.AddWithValue("@Id", NpgsqlTypes.NpgsqlDbType.Uuid, evt.Id);
            command.Parameters.AddWithValue("@Category", NpgsqlTypes.NpgsqlDbType.Integer, evt.Category);
            command.Parameters.AddWithValue("@Description", NpgsqlTypes.NpgsqlDbType.Text, evt.Description);
            command.Parameters.AddWithValue("@StartTime", NpgsqlTypes.NpgsqlDbType.Timestamp, evt.StartTime);
            command.Parameters.AddWithValue("@EndTime", NpgsqlTypes.NpgsqlDbType.Timestamp, evt.EndTime);
            command.Parameters.AddWithValue("@Lat", NpgsqlTypes.NpgsqlDbType.Double, evt.Lat);
            command.Parameters.AddWithValue("@Long", NpgsqlTypes.NpgsqlDbType.Double, evt.Long);
            command.Parameters.AddWithValue("@Name", NpgsqlTypes.NpgsqlDbType.Text, evt.Name);

            if (command.ExecuteNonQuery() == 1)
            {
                Flag = true;
            }

            PostgresConn.CloseConnection();

            return Flag;
        }



        public List<IEvent> GetEvents(Filter filter)
        {

            PostgresConn.OpenConnection();

            NpgsqlCommand command = new NpgsqlCommand(ConstRepository.GetSelectStringEvent(filter),
                PostgresConn.NpgConn());

            command.Parameters.AddWithValue("@Lat", NpgsqlTypes.NpgsqlDbType.Double, filter.ULat);
            command.Parameters.AddWithValue("@Long", NpgsqlTypes.NpgsqlDbType.Double, filter.ULong);
            command.Parameters.AddWithValue("@Radius", NpgsqlTypes.NpgsqlDbType.Double, filter.Radius * 1000);
            command.Parameters.AddWithValue("@UserStartTime", NpgsqlTypes.NpgsqlDbType.Timestamp, filter.StartTime);
            command.Parameters.AddWithValue("@UserEndTime", NpgsqlTypes.NpgsqlDbType.Timestamp, filter.EndTime);
            command.Parameters.AddWithValue("@Category", NpgsqlTypes.NpgsqlDbType.Integer, filter.Category);

            NpgsqlDataReader dr = command.ExecuteReader();

            EventEntity tmp;
            List<IEvent> SelectEvents = new List<IEvent>();

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

            return SelectEvents;
        }

    }
}