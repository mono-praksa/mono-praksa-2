using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Common;

namespace GeoEvents.Repository
{
    static public class QueryHelper
    {
        #region Constants

        /// <summary>
        /// Quoted Constant string
        /// </summary>
        static string TableNameEventQ = "\"Events\"";
        static string TableNameImagesQ = "\"Images\"";
        static string NameQ = "\"Name\"";
        static string LatQ = "\"Lat\"";
        static string LongQ = "\"Long\"";
        static string StartTimeQ = "\"StartTime\"";
        static string EndTimeQ = "\"EndTime\"";
        static string CategoryQ = "\"Category\"";
        static string IdQ = "\"Id\"";
        static string EventIdQ = "\"EventId\"";
        static string DescriptionQ = "\"Description\"";


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
        public static string ParSearchString ="@SearchString";

        /// <summary>
        /// Added Qouted strings
        /// </summary>
        static string TableNameEventNameQ = TableNameEventQ + "." + NameQ;
        static string TableNameEventDescriptionQ = TableNameEventQ + "." + DescriptionQ;
        static string TableNameEventStartTimeQ = TableNameEventQ + "." + StartTimeQ;
        static string TableNameEventEndTimeQ = TableNameEventQ + "." + EndTimeQ;
        static string TableNameEventLongQ = TableNameEventQ + "." + LongQ;
        static string TableNameEventLatQ = TableNameEventQ + "." + LatQ;
        static string TableNameEventCatQ = TableNameEventQ + "." + CategoryQ;
        static string TableNameEventIdQ = TableNameEventQ + "." + IdQ;

        /// <summary>
        /// Default DateTime
        /// </summary>
        static DateTime DefaulTime = new DateTime(0001, 01, 01);

        #endregion

        #region Metods


        /// <summary>
        /// Gets query string with filter for count query
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Query string</returns>
        public static string GetSelectCountEventString(IFilter filter) {

            string selectString;

            if (filter.OrderBy == "Distance")
            {
   
                   selectString = "SELECT COUNT("+ TableNameEventIdQ+"), earth_distance(ll_to_earth(" + ParLat + "," + ParLong + "), ll_to_earth(" + TableNameEventLatQ + ","
                    + TableNameEventLongQ + ")) as distance from" + TableNameEventQ + "WHERE";
            }
            else
            {
                selectString = "SELECT COUNT("+TableNameEventIdQ+")  FROM " + TableNameEventQ + " WHERE ";
            }

            bool isNotFirst = false;

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {             

                selectString += " (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                   TableNameEventLatQ + ", " + TableNameEventLongQ + ")) ";
                isNotFirst = true;
            }

            /// Adding Time filter query if there is time in filter

            if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                if (isNotFirst)
                {
                    selectString += " AND ";
                }
                else
                {
                    isNotFirst = true;
                }

