using GeoEvents.Common;
using GeoEvents.DAL;
using GeoEvents.Repository.Common;
using log4net.Appender;
using log4net.Core;
using Npgsql;


namespace GeoEvents.Repository
{ 
    
    public class PostgresAppender : AppenderSkeleton
    {

        /// <summary>
        /// Subclasses of <see cref="T:log4net.Appender.AppenderSkeleton" /> should implement this method
        /// to perform actual logging.
        /// </summary>
        /// <param name="loggingEvent">The event to append.</param>
        /// <remarks>
        /// <para>
        /// A subclass must implement this method to perform
        /// logging of the <paramref name="loggingEvent" />.
        /// </para>
        /// <para>This method will be called by <see cref="M:DoAppend(LoggingEvent)" />
        /// if all the conditions listed for that method are met.
        /// </para>
        /// <para>
        /// To restrict the logging of events in the appender
        /// override the <see cref="M:PreAppendCheck()" /> method.
        /// </para>
        /// </remarks>
        protected override void Append(LoggingEvent loggingEvent)
        {
            using (NpgsqlConnection conn = new NpgsqlConnection("Server=192.168.21.10;Port=5432;Database=locoDB;User Id=postgres;Password=postgres;"))
            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(QueryHelper.GetInsertLoggerQueryString(), conn))
                {
                    var appName = command.CreateParameter();
                    appName.Direction = System.Data.ParameterDirection.Input;
                    appName.DbType = System.Data.DbType.String;
                    appName.ParameterName = "@app_name";
                    appName.Value = loggingEvent.LookupProperty("AppName");
                    command.Parameters.Add(appName);

                    var thread = command.CreateParameter();
                    thread.Direction = System.Data.ParameterDirection.Input;
                    thread.DbType = System.Data.DbType.String;
                    thread.ParameterName = "@thread";
                    thread.Value = loggingEvent.ThreadName;
                    command.Parameters.Add(thread);

                    var level = command.CreateParameter();
                    level.Direction = System.Data.ParameterDirection.Input;
                    level.DbType = System.Data.DbType.String;
                    level.ParameterName = "@level";
                    level.Value = loggingEvent.Level;
                    command.Parameters.Add(level);

                    var location = command.CreateParameter();
                    location.Direction = System.Data.ParameterDirection.Input;
                    location.DbType = System.Data.DbType.String;
                    location.ParameterName = "@location";
                    location.Value = loggingEvent.LocationInformation.FullInfo;
                    command.Parameters.Add(location);

                    var message = command.CreateParameter();
                    message.Direction = System.Data.ParameterDirection.Input;
                    message.DbType = System.Data.DbType.String;
                    message.ParameterName = "@message";
                    message.Value = loggingEvent.RenderedMessage;
                    command.Parameters.Add(message);

                    var log_date = command.CreateParameter();
                    log_date.Direction = System.Data.ParameterDirection.Input;
                    log_date.DbType = System.Data.DbType.DateTime2;
                    log_date.ParameterName = "@log_date";
                    log_date.Value = loggingEvent.TimeStamp;
                    command.Parameters.Add(log_date);

                    var exception = command.CreateParameter();
                    exception.Direction = System.Data.ParameterDirection.Input;
                    exception.DbType = System.Data.DbType.String;
                    exception.ParameterName = "@exception";
                    exception.Value = loggingEvent.GetExceptionString();
                    command.Parameters.Add(exception);

                    command.ExecuteNonQuery();

                }

            }
        }



    }

}