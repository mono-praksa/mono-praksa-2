using Npgsql;

namespace GeoEvents.Common
{
    public interface IPostgresConnection
    {
        /// <summary>
        /// Creates a postgresql connection.
        /// </summary>
        NpgsqlConnection CreateConnection();
    }
}