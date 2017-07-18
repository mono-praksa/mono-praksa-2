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
                selectString.Append("SELECT COUNT(" + TableNameEventIdQ + "), earth_distance(ll_to_earth("
                    + ParLat + "," + ParLong + "), ll_to_earth("
                    + TableNameEventLatQ + "," + TableNameEventLongQ
                    + ")) as distance from" + TableNameEventQ + "WHERE");
            }
            else
            {
                selectString.Append("SELECT COUNT(" + TableNameEventIdQ + ")  FROM " + TableNameEventQ + " WHERE ");
            }

            bool isNotFirst = false;

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {
                selectString.Append(" (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                   TableNameEventLatQ + ", " + TableNameEventLongQ + ")) ");
                isNotFirst = true;
            }

            /// Adding Time filter query if there is time in filter
            if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                if (isNotFirst) { selectString.Append(" AND "); } else { isNotFirst = true; }

                selectString.Append(" ((" + ParUserStartTime + "," + ParUserEndTime +
                    ")OVERLAPS(" + TableNameEventStartTimeQ + "," + TableNameEventEndTimeQ + ")) ");
            }
            else if (filter.EndTime == null && filter.StartTime > DefaulTime)
            {
                if (isNotFirst) { selectString.Append(" AND "); } else { isNotFirst = true; }

                selectString.Append(" (" + ParUserStartTime + "<" + TableNameEventEndTimeQ + ") ");
            }
            else if (filter.EndTime > DefaulTime && filter.StartTime == null)
            {
                if (isNotFirst) { selectString.Append(" AND "); } else { isNotFirst = true; }

                selectString.Append("(" + ParUserEndTime + ">" + TableNameEventStartTimeQ + ") ");
            }
            ///

            ///Adding searcstring filter in queri if there is searchstring
            if (!String.IsNullOrWhiteSpace(filter.SearchString))
            {
                if (isNotFirst) { selectString.Append(" AND "); } else { isNotFirst = true; }
                selectString.Append(" ( " + NameQ + " ILIKE (" + ParSearchString + ")" + ") ");
            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                if (isNotFirst) { selectString.Append(" AND "); } else { isNotFirst = true; }
                selectString.Append(" (" + ParCategory + " & " + TableNameEventCatQ + " > 0)");
            }

            ///ORDERING orderby orderAscend
            ///

            switch (filter.OrderBy)
            {
                case "Name": selectString.Append(" order by " + TableNameEventNameQ); break;
                case "StartTime": selectString.Append(" order by " + TableNameEventStartTimeQ); break;
                case "EndTime": selectString.Append(" order by " + TableNameEventEndTimeQ); break;
                case "Distance": selectString.Append(" order by distance "); break;
            }

            if (filter.OrderAscending == true && String.IsNullOrEmpty(filter.OrderBy) == false)
            {
                selectString.Append(" asc ");
            }
            else
            {
                selectString.Append(" desc ");
            }

            selectString.Append(" LIMIT(" + filter.PageSize.ToString() +
                ") OFFSET (" + ((filter.PageNumber - 1) * filter.PageSize).ToString() + ") ");

            return selectString.ToString();
        }

        public static string GetSelectEventStringById()
        {
            StringBuilder selectString = new StringBuilder();
            selectString.AppendFormat("SELECT * FROM {0} WHERE {1}={2}", TableNameEventQ, TableNameEventIdQ, ParEventId);
            //string selectString = "Select * from " + TableNameEventQ + " where " + TableNameEventIdQ + "=" + ParEventId + " Limit (1)";

            return selectString.ToString();
        }

        /// <summary>
        /// Gets query filter select string for query events
        /// </summary>
        /// <param name="filter"></param>
        /// <returns> query string </returns>
        public static string GetSelectEventString(IFilter filter)
        {
            StringBuilder selectString = new StringBuilder();

            if (filter.OrderBy == "Distance" && filter.ULat != null && filter.ULong != null)
            {
                selectString.Append("SELECT *, earth_distance(ll_to_earth(" + ParLat + "," + ParLong
                    + "), ll_to_earth(" + TableNameEventLatQ + ","
                    + TableNameEventLongQ + ")) as distance from"
                    + TableNameEventQ + "WHERE ");
            }
            else
            {
                selectString.Append("SELECT * FROM " + TableNameEventQ + " WHERE ");
            }

            bool isNotFirst = false;

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {
                selectString.Append(" (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                   TableNameEventLatQ + ", " + TableNameEventLongQ + ")) ");
                isNotFirst = true;
            }

            /// Adding Time filter query if there is time in filter
            if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                if (isNotFirst) { selectString.Append(" AND "); } else { isNotFirst = true; }

                selectString.Append(" ((" + ParUserStartTime + "," + ParUserEndTime +
                    ")OVERLAPS(" + TableNameEventStartTimeQ + "," + TableNameEventEndTimeQ + ")) ");
            }
            else if (filter.EndTime == null && filter.StartTime > DefaulTime)
            {
                if (isNotFirst) { selectString.Append(" AND "); } else { isNotFirst = true; }

                selectString.Append(" (" + ParUserStartTime + "<" + TableNameEventEndTimeQ + ") ");
            }
            else if (filter.EndTime > DefaulTime && filter.StartTime == null)
            {
                if (isNotFirst) { selectString.Append(" AND "); } else { isNotFirst = true; }

                selectString.Append("(" + ParUserEndTime + ">" + TableNameEventStartTimeQ + ") ");
            }
            ///

            ///Adding searcstring filter in queri if there is searchstring
            if (!String.IsNullOrWhiteSpace(filter.SearchString))
            {
                if (isNotFirst) { selectString.Append(" AND "); } else { isNotFirst = true; }
                selectString.Append(" ( " + NameQ + " ILIKE (" + ParSearchString + ")" + ") ");
            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                if (isNotFirst) { selectString.Append(" AND "); } else { isNotFirst = true; }
                selectString.Append(" (" + ParCategory + " & " + TableNameEventCatQ + " > 0)");
            }

            ///ORDERING orderby orderAscend
            ///

            switch (filter.OrderBy)
            {
                case "Name": selectString.Append(" order by " + TableNameEventNameQ); break;
                case "StartTime": selectString.Append(" order by " + TableNameEventStartTimeQ); break;
                case "EndTime": selectString.Append(" order by " + TableNameEventEndTimeQ); break;
                case "Distance": selectString.Append(" order by distance "); break;
            }

            if (filter.OrderAscending == true && String.IsNullOrEmpty(filter.OrderBy) == false)
            {
                selectString.Append(" asc ");
            }
            else
            {
                selectString.Append(" desc ");
            }

            selectString.Append(" LIMIT(" + filter.PageSize.ToString() +
                ") OFFSET (" + ((filter.PageNumber - 1) * filter.PageSize).ToString() + ") ");

            return selectString.ToString();
        }

        /// <summary>
        /// gets query insert string for event
        /// </summary>
        /// <returns>Query string</returns>
        public static string GetInsertEventString()
        {
            StringBuilder insertString = new StringBuilder();
            insertString.AppendFormat("INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13})",
                TableNameEventQ, IdQ, StartTimeQ, EndTimeQ, LatQ, LongQ, NameQ, DescriptionQ, CategoryQ, PriceQ, CapacityQ, ReservedQ,
                RatingQ, RateCountQ);
            insertString.AppendFormat(" VALUES ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10}, {11}, {12}, {13})",
                TableNameEventQ, ParId, ParStartTime, ParEndTime, ParLat, ParLong, ParName, ParDescription, ParCategory, ParPrice,
                ParCapacity, ParReserved, ParRating, ParRateCount);

            //string insertString = " INSERT INTO " + TableNameEventQ + "values(" + ParId + "," + ParStartTime + "," + ParEndTime +
            //    "," + ParLat + "," + ParLong + "," + ParName + "," + ParDescription + "," + ParCategory + "," + ParPrice + "," +
            //    ParCapacity+ "," + ParReserved + "," + ParRating + "," + ParRateCount  + ") ";

            return insertString.ToString();
        }

        /// <summary>
        /// Gets query select string for Images
        /// </summary>
        /// <returns>Query string</returns>
        public static string GetSelectImagesString()
        {
            string selectString = " SELECT * FROM " + TableNameImagesQ + "WHERE (" + ParEventId + " = " + TableNameImagesQ + "." + EventIdQ + ") ";

            return selectString;
        }

        /// <summary>
        /// get query Insert string for images
        /// </summary>
        /// <returns>guery string</returns>
        public static string GetInsertImagesString()
        {
            string insertString = " INSERT INTO " + TableNameImagesQ +
                " VALUES(" + ParId + "," + ParContent + "," + ParEventId + ") ";

            return insertString;
        }

        /// <summary>
        /// get query select string form images
        /// </summary>
        /// <returns></returns>
        public static string GetSelectImageString()
        {
            string selectString = " SELECT * FROM " + TableNameImagesQ + " WHERE " + TableNameImagesQ + "." + IdQ + "=" + ParId;
            return selectString;
        }

        #endregion Metods
    }
}