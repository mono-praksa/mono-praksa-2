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
        

        NpgsqlCommand NpgComm();

        NpgsqlConnection NpgConn();
    }
}
