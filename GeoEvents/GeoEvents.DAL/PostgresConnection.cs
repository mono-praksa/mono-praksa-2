using GeoEvents.Common;
using Npgsql;

namespace GeoEvents.DAL
{
    public class PostgresConnection : IPostgresConnection
    {
        #region Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        protected IGeoEventsConfiguration configuration { get; private set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgresConnection"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public PostgresConnection(IGeoEventsConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #endregion Constructor

        #region Methods


        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>
        /// NpgsqlConnection
        /// </returns>
        public NpgsqlConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(configuration.ConnectionString);

            return connection;
        }

        #endregion Methods
    }
}