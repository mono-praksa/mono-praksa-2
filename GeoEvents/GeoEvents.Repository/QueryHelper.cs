using GeoEvents.Common;
using System;

namespace GeoEvents.Repository
{
    static public class QueryHelper
    {
        #region Constants

        /// <summary>
        /// Quoted Constant string
        /// </summary>
        private static string TabelNameEventQ = "\"Events\"";

        private static string TabelNameImagesQ = "\"Images\"";
        private static string NameQ = "\"Name\"";
        private static string LatQ = "\"Lat\"";
        private static string LongQ = "\"Long\"";
        private static string StartTimeQ = "\"StartTime\"";
        private static string EndTimeQ = "\"EndTime\"";
        private static string CategoryQ = "\"Category\"";
        private static string IdQ = "\"Id\"";
        private static string EventIdQ = "\"EventId\"";
        private static string DescriptionQ = "\"Description\"";

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
        public static string ParSearcString = "@SearchString";

        /// <summary>
        /// Added Qouted strings
        /// </summary>
        private static string TNameEventNameQ = TabelNameEventQ + "." + NameQ;

        private static string TNameEventDescriptionQ = TabelNameEventQ + "." + DescriptionQ;
        private static string TNameEventStartTimeQ = TabelNameEventQ + "." + StartTimeQ;
        private static string TNameEventEndTimeQ = TabelNameEventQ + "." + EndTimeQ;
        private static string TNameEventLongQ = TabelNameEventQ + "." + LongQ;
        private static string TNameEventLatQ = TabelNameEventQ + "." + LatQ;
        private static string TNameEventCatQ = TabelNameEventQ + "." + CategoryQ;
        private static string TNameEventIdQ = TabelNameEventQ + "." + IdQ;

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
        public static string GetSelectCountEventString(IFilter filter)
        {
            string selectString;

            if (filter.OrderBy == "Distance")
            {
                selectString = "SELECT COUNT(" + TNameEventIdQ + "), earth_distance(ll_to_earth(" + ParLat + "," + ParLong + "), ll_to_earth(" + TNameEventLatQ + ","
                 + TNameEventLongQ + ")) as distance from" + TabelNameEventQ + "WHERE";
            }
            else
            {
                selectString = "SELECT COUNT(" + TNameEventIdQ + ")  FROM " + TabelNameEventQ + " WHERE ";
            }

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {
                selectString += " (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                   TNameEventLatQ + ", " + TNameEventLongQ + ")) ";
            }

            /// Adding Time filter query if there is time in filter

            if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                selectString += " AND ";

