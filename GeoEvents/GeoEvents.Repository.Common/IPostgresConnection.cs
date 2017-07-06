using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository.Common
{
    public interface IPostgresConnection
    {

        void OpenConnection();

        void CloseConnection();

        Npgsql.NpgsqlCommand NpgComm();

        Npgsql.NpgsqlConnection NpgConn();
    }
}
