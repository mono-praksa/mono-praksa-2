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

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgresConnection"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public PostgresConnection(IGeoEventsConfiguration configuration)
        {
            this.configuration = configuration;
            connection = new NpgsqlConnection(configuration.ConnectionString);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>
        /// NpgsqlCommand
        /// </returns>
        public NpgsqlCommand CreateCommand()
        {
            return connection.CreateCommand();
        }

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>
        /// NpgsqlConnection
        /// </returns>
        public NpgsqlConnection CreateConnection()
        {
            return connection;
        }

        #endregion Methods
    }
}