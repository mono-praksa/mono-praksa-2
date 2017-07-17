using GeoEvents.Common;
using Npgsql;

namespace GeoEvents.DAL
{
    public class PostgresConnection : IPostgresConnection
    {
        #region Properties

        protected IGeoEventsConfiguration configuration { get; private set; }
        protected NpgsqlConnection connection { get; private set; }

        #endregion Properties

        #region Constructor

        public PostgresConnection(IGeoEventsConfiguration configuration)
        {
            this.configuration = configuration;
            connection = new NpgsqlConnection(configuration.ConnectionString);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// return Npgcommand
        /// </summary>
        /// <returns>NpgsqlConnection connection.CreateCommand() </returns>
        public NpgsqlCommand NpgComm()
        {
            return connection.CreateCommand();
        }

        /// <summary>
        /// returns Npqconnection
        /// </summary>
        /// <returns>NpgsqlConnection connection </returns>
        public NpgsqlConnection NpgConn()
        {
            return connection;
        }

        #endregion Methods
    }
}