using Npgsql;

namespace GeoEvents.Common
{
    public interface IPostgresConnection
    {
        NpgsqlCommand NpgComm();

        NpgsqlConnection NpgConn();
    }
}