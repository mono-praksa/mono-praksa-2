using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Repository.Common;

namespace GeoEvents.DAL
{
    public class PostgresConnection : IPostgresConnection

    {
        const string ConnStringDefault = "Server=localhost;Port=5432;Database=Proba;User Id=postgres;Password=postgres;";
        NpgsqlConnection connection;

        public PostgresConnection()
        {           
                connection = new NpgsqlConnection(ConnStringDefault);        
        }

        public void OpenConnection()
        {
            try
            {
                connection.Open();
            }
            catch (PostgresException msg)
            {
                throw msg;
            }


        }

        public void CloseConnection()
        {
            try
            {
                connection.Close();
            }
            catch (PostgresException msg)
            {
                throw msg;
            }

        }


        public NpgsqlCommand NpgComm()
        {
            return connection.CreateCommand();

        }

        public NpgsqlConnection NpgConn()
        {

            return connection;
        }


    }
}
