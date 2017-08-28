using GeoEvents.Common;
using System.Configuration;

namespace GeoEvents.WebAPI.DbConnection
{
    public class GeoEventsConfiguration : IGeoEventsConfiguration
    {
        public string ConnectionString { get; set; }

        public GeoEventsConfiguration()
        {
            ConnectionString = System.Configuration.ConfigurationManager.AppSettings["Constring"];
        }
    }

}