                selectString += " ((" + ParUserStartTime + "," + ParUserEndTime +
                    ")OVERLAPS(" + TableNameEventStartTimeQ + "," + TableNameEventEndTimeQ + ")) ";
            }
            else if (filter.EndTime == null && filter.StartTime > DefaulTime)
            {
                if (isNotFirst)
                {
                    selectString += " AND ";
                }
                else
                {
                    isNotFirst = true;
                }

                selectString += " (" + ParUserStartTime + "<" + TableNameEventEndTimeQ + ") ";
            }
            else if (filter.EndTime > DefaulTime && filter.StartTime == null)
            {
                if (isNotFirst)
                {
                    selectString += " AND ";
                }
                else
                {
                    isNotFirst = true;
                }

                selectString += "(" + ParUserEndTime + ">" + TableNameEventStartTimeQ + ") ";
            }
            ///


            if (!String.IsNullOrWhiteSpace(filter.SearchString))
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }
                selectString += " ( ";
                selectString += NameQ + " ILIKE (" + ParSearchString + ")";            
                selectString += ") ";
               
            }



            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                if (isNotFirst)
                {
                    selectString += " AND ";
                }
                else
                {
                    isNotFirst = true;
                }
                selectString = selectString + " (" + ParCategory + " & " + TableNameEventCatQ + " > 0)";
            }


            ///ORDERING orderby orderAscend
            ///

            switch (filter.OrderBy)
            {
                case "Name": selectString += "order by" + TableNameEventNameQ; break;
                case "StartTime": selectString += "order by" + TableNameEventStartTimeQ; break;
                case "EndTime": selectString += "order by" + TableNameEventEndTimeQ; break;
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

        public static string GetSelectEventStringById() {

            string selectString = "Select * from "+TableNameEventQ+" where "+TableNameEventIdQ + "="+ParEventId +" Limit (1)";


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
                selectString = "SELECT *, earth_distance(ll_to_earth(" + ParLat + "," + ParLong + "), ll_to_earth(" + TableNameEventLatQ + "," 
                    + TableNameEventLongQ + ")) as distance from" + TableNameEventQ + "WHERE ";
            }
            else
            {
                selectString = "SELECT * FROM " + TableNameEventQ + " WHERE ";
            }

            bool isNotFirst = false;

            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius!=0) {

                selectString += " (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                   TableNameEventLatQ + ", " + TableNameEventLongQ + ")) ";
                isNotFirst = true;
            }

            /// Adding Time filter query if there is time in filter
                if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }

                selectString += " ((" + ParUserStartTime + "," + ParUserEndTime +
                    ")OVERLAPS(" + TableNameEventStartTimeQ + "," + TableNameEventEndTimeQ + ")) ";
            }
            else if (filter.EndTime == null && filter.StartTime > DefaulTime)
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }

                selectString += " (" + ParUserStartTime + "<" + TableNameEventEndTimeQ + ") ";
            }
            else if (filter.EndTime > DefaulTime && filter.StartTime == null) 
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }

                selectString += "(" + ParUserEndTime + ">" + TableNameEventStartTimeQ+") ";
            }
            ///




            ///Adding searcstring filter in queri if there is searchstring 
            if (!String.IsNullOrWhiteSpace(filter.SearchString))
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }
                selectString += " ( ";
                selectString += NameQ + " ILIKE (" + ParSearchString + ")";       
                selectString += ") ";

            }




            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                if (isNotFirst) { selectString += " AND "; } else { isNotFirst = true; }
                selectString = selectString +  " (" + ParCategory + " & " + TableNameEventCatQ + " > 0)";
            }


            ///ORDERING orderby orderAscend
            ///

            

            switch (filter.OrderBy)
            {
                case "Name": selectString += " order by " + TableNameEventNameQ; break;
                case "StartTime": selectString += " order by " + TableNameEventStartTimeQ; break;
                case "EndTime": selectString += " order by " + TableNameEventEndTimeQ; break;
                case "Distance": selectString += " order by distance "; break;      
            }

            if (filter.OrderAscending == true  && String.IsNullOrEmpty(filter.OrderBy) == false )
            {
                selectString += " asc ";
            }
            else
            {
                selectString += " desc ";
            }


            
            selectString += " LIMIT(" + filter.PageSize.ToString() + ") OFFSET ("+((filter.PageNumber-1)*filter.PageSize).ToString()+") ";
          


            return selectString;
        }


        /// <summary>
        /// gets query insert string for event
        /// </summary>
        /// <returns>Query string</returns>
        public static string GetInsertEventString()
        {

            string insertString = " INSERT INTO " + TableNameEventQ + "values(" + ParId + "," + ParStartTime + "," + ParEndTime +
                "," + ParLat + "," + ParLong + "," + ParName + "," + ParDescription + "," + ParCategory + ") ";

            return insertString;
        }

        /// <summary>
        /// Gets query select string for Images
        /// </summary>
        /// <returns>Query string</returns>
        public static string GetSelectImagesString()
        {

            string selectString = " SELECT * FROM " + TableNameImagesQ + "WHERE (" + ParEventId + " = " + TableNameImagesQ + "." + EventIdQ + ") ";

            return selectString ;
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

            string selectString = " SELECT * FROM " + TableNameImagesQ + " WHERE "+TableNameImagesQ+"."+IdQ+"="+ParId ;
            return selectString;
        }


        #endregion
    }
}
