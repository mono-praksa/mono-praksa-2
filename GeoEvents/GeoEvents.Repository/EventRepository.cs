using GeoEvents.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Common;
using GeoEvents.DAL;
using Npgsql;


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



        public List<IEventEntity> GetEvents(Filter filter)
        {

            PostgresConn.OpenConnection();



            NpgsqlCommand CountCommand = new NpgsqlCommand(ConstRepository.GetCountSelectString(filter),
                PostgresConn.NpgConn());

            List<IEventEntity> ToBigCountIndicator = new List<IEventEntity>();
            ToBigCountIndicator.Add(new EventEntity(new Guid(), new DateTime(), new DateTime(), 0M, 0M, "1Break", "", 0));



            CountCommand.Parameters.AddWithValue("@Lat", NpgsqlTypes.NpgsqlDbType.Double, filter.ULat);
            CountCommand.Parameters.AddWithValue("@Long", NpgsqlTypes.NpgsqlDbType.Double, filter.ULong);
            CountCommand.Parameters.AddWithValue("@Radius", NpgsqlTypes.NpgsqlDbType.Double, filter.Radius);
            CountCommand.Parameters.AddWithValue("@UserStartTime", NpgsqlTypes.NpgsqlDbType.Timestamp, filter.StartTime);
            CountCommand.Parameters.AddWithValue("@UserEndTime", NpgsqlTypes.NpgsqlDbType.Timestamp, filter.EndTime);
            CountCommand.Parameters.AddWithValue("@Category", NpgsqlTypes.NpgsqlDbType.Integer, filter.Category);
            Int64 count = (Int64)CountCommand.ExecuteScalar();

           
            ToBigCountIndicator.Add(new EventEntity(new Guid(), new DateTime(), new DateTime(), 0M, 0M, "2Break", "", 0));
        
            if (count > 11)
            {
                List<IEventEntity> ToBigCountIndicator2 = new List<IEventEntity>();
                ToBigCountIndicator2.Add(new EventEntity(new Guid(), new DateTime(), new DateTime(), 0M, 0M, count.ToString(), "", 0));
                PostgresConn.CloseConnection();
                return ToBigCountIndicator2;
            }

            ToBigCountIndicator.Add(new EventEntity(new Guid(), new DateTime(), new DateTime(), 0M, 0M, "3Break", "", 0));

            //PostgresConn.CloseConnection();
            //return ToBigCountIndicator;


            PostgresConn.CloseConnection();
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