                selectString += " ((" + ParUserStartTime + "," + ParUserEndTime +
                    ")OVERLAPS(" + TNameEventStartTimeQ + "," + TNameEventEndTimeQ + ")) ";
            }
            else if (filter.EndTime == null && filter.StartTime > DefaulTime)
            {
                selectString += " AND ";

                selectString += " (" + ParUserStartTime + "<" + TNameEventEndTimeQ + ") ";
            }
            else if (filter.EndTime > DefaulTime && filter.StartTime == null)
            {
                selectString += " AND ";

                selectString += "(" + ParUserEndTime + ">" + TNameEventStartTimeQ + ") ";
            }
            ///

            ///Adding searcstring filter in queri if there is searchstring
            if (!string.IsNullOrEmpty(filter.SearchString))
            {
                selectString += " AND";

                selectString += TNameEventNameQ + " ILIKE '%" + ParSearcString + "%'";

                if (filter.NameOnly == false)
                {
                    selectString += " AND" + TNameEventDescriptionQ + " ILIKE '%" + ParSearcString + "%'";
                }
            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                selectString += " AND ";
                selectString = selectString + " (" + ParCategory + " & " + TNameEventCatQ + " > 0)";
            }

            ///ORDERING orderby orderAscend
            ///

            switch (filter.OrderBy)
            {
                case "Name": selectString += "order by" + TNameEventNameQ; break;
                case "StartTime": selectString += "order by" + TNameEventStartTimeQ; break;
                case "EndTime": selectString += "order by" + TNameEventEndTimeQ; break;
                case "Distance": selectString += "order by distance"; break;
            }

            if (filter.OrderAscending == true)
            {
                selectString += "asc ";
            }
            else
            {
                selectString += "desc ";
            }

            ///pageing
            selectString += "LIMIT(" + filter.PageSize.ToString() + ") OFFSET (" + ((filter.PageNumber - 1) * filter.PageSize).ToString() + ")";
            ///

            return selectString;
        }

        public static string GetSelectEventStringById()
        {
            string selectString = "Select * from " + TabelNameEventQ + " where " + TNameEventIdQ + "=" + ParEventId + " Limit (1)";

            return selectString;
        }

        /// <summary>
        /// Gets query filter select string for query events
        /// </summary>
        /// <param name="filter"></param>
        /// <returns> query string </returns>
        public static string GetSelectEventString(IFilter filter)
        {
            string selectString;

            if (filter.OrderBy == "Distance" && filter.ULat != null && filter.ULong != null)
            {
                selectString = "SELECT *, earth_distance(ll_to_earth(" + ParLat + "," + ParLong + "), ll_to_earth(" + TNameEventLatQ + ","
                    + TNameEventLongQ + ")) as distance from" + TabelNameEventQ + "WHERE ";
            }
            else
            {
                selectString = "SELECT * FROM " + TabelNameEventQ + " WHERE ";
            }

            bool isNotFirst = false;

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {
                selectString += " (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                   TNameEventLatQ + ", " + TNameEventLongQ + ")) ";
                isNotFirst = true;
            }

            /// Adding Time filter query if there is time in filter
            if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }

                selectString += " ((" + ParUserStartTime + "," + ParUserEndTime +
                    ")OVERLAPS(" + TNameEventStartTimeQ + "," + TNameEventEndTimeQ + ")) ";
            }
            else if (filter.EndTime == null && filter.StartTime > DefaulTime)
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }

                selectString += " (" + ParUserStartTime + "<" + TNameEventEndTimeQ + ") ";
            }
            else if (filter.EndTime > DefaulTime && filter.StartTime == null)
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }

                selectString += "(" + ParUserEndTime + ">" + TNameEventStartTimeQ + ") ";
            }
            ///

            ///Adding searcstring filter in queri if there is searchstring
            if (!String.IsNullOrWhiteSpace(filter.SearchString))
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }
                selectString += " ( ";
                selectString += TNameEventNameQ + " ILIKE (\'%" + ParSearcString + "%\')";
                //selectString += String.Format("{0} ILIKE (\'%{1}%\')", TNameEventNameQ, ParSearcString);

                //if (filter.NameOnly == false)
                //{
                //    //selectString += " OR(" + TNameEventDescriptionQ + " ILIKE (\'%" + ParSearcString + "%\')))";
                //    selectString += string.Format(" OR ({0} ILIKE (\'%{1}%\')))", TNameEventDescriptionQ, ParSearcString);
                //}
                //else {
                selectString += ") ";
                //}
            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }
                selectString = selectString + " (" + ParCategory + " & " + TNameEventCatQ + " > 0)";
            }

            ///ORDERING orderby orderAscend
            ///

            switch (filter.OrderBy)
            {
                case "Name": selectString += " order by " + TNameEventNameQ; break;
                case "StartTime": selectString += " order by " + TNameEventStartTimeQ; break;
                case "EndTime": selectString += " order by " + TNameEventEndTimeQ; break;
                case "Distance": selectString += " order by distance "; break;
            }

            if (filter.OrderAscending == true && String.IsNullOrEmpty(filter.OrderBy) == false)
            {
                selectString += " asc ";
            }
            else
            {
                selectString += " desc ";
            }

            selectString += " LIMIT(" + filter.PageSize.ToString() + ") OFFSET (" + ((filter.PageNumber - 1) * filter.PageSize).ToString() + ") ";

            return selectString;
        }

        /// <summary>
        /// gets query insert string for event
        /// </summary>
        /// <returns>Query string</returns>
        public static string GetInsertEventString()
        {
            string insertString = " INSERT INTO " + TabelNameEventQ + "values(" + ParId + "," + ParStartTime + "," + ParEndTime +
                "," + ParLat + "," + ParLong + "," + ParName + "," + ParDescription + "," + ParCategory + ") ";

            return insertString;
        }

        /// <summary>
        /// Gets query select string for Images
        /// </summary>
        /// <returns>Query string</returns>
        public static string GetSelectImagesString()
        {
            string selectString = " SELECT * FROM " + TabelNameImagesQ + "WHERE (" + ParEventId + " = " + TabelNameImagesQ + "." + EventIdQ + ") ";

            return selectString;
        }

        /// <summary>
        /// get query Insert string for images
        /// </summary>
        /// <returns>guery string</returns>
        public static string GetInsertImagesString()
        {
            string insertString = " INSERT INTO " + TabelNameImagesQ +
                " VALUES(" + ParId + "," + ParContent + "," + ParEventId + ") ";

            return insertString;
        }

        /// <summary>
        /// get query select string form images
        /// </summary>
        /// <returns></returns>
        public static string GetSelectImageString()
        {
            string selectString = " SELECT * FROM " + TabelNameImagesQ + " WHERE " + TabelNameImagesQ + "." + IdQ + "=" + ParId;
            return selectString;
        }

        #endregion Metods
    }
}