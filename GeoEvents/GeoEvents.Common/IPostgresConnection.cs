using Npgsql;

namespace GeoEvents.Common
{
    public interface IPostgresConnection
    {
        NpgsqlConnection CreateConnection();
    }
}