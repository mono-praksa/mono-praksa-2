using Npgsql;

namespace GeoEvents.Common
{
    public interface IPostgresConnection
    {
        /// <summary>
        /// Creates the command.
        /// </summary>
        /// <returns>NpgsqlCommand</returns>
        NpgsqlCommand CreateCommand();

        /// <summary>
        /// Creates the connection.
        /// </summary>
        /// <returns>NpgsqlConnection</returns>
        NpgsqlConnection CreateConnection();
    }
}