using GeoEvents.Common;
using System;
using System.Text;

namespace GeoEvents.Repository
{
    static public class QueryConstants
    {
        #region Constants

        /// <summary>
        /// Quoted Constant string
        /// </summary>
        public static string TableNameEventQ = "events";
        public static string TableNameLogsQ = "logs";
        public static string TableNameLocationQ = "locations";
        public static string TableNameImagesQ = "images";

        public static string NameQ = "name";
        public static string LatQ = "latitude";
        public static string LongQ = "longitude";
        public static string StartTimeQ = "start_time";
        public static string EndTimeQ = "end_time";
        public static string CategoryQ = "category";
        public static string IdQ = "id";
        public static string EventIdQ = "event_id";
        public static string DescriptionQ = "description";

        public static string PriceQ = "price";
        public static string CapacityQ = "capacity";
        public static string ReservedQ = "reserved";
        public static string RatingQ = "rating";
        public static string RateCountQ = "rate_count";
        public static string RatingLocationQ = "rating_location";
        public static string CustomQ = "custom";
        public static string AddressQ = "address";
        public static string LocationIdQ = "location_id";

        public static string OccurrenceQ = "occurrence";
        public static string RepeatEveryQ = "repeat_every";
        public static string RepeatOnQ = "repeat_on";
        public static string RepeatCountQ = "repeat_count";
        public static string DistanceQ = "distance";

        /// <summary>
        /// Parametar Constant strings
        /// </summary>
        public static string ParLatitude = "@Latitude";
        public static string ParLongitude = "@Longitude";
        public static string ParName = "@Name";
        public static string ParEndTime = "@EndTime";
        public static string ParStartTime = "@StartTime";
        public static string ParUserEndTime = "@UserEndTime";
        public static string ParUserStartTime = "@UserStartTime";
        public static string ParId = "@Id";
        public static string ParCategory = "@Category";
        public static string ParDescription = "@Description";
        public static string ParRadius = "@Radius";
        public static string ParEventId = "@EventId";
        public static string ParContent = "@Content";
        public static string ParSearchString = "@SearchString";

        public static string ParPrice = "@Price";
        public static string ParCapacity = "@Capacity";
        public static string ParReserved = "@Reserved";
        public static string ParRating = "@Rating";
        public static string ParRateCount = "@RateCount";
        public static string ParRatingLocation = "@RatingLocation";
        public static string ParLocationId = "@LocationId";
        public static string ParCustom = "@Custom";
        public static string ParAddress = "@Address";

        public static string ParAppName = "@AppName";
        public static string ParThread = "@Thread";
        public static string ParLevel = "@Level";
        public static string ParLocation = "@Location";
        public static string ParMessage = "@Message";
        public static string ParLogDate = "@LogDate";
        public static string ParException = "@Exception";

        public static string ParOccurrence = "@Occurrence";
        public static string ParRepeatEvery = "@RepeatEvery";
        public static string ParRepeatOn = "@RepeatOn";
        public static string ParRepeatCount = "@RepeatCount";

        /// <summary>
        /// Added Qouted strings
        /// </summary>
        public static string TableNameEventNameQ = string.Format("{0}.{1}", TableNameEventQ, NameQ);
        public static string TableNameEventDescriptionQ = string.Format("{0}.{1}", TableNameEventQ, DescriptionQ);
        public static string TableNameEventStartTimeQ = string.Format("{0}.{1}", TableNameEventQ, StartTimeQ);
        public static string TableNameEventEndTimeQ = string.Format("{0}.{1}", TableNameEventQ, EndTimeQ);
        public static string TableNameEventLongQ = string.Format("{0}.{1}", TableNameEventQ, LongQ);
        public static string TableNameEventLatQ = string.Format("{0}.{1}", TableNameEventQ, LatQ);
        public static string TableNameEventCatQ = string.Format("{0}.{1}", TableNameEventQ, CategoryQ);
        public static string TableNameEventIdQ = string.Format("{0}.{1}", TableNameEventQ, IdQ);
        public static string TableNameEventPriceQ = string.Format("{0}.{1}", TableNameEventQ, PriceQ);
        public static string TableNameEventRatingQ = string.Format("{0}.{1}", TableNameEventQ, RatingQ);
        public static string TableNameEventRateCountQ = string.Format("{0}.{1}", TableNameEventQ, RateCountQ);
        public static string TableNameEventLocationIdQ = string.Format("{0}.{1}", TableNameEventQ, LocationIdQ);
        public static string TableNameEventCustomQ = string.Format("{0}.{1}", TableNameEventQ, CustomQ);

        public static string TableNameEventOccurrenceQ = string.Format("{0}.{1}", TableNameEventQ, OccurrenceQ);
        public static string TableNameEventRepeatEveryQ = string.Format("{0}.{1}", TableNameEventQ, RepeatEveryQ);
        public static string TableNameEventRepeatOnQ = string.Format("{0}.{1}", TableNameEventQ, RepeatOnQ);
        public static string TableNameEventRepeatCountQ = string.Format("{0}.{1}", TableNameEventQ, RepeatCountQ);

        public static string TableNameLocationIdQ = string.Format("{0}.{1}", TableNameLocationQ, IdQ);
        public static string TableNameLocationRatingQ = string.Format("{0}.{1}", TableNameLocationQ, RatingQ);
        #endregion Constants

    }
}