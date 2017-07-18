using GeoEvents.Common;
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
        private static string TableNameEventQ = "\"Events\"";

        private static string TableNameImagesQ = "\"Images\"";
        private static string NameQ = "\"Name\"";
        private static string LatQ = "\"Lat\"";
        private static string LongQ = "\"Long\"";
        private static string StartTimeQ = "\"StartTime\"";
        private static string EndTimeQ = "\"EndTime\"";
        private static string CategoryQ = "\"Category\"";
        private static string IdQ = "\"Id\"";
        private static string EventIdQ = "\"EventId\"";
        private static string DescriptionQ = "\"Description\"";

        private static string PriceQ = "\"Price\"";
        private static string CapacityQ = "\"Capacity\"";
        private static string ReservedQ = "\"Reserved\"";
        private static string RatingQ = "\"Rating\"";
        private static string RateCountQ = "\"RateCount\"";

        /// <summary>
        /// Parametar Constant strings
        /// </summary>
        public static string ParLat = "@Lat";

        public static string ParLong = "@Long";
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
        public static string ParRatingEvent = "@RatingEvent";

        /// <summary>
        /// Added Qouted strings
        /// </summary>
        private static string TableNameEventNameQ = TableNameEventQ + "." + NameQ;

        private static string TableNameEventDescriptionQ = TableNameEventQ + "." + DescriptionQ;
        private static string TableNameEventStartTimeQ = TableNameEventQ + "." + StartTimeQ;
        private static string TableNameEventEndTimeQ = TableNameEventQ + "." + EndTimeQ;
        private static string TableNameEventLongQ = TableNameEventQ + "." + LongQ;
        private static string TableNameEventLatQ = TableNameEventQ + "." + LatQ;
        private static string TableNameEventCatQ = TableNameEventQ + "." + CategoryQ;
        private static string TableNameEventIdQ = TableNameEventQ + "." + IdQ;
        private static string TableNameEventPriceQ = TableNameEventQ + "." + PriceQ;
        private static string TableNameEventRatingQ = TableNameEventQ + "." + RatingQ;
        private static string TableNameEventRateCountQ = TableNameEventQ + "." + RateCountQ;

        /// <summary>
        /// Default DateTime
        /// </summary>
        private static DateTime DefaulTime = new DateTime(0001, 01, 01);

        #endregion Constants

        #region Metods

        /// <summary>
        /// Gets query string with filter for count query
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Query string</returns>
        ///

        public static string GetSelectCountEventString(IFilter filter)
        {
            StringBuilder selectString = new StringBuilder();

            if (filter.OrderBy == "Distance")
            {
                selectString.AppendFormat("SELECT COUNT({0}), earth_distance(ll_to_earth({1},{2}), ll_to_earth({3},{4})) as distance from {5} WHERE",
                    TableNameEventIdQ, ParLat, ParLong, TableNameEventLatQ, TableNameEventLongQ, TableNameEventQ);
            }
            else
            {
                selectString.AppendFormat("SELECT COUNT({0}) FROM {1} WHERE",
                    TableNameEventIdQ, TableNameEventQ);
            }

            bool isNotFirst = false;

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {
                selectString.AppendFormat(" (earth_box(ll_to_earth({0},{1}),{2})@> ll_to_earth({3},{4}))",
                    ParLat, ParLong, ParRadius, TableNameEventLatQ, TableNameEventLongQ);

                isNotFirst = true;
            }

            /// Adding Time filter query if there is time in filter
            if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }

                selectString.AppendFormat(" ({0},{1})OVERLAPS({2},{3})",
                    ParUserStartTime, ParUserEndTime, TableNameEventStartTimeQ, TableNameEventEndTimeQ);
            }
            else if (filter.EndTime == null && filter.StartTime > DefaulTime)
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }

                selectString.AppendFormat(" {0} < {1})",
                    ParUserStartTime, TableNameEventEndTimeQ);
            }
            else if (filter.EndTime > DefaulTime && filter.StartTime == null)
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }

                selectString.AppendFormat("({0}>{1}) ",
                    ParUserEndTime, TableNameEventStartTimeQ);
            }
            ///

            ///Adding searcstring filter in queri if there is searchstring
            if (!String.IsNullOrWhiteSpace(filter.SearchString))
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }
                selectString.AppendFormat(" (  {0} ILIKE ('% {1} %')) ",
                    NameQ, ParSearchString);
            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }
                selectString.AppendFormat(" ({0} & {1} > 0",
                    ParCategory, TableNameEventCatQ);
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

                case "Rating":
                    selectString.AppendFormat(" order by {0} ",
                        TableNameEventRatingQ);
                    break;
            }

            if (filter.OrderAscending == true && String.IsNullOrEmpty(filter.OrderBy) == false)
            {
                selectString.Append(" asc ");
            }
            else
            {
                selectString.Append(" desc ");
            }

            selectString.AppendFormat(" LIMIT({0}) OFFSET ({1})",
                filter.PageSize.ToString(), ((filter.PageNumber - 1) * filter.PageSize).ToString());

            return selectString.ToString();
        }

        public static string GetSelectEventStringById()
        {
            StringBuilder selectString = new StringBuilder();
            selectString.AppendFormat("SELECT * FROM {0} WHERE {1}={2}", TableNameEventQ, TableNameEventIdQ, ParEventId);

            return selectString.ToString();
        }

        /// <summary>
        /// Gets query filter select string for query events
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>
        /// query string
        /// </returns>
        public static string GetSelectEventString(IFilter filter)
        {
            StringBuilder selectString = new StringBuilder();

            if (filter.OrderBy == "Distance" && filter.ULat != null && filter.ULong != null)
            {
                selectString.AppendFormat("SELECT *, earth_distance(ll_to_earth({0},{1}), ll_to_earth({2},{3})) as distance from {4} WHERE ",
                    ParLat, ParLong, TableNameEventLatQ, TableNameEventLongQ, TableNameEventQ);
            }
            else
            {
                selectString.AppendFormat("SELECT * FROM {0} WHERE ",
                    TableNameEventQ);
            }

            bool isNotFirst = false;

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {
                selectString.AppendFormat(" (earth_box(ll_to_earth({0},{1}),{2})@> ll_to_earth({3},{4})) ",
      ParLat, ParLong, ParRadius, TableNameEventLatQ, TableNameEventLongQ);
                isNotFirst = true;
            }

            //Adding Price filter query if there is price
            if (filter.Price != null)
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }
                selectString.AppendFormat("({0} < {1}) ",
                    TableNameEventPriceQ, ParPrice);
            }

            //Adding Rating Event filter query if there is rating event
            if (filter.RatingEvent != null)
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }
                selectString.AppendFormat("({0} > {1}) ",
                    TableNameEventRatingQ, ParRating);
            }

            /// Adding Time filter query if there is time in filter
            if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }

                selectString.AppendFormat(" (({0},{1}) OVERLAPS ({2},{3})) ",
                    ParUserStartTime, ParUserEndTime, TableNameEventStartTimeQ, TableNameEventEndTimeQ);
            }
            else if (filter.EndTime == null && filter.StartTime > DefaulTime)
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }

                selectString.AppendFormat(" ({0}<{1}) ",
                    ParUserStartTime, TableNameEventEndTimeQ);
            }
            else if (filter.EndTime > DefaulTime && filter.StartTime == null)
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }

                selectString.AppendFormat("{0}>{1}) ",
                    ParUserEndTime, TableNameEventStartTimeQ);
            }
            ///

            ///Adding searcstring filter in queri if there is searchstring
            if (!String.IsNullOrWhiteSpace(filter.SearchString))
            {
                if (isNotFirst)

                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }

                selectString.AppendFormat(" ({0} ILIKE ({1})) ",
                    NameQ, ParSearchString);
            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                if (isNotFirst)
                {
                    selectString.Append(" AND ");
                }
                else
                {
                    isNotFirst = true;
                }

                selectString.AppendFormat(" ({0} & {1} > 0)",
                    ParCategory, TableNameEventCatQ);
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
                    // case "RatingLocation": selectString.Append(" order by ")
            }

            if (filter.OrderAscending == true && String.IsNullOrEmpty(filter.OrderBy) == false)
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
        public static string GetInsertEventString()
        {
            StringBuilder insertString = new StringBuilder();
            insertString.AppendFormat("INSERT INTO {0} VALUES ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13})",
                TableNameEventQ, ParId, ParStartTime, ParEndTime, ParLat, ParLong, ParName, ParDescription, ParCategory, ParPrice,
                ParCapacity, ParReserved, ParRating, ParRateCount);

            return insertString.ToString();
        }

        /// <summary>
        /// Gets query select string for Images
        /// </summary>
        /// <returns>Query string</returns>
        public static string GetSelectImagesString()
        {
            StringBuilder selectString = new StringBuilder();
            selectString.AppendFormat(" SELECT * FROM {0} WHERE ({1}={2}.{3})",
                TableNameImagesQ, ParEventId, TableNameImagesQ, EventIdQ);

            return selectString.ToString();
        }

        /// <summary>
        /// get query Insert string for images
        /// </summary>
        /// <returns>guery string</returns>
        public static string GetInsertImagesString()
        {
            StringBuilder insertString = new StringBuilder();
            insertString.AppendFormat(" INSERT INTO {0} VALUES({1},{2},{3}) ",
                TableNameImagesQ, ParId, ParContent, ParEventId);

            return insertString.ToString();
        }

        /// <summary>
        /// get query select string form images
        /// </summary>
        /// <returns></returns>
        public static string GetSelectImageString()
        {
            StringBuilder selectString = new StringBuilder();
            selectString.AppendFormat(" SELECT * FROM {0} WHERE {1}.{2}={3} ",
                TableNameImagesQ, TableNameImagesQ, IdQ, ParId);
            return selectString.ToString();
        }

        #endregion Metods
    }
}