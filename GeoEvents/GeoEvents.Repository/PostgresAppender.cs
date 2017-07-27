using GeoEvents.Common;
using log4net.Appender;
using log4net.Core;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GeoEvents.Repository
{

    public class PostgresAppender : AppenderSkeleton
    {
        private string ConnectionString;

        public void SetConnectionString(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))//ConnectionString))


            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetInsertLoggerQueryString(), conn))
                {

                    //command.Parameters.AddWithValue("@AppName", NpgsqlDbType.Text, loggingEvent.LookupProperty("AppName"));
                    //command.Parameters.AddWithValue("@Thread", NpgsqlDbType.Text, loggingEvent.ThreadName);
                    //command.Parameters.AddWithValue("@level", NpgsqlDbType.Text, loggingEvent.Level);
                    //command.Parameters.AddWithValue("@location", NpgsqlDbType.Text, loggingEvent.LocationInformation.FullInfo);
                    //command.Parameters.AddWithValue("@Message", NpgsqlDbType.Text, loggingEvent.RenderedMessage);
                    //command.Parameters.AddWithValue("@LogDate", NpgsqlDbType.Timestamp, loggingEvent.TimeStamp);
                    //command.Parameters.AddWithValue("@Exception", NpgsqlDbType.Text, loggingEvent.GetExceptionString());


                    var appName = command.CreateParameter();
                    appName.Direction = System.Data.ParameterDirection.Input;
                    appName.DbType = System.Data.DbType.String;
                    appName.ParameterName = "@app_name";
                    appName.Value = loggingEvent.LookupProperty("AppName");
                    command.Parameters.Add(appName);

                    var thread = command.CreateParameter();
                    thread.Direction = System.Data.ParameterDirection.Input;
                    thread.DbType = System.Data.DbType.String;
                    thread.ParameterName = ":thread";
                    thread.Value = loggingEvent.ThreadName;
                    command.Parameters.Add(thread);

                    var level = command.CreateParameter();
                    level.Direction = System.Data.ParameterDirection.Input;
                    level.DbType = System.Data.DbType.String;
                    level.ParameterName = ":level";
                    level.Value = loggingEvent.Level;
                    command.Parameters.Add(level);

                    var location = command.CreateParameter();
                    location.Direction = System.Data.ParameterDirection.Input;
                    location.DbType = System.Data.DbType.String;
                    location.ParameterName = ":location";
                    location.Value = loggingEvent.LocationInformation.FullInfo;
                    command.Parameters.Add(location);

                    var message = command.CreateParameter();
                    message.Direction = System.Data.ParameterDirection.Input;
                    message.DbType = System.Data.DbType.String;
                    message.ParameterName = ":message";
                    message.Value = loggingEvent.RenderedMessage;
                    command.Parameters.Add(message);

                    var log_date = command.CreateParameter();
                    log_date.Direction = System.Data.ParameterDirection.Input;
                    log_date.DbType = System.Data.DbType.DateTime2;
                    log_date.ParameterName = ":log_date";
                    log_date.Value = loggingEvent.TimeStamp;
                    command.Parameters.Add(log_date);

                    var exception = command.CreateParameter();
                    exception.Direction = System.Data.ParameterDirection.Input;
                    exception.DbType = System.Data.DbType.String;
                    exception.ParameterName = ":exception";
                    exception.Value = loggingEvent.GetExceptionString();
                    command.Parameters.Add(exception);

                    command.ExecuteNonQuery();

                }
                //conn.Close();
            }
        }
    }

}