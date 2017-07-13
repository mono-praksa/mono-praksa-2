using GeoEvents.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace GeoEvents.WebAPI.DbConnection
{
    public class GeoEventsConfiguration : IGeoEventsConfiguration
    {

        public string ConnectionString { get; set; }
        public GeoEventsConfiguration()
        {
            ConnectionString = ConfigurationSettings.AppSettings["Constring"];

        }

    }
}