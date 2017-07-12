using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Common
{
    public interface IPostgresConnection
    {
        NpgsqlConnection connection { get; set; }

        NpgsqlCommand NpgComm();

        NpgsqlConnection NpgConn();
    }
}
