﻿using GeoEvents.Common;
using System;
using System.Text;

namespace GeoEvents.Repository
{
    static public class QueryHelper
    {
        #region Constants

        /// <summary>
        /// Quoted Constant string
        /// </summary>
        private static string TableNameEventQ = "events";
        private static string TableNameLogsQ = "logs";
        private static string TableNameLocationQ = "locations";
        private static string TableNameImagesQ = "images";
        private static string NameQ = "name";
        private static string LatQ = "latitude";
        private static string LongQ = "longitude";
        private static string StartTimeQ = "start_time";
        private static string EndTimeQ = "end_time";
        private static string CategoryQ = "category";
        private static string IdQ = "id";
        private static string EventIdQ = "event_id";
        private static string DescriptionQ = "description";
        private static string PriceQ = "price";
        private static string CapacityQ = "capacity";
        private static string ReservedQ = "reserved";
        private static string RatingQ = "rating";
        private static string RateCountQ = "rate_count";
        private static string RatingLocationQ = "rating_location";
        private static string CustomQ = "custom";
        private static string AddressQ = "address";
        private static string LocationIdQ = "location_id";

        private static string OccurrenceQ = "occurrence";
        private static string RepeatEveryQ = "repeat_every";
        private static string RepeatOnQ = "repeat_on";
        private static string RepeatCountQ = "repeat_count";

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
        public static string ParLogDate = "LogDate";
        public static string ParException = "@Exception";
        public static string ParOccurrence = "@Occurrence";
        public static string ParRepeatEvery = "@RepeatEvery";
        public static string ParRepeatOn = "@RepeatOn";
        public static string ParRepeatCount = "@RepeatCount";

        /// <summary>
        /// Added Qouted strings
        /// </summary>
        private static string TableNameEventNameQ = String.Format("{0}.{1}", TableNameEventQ, NameQ);

        private static string TableNameEventDescriptionQ = String.Format("{0}.{1}", TableNameEventQ, DescriptionQ);
        private static string TableNameEventStartTimeQ = String.Format("{0}.{1}", TableNameEventQ, StartTimeQ);
        private static string TableNameEventEndTimeQ = String.Format("{0}.{1}", TableNameEventQ, EndTimeQ);
        private static string TableNameEventLongQ = String.Format("{0}.{1}", TableNameEventQ, LongQ);
        private static string TableNameEventLatQ = String.Format("{0}.{1}", TableNameEventQ, LatQ);
        private static string TableNameEventCatQ = String.Format("{0}.{1}", TableNameEventQ, CategoryQ);
        private static string TableNameEventIdQ = String.Format("{0}.{1}", TableNameEventQ, IdQ);
        private static string TableNameEventPriceQ = String.Format("{0}.{1}", TableNameEventQ, PriceQ);
        private static string TableNameEventRatingQ = String.Format("{0}.{1}", TableNameEventQ, RatingQ);
        private static string TableNameEventRateCountQ = String.Format("{0}.{1}", TableNameEventQ, RateCountQ);
        private static string TableNameEventLocationIdQ = String.Format("{0}.{1}", TableNameEventQ, LocationIdQ);
        private static string TableNameEventCustomQ = String.Format("{0}.{1}", TableNameEventQ, CustomQ);

        private static string TableNameEventOccurrenceQ = String.Format("{0}.{1}", TableNameEventQ, OccurrenceQ);
        private static string TableNameEventRepeatEveryQ = String.Format("{0}.{1}", TableNameEventQ, RepeatEveryQ);
        private static string TableNameEventRepeatOnQ = String.Format("{0}.{1}", TableNameEventQ, RepeatOnQ);
        private static string TableNameEventRepeatCountQ = String.Format("{0}.{1}", TableNameEventQ, RepeatCountQ);


        private static string TableNameLocationIdQ = String.Format("{0}.{1}", TableNameLocationQ, IdQ);
        private static string TableNameLocationRatingQ = String.Format("{0}.{1}", TableNameLocationQ, RatingQ);

        /// <summary>
        /// Default DateTime
        /// </summary>
        private static bool isNotFirst = false;

        #endregion Constants

        #region Methods

        public static string GetSelectAllEventsQueryString(IFilter filter)
        {
            StringBuilder selectString = new StringBuilder();

            selectString.AppendFormat("SELECT * FROM {0} ", TableNameEventQ);

            isNotFirst = false;

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {
                selectString.Append(" WHERE ");
                selectString.AppendFormat(" (earth_box(ll_to_earth({0},{1}),{2})@> ll_to_earth({3},{4})) ",
                        ParLatitude, ParLongitude, ParRadius, TableNameEventLatQ, TableNameEventLongQ);
                isNotFirst = true;
            }

            ///Adding Price filter query if there is price
            if (filter.Price != null)
            {
                selectString = ConditionValidation(selectString);
                selectString.AppendFormat("({0} <= {1}) ",
                    TableNameEventPriceQ, ParPrice);
            }

            ///Adding Rating Event filter query if there is rating event
            if (filter.RatingEvent != 0)
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat("({0} >= {1}) ",
                    TableNameEventRatingQ, ParRating);
            }

