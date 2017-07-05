using GeoEvents.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Common;
using GeoEvents.DAL;
using Npgsql;
using System.Data;

namespace GeoEvents.Repository
{
    public class EventRepository : IEventRepository
    {
        PostgresConnection PostgresConn;

        public EventRepository()
        {
            PostgresConn = new PostgresConnection();
        }

        public bool CreateEvent(IEventEntity evt)
        {
            PostgresConn.OpenConnection();
            bool Flag = false;


            NpgsqlCommand command = PostgresConn.NpgComm();
            command = new NpgsqlCommand
                ("insert into \"Events\" values(@Id, @StartTime, @EndTime, @Lat, @Long, @Name, @Description, @Category)",
                PostgresConn.NpgConn());

            command.Parameters.AddWithValue("@Id", evt.Id);
            command.Parameters.AddWithValue("@Category", evt.Category);
            command.Parameters.AddWithValue("@Description", evt.Description);
            command.Parameters.AddWithValue("@StartTime", evt.StartTime);
            command.Parameters.AddWithValue("@EndTime", evt.EndTime);
            command.Parameters.AddWithValue("@Lat", evt.Lat);
            command.Parameters.AddWithValue("@Long", evt.Long);
            command.Parameters.AddWithValue("@Name", evt.Name);

            if (command.ExecuteNonQuery() == 1)
            {
                Flag = true;
            }

            PostgresConn.CloseConnection();

            return Flag;
        }



        public List<IEventEntity> GetEvents(Filter filter)
        {
            PostgresConn.OpenConnection();

            NpgsqlCommand command = new NpgsqlCommand("SELECT * FROM \"Events\" WHERE ", PostgresConn.NpgConn());

            NpgsqlDataReader dr = command.ExecuteReader();

            EventEntity tmp;
            List<IEventEntity> SelectEvents = new List<IEventEntity>();

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

                SelectEvents.Add(tmp);
            }

            return SelectEvents;
        }


    }
}