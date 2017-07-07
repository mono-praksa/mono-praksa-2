using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace GeoEvents.DAL
{
    public class PostgresConnection
    {

        string ConnStringDefault;
        NpgsqlConnection connection;


        public PostgresConnection()
        {

            this.ConnStringDefault = ConfigurationSettings.AppSettings["Constring"];
            connection = new NpgsqlConnection(ConnStringDefault);
        }

        public void OpenConnection()
        {      
                connection.Open();       
        }

        public void CloseConnection()
        {                     
                connection.Close();           
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
