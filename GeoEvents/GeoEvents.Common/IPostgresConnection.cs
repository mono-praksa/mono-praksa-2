using Npgsql;

namespace GeoEvents.Common
{
    public interface IPostgresConnection
    {
        NpgsqlCommand CreateCommand();

        NpgsqlConnection CreateConnection();
    }
}