            /// Adding Time filter query if there is time in filter
            if (filter.EndTime > filter.StartTime)
            {
                selectString = ConditionValidation(selectString);

                //selectString.AppendFormat(" (({0},{1}) OVERLAPS ({2},{3})) ",
                //    ParUserStartTime, ParUserEndTime, TableNameEventStartTimeQ, TableNameEventEndTimeQ);

                selectString.AppendFormat(" (check_recurrence({0},{1},{2},{3},{4},{5},{6},{7})) ",
                    ParUserStartTime, ParUserEndTime, TableNameEventStartTimeQ, TableNameEventEndTimeQ, TableNameEventOccurrenceQ,
                    TableNameEventRepeatEveryQ, TableNameEventRepeatOnQ, TableNameEventRepeatCountQ);
            }

            ///Adding searcstring filter in queri if there is searchstring
            if (!String.IsNullOrWhiteSpace(filter.SearchString))
            {
                selectString = ConditionValidation(selectString);


                selectString.AppendFormat(" (lower({0}) LIKE lower({1})) ",
                    NameQ, ParSearchString);
            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                selectString = ConditionValidation(selectString);


                selectString.AppendFormat(" ({0} & {1} > 0)",
                    ParCategory, TableNameEventCatQ);
            }

            /// adding custom search
            if (!String.IsNullOrWhiteSpace(filter.Custom))
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat(" ({0} @> {1}) ",
                    CustomQ, ParCustom);
            }
            return selectString.ToString();
        }


        /// <summary>
        /// Gets query string with filter for count query
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Query string</returns>
        public static string GetSelectCountEventQueryString(IFilter filter)
        {
            StringBuilder selectString = new StringBuilder();

            selectString.AppendFormat("SELECT COUNT({0}) FROM {1} ",
                TableNameEventIdQ, TableNameEventQ);

            isNotFirst = false;

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {
                selectString.Append(" WHERE ");
                selectString.AppendFormat(" (earth_box(ll_to_earth({0},{1}),{2})@> ll_to_earth({3},{4})) ",
                        ParLatitude, ParLongitude, ParRadius, TableNameEventLatQ, TableNameEventLongQ);
                isNotFirst = true;
            }

            ///Adding Price filter query if there is price
            if (filter.Price != null)
            {
                selectString = ConditionValidation(selectString);
                selectString.AppendFormat("({0} <= {1}) ",
                    TableNameEventPriceQ, ParPrice);
            }

            ///Adding Rating Event filter query if there is rating event
            if (filter.RatingEvent != 0)
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat("({0} >= {1}) ",
                    TableNameEventRatingQ, ParRating);
            }

            /// Adding Time filter query if there is time in filter
            if (filter.EndTime > filter.StartTime)
            {
                selectString = ConditionValidation(selectString);

                //selectString.AppendFormat(" (({0},{1}) OVERLAPS ({2},{3})) ",
                //    ParUserStartTime, ParUserEndTime, TableNameEventStartTimeQ, TableNameEventEndTimeQ);

                selectString.AppendFormat(" (check_recurrence({0},{1},{2},{3},{4},{5},{6},{7})) ",
                    ParUserStartTime, ParUserEndTime, TableNameEventStartTimeQ, TableNameEventEndTimeQ, TableNameEventOccurrenceQ,
                    TableNameEventRepeatEveryQ, TableNameEventRepeatOnQ, TableNameEventRepeatCountQ);
            }

            ///Adding searcstring filter in queri if there is searchstring
            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat(" (lower({0}) LIKE lower({1})) ",
                    NameQ, ParSearchString);
            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat(" ({0} & {1} > 0)",
                    ParCategory, TableNameEventCatQ);
            }

            /// adding custom search
            if (!string.IsNullOrWhiteSpace(filter.Custom))
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat(" ({0} @> {1}) ",
                    CustomQ, ParCustom);
            }

            return selectString.ToString();
        }

        /// <summary>
        /// Gets query filter select string for event by Id
        /// </summary>
        /// <returns>
        /// query string
        /// </returns>
        public static string GetSelectEventByIdQueryString()
        {
            return string.Format("SELECT * FROM {0} WHERE {1}={2}", TableNameEventQ, TableNameEventIdQ, ParEventId);
        }

        /// <summary>
        /// Get Rating and reate Count
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public static string GetSelectFromEventRatingQueryString()
        {
            return string.Format("SELECT rating,ratecount FROM {0} WHERE {1}={2}", TableNameEventQ, TableNameEventIdQ, ParEventId);
        }

        /// <summary>
        /// Gets query filter select string for query events
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// query string
        /// </returns>
        public static string GetSelectEventQueryString(IFilter filter)
        {
            StringBuilder selectString = new StringBuilder();

            if (filter.OrderBy == "Distance" && filter.ULat != null && filter.ULong != null)
            {
                selectString.AppendFormat("SELECT *, earth_distance(ll_to_earth({0},{1}), ll_to_earth({2},{3})) as distance from {4} WHERE ",
                    ParLatitude, ParLongitude, TableNameEventLatQ, TableNameEventLongQ, TableNameEventQ);
            }
            else if (filter.OrderBy == "RatingLocation")
            {
                selectString.AppendFormat("SELECT * FROM {0} INNER JOIN {1} ON ({2} = {3}) ",
                    TableNameEventQ, TableNameLocationQ, TableNameEventLocationIdQ, TableNameLocationIdQ);
            }
            else
            {
                selectString.AppendFormat("SELECT * FROM {0} ",
                    TableNameEventQ);
            }

            isNotFirst = false;

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {
                selectString.Append(" WHERE ");
                selectString.AppendFormat(" (earth_box(ll_to_earth({0},{1}),{2})@> ll_to_earth({3},{4})) ",
                        ParLatitude, ParLongitude, ParRadius, TableNameEventLatQ, TableNameEventLongQ);
                isNotFirst = true;
            }

            ///Adding Price filter query if there is price
            if (filter.Price != null)
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat("({0} <= {1}) ",
                    TableNameEventPriceQ, ParPrice);
            }

            ///Adding Rating Event filter query if there is rating event
            if (filter.RatingEvent != null)
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat("({0} >= {1}) ",
                    TableNameEventRatingQ, ParRating);
            }

            /// Adding Time filter query if there is time in filter

            if (filter.EndTime > filter.StartTime)
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat(" (check_recurrence({0},{1},{2},{3},{4},{5},{6},{7})) ",
                    ParUserStartTime, ParUserEndTime, TableNameEventStartTimeQ, TableNameEventEndTimeQ, TableNameEventOccurrenceQ,
                    TableNameEventRepeatEveryQ, TableNameEventRepeatOnQ, TableNameEventRepeatCountQ);


            }

            ///Adding searcstring filter in queri if there is searchstring
            if (!string.IsNullOrWhiteSpace(filter.SearchString))
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat(" (lower({0}) LIKE lower({1})) ",
                    NameQ, ParSearchString);
            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat(" ({0} & {1} > 0)",
                    ParCategory, TableNameEventCatQ);
            }

            /// adding custom search
            if (!string.IsNullOrWhiteSpace(filter.Custom))
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat(" ({0} @> {1}) ",
                    CustomQ, ParCustom);
            }

            ///ORDERING orderby orderAscend
            ///

            switch (filter.OrderBy)
            {
                case "Name":
                    selectString.AppendFormat(" order by {0} ",
                        TableNameEventNameQ);
                    break;

                case "StartTime":
                    selectString.AppendFormat(" order by {0} ",
                        TableNameEventStartTimeQ);
                    break;

                case "EndTime":
                    selectString.AppendFormat(" order by {0} ",
                        TableNameEventEndTimeQ);
                    break;

                case "Distance":
                    selectString.Append(" order by distance ");
                    break;

                case "Price":
                    selectString.AppendFormat(" order by {0} ",
                        TableNameEventPriceQ);
                    break;

                case "RatingEvent":
                    selectString.AppendFormat(" order by {0} ",
                        TableNameEventRatingQ);
                    break;

                case "RatingLocation":
                    selectString.AppendFormat(" order by {0} ",
                        TableNameLocationRatingQ);
                    break;
            }

            if (filter.OrderAscending == true && string.IsNullOrEmpty(filter.OrderBy) == false)
            {
                selectString.Append(" asc ");
            }
            else
            {
                selectString.Append(" desc ");
            }

            selectString.AppendFormat(" LIMIT({0}) OFFSET ({1}) ",
                filter.PageSize.ToString(), ((filter.PageNumber - 1) * filter.PageSize).ToString());

            return selectString.ToString();
        }

        /// <summary>
        /// gets query insert string for event
        /// </summary>
        /// <returns>Query string</returns>
        public static string GetInsertEventQueryString()
        {
            return String.Format("INSERT INTO {0} VALUES ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19})",
                   TableNameEventQ, ParId, ParName, ParDescription, ParCategory, ParLatitude, ParLongitude,
                   ParStartTime, ParEndTime, ParRating, ParRateCount, ParPrice, ParCapacity, ParReserved,
                   ParCustom, ParOccurrence, ParRepeatEvery, ParRepeatOn, ParRepeatCount, ParLocationId);
        }

        /// <summary>
        /// Gets query select string for Images
        /// </summary>
        /// <returns>Query string</returns>
        public static string GetSelectImagesQueryString()
        {
            return string.Format(" SELECT * FROM {0} WHERE ({1}={2}.{3})",
                TableNameImagesQ, ParEventId, TableNameImagesQ, EventIdQ);
        }

        /// <summary>
        /// get query Insert string for images
        /// </summary>
        /// <returns>guery string</returns>
        public static string GetInsertImagesQueryString()
        {
            return string.Format(" INSERT INTO {0} VALUES({1},{2},{3}) ",
                TableNameImagesQ, ParId, ParContent, ParEventId);
        }

        /// <summary>
        /// get query select string form images
        /// </summary>
        /// <returns></returns>
        public static string GetSelectImageQueryString()
        {
            return string.Format(" SELECT * FROM {0} WHERE {1}.{2}={3} ",
                TableNameImagesQ, TableNameImagesQ, IdQ, ParId);
        }

        /// <summary>
        /// Get select query for Update Rating
        /// </summary>
        /// <returns>
        /// query string
        /// </returns>
        public static string GetSelectUpdateRatingQueryString()
        {
            return string.Format("SELECT {0},{1},{2},{3} FROM {4} WHERE {5}={6}",
                RatingQ, RateCountQ, LatQ, LongQ, TableNameEventQ, TableNameEventIdQ, ParEventId);
        }

        /// <summary>
        /// Get insert query for Rating
        /// </summary>
        /// <returns>
        /// query string
        /// </returns>
        public static string GetsInsertUpdateRatingQueryString()
        {
            return string.Format("UPDATE {0} SET {1}={2} , {3}={4} WHERE {5}={6}",
                TableNameEventQ, RatingQ, ParRating, RateCountQ, ParRateCount, TableNameEventIdQ, ParEventId);
        }

        /// <summary>
        /// Get select string for current reservation
        /// </summary>
        /// <returns>
        /// query string
        /// </returns>
        public static string GetSelectCurrentReservationQueryString()
        {
            return string.Format("SELECT {0} FROM {1} WHERE {2}={3}",
                ReservedQ, TableNameEventQ, TableNameEventIdQ, ParEventId);
        }

        /// <summary>
        /// Get insert query for Update reservation
        /// </summary>
        /// <returns>
        /// query string
        /// </returns>
        public static string GetInsertUpdateReservationQueryString()
        {
            return string.Format("UPDATE {0} SET {1}={2} WHERE {3}={4}",
                TableNameEventQ, ReservedQ, ParReserved, TableNameEventIdQ, ParEventId);
        }

        /// <summary>
        /// Get Select Location Query string
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public static string GetSelectLocationQueryString()
        {
            return string.Format("SELECT * FROM {0} WHERE {1}={2}", TableNameLocationQ, AddressQ, ParAddress);
        }

        /// <summary>
        /// Get Select Location By Id Query string
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public static string GetSelectLocationByIdQueryString()
        {
            return string.Format("SELECT * FROM {0} WHERE {1}={2}", TableNameLocationQ, IdQ, ParId);
        }

        /// <summary>
        /// Get insert CreateLocation Query String
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public static string GetInsertCreateLocationQueryString()
        {
            return string.Format("INSERT INTO {0} VALUES ({1},{2},{3},{4})",
                TableNameLocationQ, ParId, ParAddress, ParRating, ParRateCount);
        }

        /// <summary>
        /// Get Update Location Rating Query String
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public static string GetUpdateLocationRatingQueryString()
        {
            return string.Format("UPDATE {0} SET {1}={2},{3}={4} WHERE  {5}={6} ",
                TableNameLocationQ, RatingQ, ParRating, RateCountQ,
                ParRateCount, IdQ, ParId);
        }


        /// <summary>
        /// Get Insert Logger Query string
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public static string GetInsertLoggerQueryString()
        {

            return string.Format("insert into {0}(app_name,thread,level,location,message,log_date,exception) values({1},{2},{3},{4},{5},{6},{7})",
                TableNameLogsQ,ParAppName,ParThread,ParLevel,ParLocation,ParMessage,ParLogDate,ParException);
        }

        /// <summary>
        /// Condition validation 
        /// </summary>
        /// <param name="selectString"></param>
        /// <returns>
        /// StringBuilder
        /// </returns>
        private static StringBuilder ConditionValidation(StringBuilder selectString)
        {
            if (isNotFirst)
            {
                selectString.Append(" AND ");
            }
            else
            {
                selectString.Append(" WHERE ");
                isNotFirst = true;
            }

            return selectString;
        }

        #endregion Metods
    }
}