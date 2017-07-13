﻿using System;
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
        /// 
        /// </summary>
        static string TabelNameEventQ = "\"Events\"";
        static string TabelNameImagesQ = "\"Images\"";
        static string NameQ = "\"Name\"";
        static string LatQ = "\"Lat\"";
        static string LongQ = "\"Long\"";
        static string StartTimeQ = "\"StartTime\"";
        static string EndTimeQ = "\"EndTime\"";
        static string CategoryQ = "\"Category\"";
        static string IdQ = "\"Id\"";
        static string EventIdQ = "\"EventId\"";
        static string DescriptionQ = "\"Description\"";

        static string ParLat = "@Lat";
        static string ParLong = "@Long";
        static string ParName = "@Name";
        static string ParEndTime = "@EndTime";
        static string ParStartTime = "@StartTime";
        static string ParUserEndTime = "@UserEndTime";
        static string ParUserStartTime = "@UserStartTime";
        static string ParId = "@Id";
        static string ParCategory = "@Category";
        static string ParDescription = "@Description";
        static string ParRadius = "@Radius";
        static string ParEventId = "@EventId";
        static string ParContent = "@Content";
        static string ParSearcString ="@SearchString";

        static string TNameEventName = TabelNameEventQ + "." + NameQ;
        static string TNameEventDescription = TabelNameEventQ + "." + DescriptionQ;
        static string TNameEventStartTime = TabelNameEventQ + "." + StartTimeQ;
        static string TNameEventEndTime = TabelNameEventQ + "." + EndTimeQ;
        static string TNameEventLong = TabelNameEventQ + "." + LongQ;
        static string TNameEventLat = TabelNameEventQ + "." + LatQ;
        static string TNameEventCat = TabelNameEventQ + "." + CategoryQ;
        static string TNameEventId = TabelNameEventQ + "." + IdQ;

        static DateTime DefaulTime = new DateTime(0001, 01, 01);
        static string CountString;
        #endregion

        #region Metods

        public static string GetEventCountString(IFilter filter) {

            string selectString;

            if (filter.OrderBy == "Distance")
            {
   
                   selectString = "SELECT COUNT("+ TNameEventId+"), earth_distance(ll_to_earth(" + ParLat + "," + ParLong + "), ll_to_earth(" + TNameEventLat + ","
                    + TNameEventLong + ")) as distance from" + TabelNameEventQ + "WHERE";
            }
            else
            {
                selectString = "SELECT COUNT("+TNameEventId+")  FROM " + TabelNameEventQ + " WHERE ";
            }



            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != null)
            {

                selectString += " (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                   TNameEventLat + ", " + TNameEventLong + ")) ";
            }

            /// Adding Time filter query if there is time in filter

            if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                selectString += " AND ";

                selectString += " ((" + ParUserStartTime + "," + ParUserEndTime +
                    ")OVERLAPS(" + TNameEventStartTime + "," + TNameEventEndTime + ")) ";
            }
            else if (filter.EndTime == null && filter.StartTime > DefaulTime)
            {
                selectString += " AND ";

                selectString += " (" + ParUserStartTime + "<" + TNameEventEndTime + ") ";
            }
            else if (filter.EndTime > DefaulTime && filter.StartTime == null)
            {
                selectString += " AND ";

                selectString += "(" + ParUserEndTime + ">" + TNameEventStartTime + ") ";
            }
            ///


            ///Adding searcstring filter in queri if there is searchstring 
            if (filter.SearchString != "" || filter.SearchString != null)
            {
                selectString += " AND";

                selectString += TNameEventName + " ILIKE '%" + ParSearcString + "%'";

                if (filter.NameOnly == false)
                {
                    selectString += " AND" +  TNameEventDescription + " ILIKE '%" + ParSearcString + "%'";
                }

            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                selectString += " AND ";
                selectString = selectString + " (" + ParCategory + " & " + TNameEventCat + " > 0)";
            }


            ///ORDERING orderby orderAscend
            ///

            switch (filter.OrderBy)
            {
                case "Name": selectString += "order by" + TNameEventName; break;
                case "StartTime": selectString += "order by" + TNameEventStartTime; break;
                case "EndTime": selectString += "order by" + TNameEventEndTime; break;
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

        public static string GetSelectStringEvent(IFilter filter)
        {

            string selectString;
           
            if (filter.OrderBy == "Distance")
            {
                selectString = "SELECT *, earth_distance(ll_to_earth(" + ParLat + "," + ParLong + "), ll_to_earth(" + TNameEventLat + "," 
                    + TNameEventLong + ")) as distance from" + TabelNameEventQ + "WHERE ";
            }
            else
            {
                selectString = "SELECT * FROM " + TabelNameEventQ + " WHERE ";
            }
            


            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius!=null) {

                selectString += " (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                   TNameEventLat + ", " + TNameEventLong + ")) ";
            }

            /// Adding Time filter query if there is time in filter

                if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                selectString += " AND ";

                selectString += " ((" + ParUserStartTime + "," + ParUserEndTime +
                    ")OVERLAPS(" + TNameEventStartTime + "," + TNameEventEndTime + ")) ";
            }
            else if (filter.EndTime == null && filter.StartTime > DefaulTime)
            {
                selectString += " AND ";

                selectString += " (" + ParUserStartTime + "<" + TNameEventEndTime + ") ";
            }
            else if (filter.EndTime > DefaulTime && filter.StartTime == null) 
            {
                selectString += " AND ";

                selectString += "(" + ParUserEndTime + ">" + TNameEventStartTime+") ";
            }
            ///




            ///Adding searcstring filter in queri if there is searchstring 
            if (filter.SearchString != "")
            {
                selectString += " AND";

                selectString += TNameEventName + " ILIKE '%" + ParSearcString + "%'";

                if (filter.NameOnly == false)
                {
                    selectString += " AND" + TNameEventDescription + " ILIKE '%" + ParSearcString + "%'";
                }

            }




            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                selectString += " AND ";
                selectString = selectString +  " (" + ParCategory + " & " + TNameEventCat + " > 0)";
            }


            ///ORDERING orderby orderAscend
            ///

            

            switch (filter.OrderBy)
            {
                case "Name": selectString += " order by " + TNameEventName; break;
                case "StartTime": selectString += " order by " + TNameEventStartTime; break;
                case "EndTime": selectString += " order by " + TNameEventEndTime; break;
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


            /// COunt string befor pageing
            CountString = selectString;

            ///pageing 
            selectString += " LIMIT(" + filter.PageSize.ToString() + ") OFFSET ("+((filter.PageNumber-1)*filter.PageSize).ToString()+") ";
            ///


            return selectString;
        }

        public static string GetInsertStringEvent()
        {

            string insertString = " INSERT INTO " + TabelNameEventQ + "values(" + ParId + "," + ParStartTime + "," + ParEndTime +
                "," + ParLat + "," + ParLong + "," + ParName + "," + ParDescription + "," + ParCategory + ") ";

            return insertString;
        }

        public static string GetSelectStringImages()
        {

            string selectString = " SELECT * FROM " + TabelNameImagesQ + "WHERE (" + ParEventId + " = " + TabelNameImagesQ + "." + EventIdQ + ") ";

            return selectString ;
        }

        public static string GetInsertStringImages()
        {

            string insertString = " INSERT INTO " + TabelNameImagesQ +
                " VALUES(" + ParId + "," + ParContent + "," + ParEventId + ") ";

            return insertString;
        }

        public static string GetSelectStringImage()
        {

            string selectString = " SELECT * FROM " + TabelNameImagesQ + " WHERE "+TabelNameImagesQ+"."+IdQ+"="+ParId ;
            return selectString;
        }


        #endregion
    }
}
