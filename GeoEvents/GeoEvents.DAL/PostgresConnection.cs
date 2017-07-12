using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using GeoEvents.Common;

namespace GeoEvents.DAL
{
    public class PostgresConnection : IPostgresConnection
    {
        
        protected IGeoEventsConfiguration configuration { get; private set; }
        public NpgsqlConnection connection { get; set; }

        public PostgresConnection(IGeoEventsConfiguration configuration)
        {
            this.configuration = configuration;
            
            connection = new NpgsqlConnection(configuration.ConnectionString);

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
