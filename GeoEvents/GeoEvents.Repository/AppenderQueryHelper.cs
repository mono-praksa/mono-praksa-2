using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository
{
    class AppenderQueryHelper
    {

        /// <summary>
        /// Get Insert Logger Query string
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public static string GetInsertLoggerQueryString()
        {

            return string.Format("INSERT INTO {0}(app_name,thread,level,location,message,exception,log_date) VALUES({1},{2},{3},{4},{5},{6},{7})",
                QueryConstants.TableNameLogsQ, QueryConstants.ParAppName,
                QueryConstants.ParThread, QueryConstants.ParLevel,
                QueryConstants.ParLocation, QueryConstants.ParMessage,
                QueryConstants.ParException, QueryConstants.ParLogDate);
        }

    }
}
