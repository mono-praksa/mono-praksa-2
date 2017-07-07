using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.DAL;
using Npgsql;


namespace GeoEvents.Repository
{
    public class BulkInsert
    {

        PostgresConnection PostgresConn;

        public BulkInsert()
        {
            PostgresConn = new PostgresConnection();
        }

        public void BulckInsertData()
           {
            //    PostgresConn.NpgComm().Parameters.AddWithValue("@Id", Id);
            //    PostgresConn.NpgComm().Parameters.AddWithValue("@Category", NpgsqlTypes.NpgsqlDbType.Integer, Category);
            //    PostgresConn.NpgComm().Parameters.AddWithValue("@Description", NpgsqlTypes.NpgsqlDbType.Text, Description);
            //    PostgresConn.NpgComm().Parameters.AddWithValue("@StartTime", NpgsqlTypes.NpgsqlDbType.Timestamp, StartTime);
            //    PostgresConn.NpgComm().Parameters.AddWithValue("@EndTime", NpgsqlTypes.NpgsqlDbType.Timestamp, EndTime);
            //    PostgresConn.NpgComm().Parameters.AddWithValue("@Lat", NpgsqlTypes.NpgsqlDbType.Double, Lat);
            //    PostgresConn.NpgComm().Parameters.AddWithValue("@Long", NpgsqlTypes.NpgsqlDbType.Double, Long);
            //    PostgresConn.NpgComm().Parameters.AddWithValue("@Name", NpgsqlTypes.NpgsqlDbType.Text, Name);

           

            using (var writer = PostgresConn.NpgConn().BeginBinaryImport
                (
                "COPY \"Events\"  FROM STDIN (FORMAT BINARY)")
                )
            {
                for (int i=0;i<1;i++) {
                    writer.StartRow();
                    writer.Write(Guid.NewGuid(), NpgsqlTypes.NpgsqlDbType.Uuid);
                    writer.Write(new DateTime(2017,7,4), NpgsqlTypes.NpgsqlDbType.Timestamp);
                    writer.Write( 2^(i%8), NpgsqlTypes.NpgsqlDbType.Integer);
                }



            }
        }



    }
}
