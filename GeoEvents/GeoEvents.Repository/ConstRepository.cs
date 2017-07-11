using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Common;

namespace GeoEvents.Repository
{
    static public class ConstRepository
    {
        #region Constants
        static string TabeNameEventQ = "\"Events\"";
        static string TabeNameImagesQ = "\"Images\"";
        static string LatQ = "\"Lat\"";
        static string LongQ = "\"Long\"";
        static string StartTimeQ = "\"StartTime\"";
        static string EndTimeQ = "\"EndTime\"";
        static string CategoryQ = "\"Category\"";
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
        static string ParContent = "@Contetnt";
        static string EventIdQ = "\"EventId\"";
        static string TNameEventStartTime = TabeNameEventQ + "." + StartTimeQ;
        static string TNameEventEndTime = TabeNameEventQ + "." + EndTimeQ;
        static string TNameEventLong = TabeNameEventQ + "." + LongQ;
        static string TNameEventLat = TabeNameEventQ + "." + LatQ;
        static string TNameEventCat = TabeNameEventQ + "." + CategoryQ;
        static DateTime DefaulTime = new DateTime(0001, 01, 01);
        static int PageSize=10;
        static string LimitString = "  LIMIT("+PageSize.ToString()+")";
        #endregion

        #region Metods

        public static string GetCountSelectString(Filter filter)
        {
            string selectCountString;


            if (filter.EndTime == DefaulTime && filter.StartTime > DefaulTime)
            {
                selectCountString = "SELECT COUNT (\"Id\") FROM " + TabeNameEventQ + " WHERE (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                   TNameEventLat + ", " + TNameEventLong + ")) AND (" + ParUserStartTime + "<" + TNameEventEndTime + ")";
            }

            else if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                selectCountString = "SELECT COUNT (\"Id\") FROM " + TabeNameEventQ + " WHERE (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                    TNameEventLat + ", " + TNameEventLong + ")) AND ((" + ParUserStartTime + "," + ParUserEndTime +
                    ")OVERLAPS(" + TNameEventStartTime + "," + TNameEventEndTime + "))";

                //selectCountString = string.Format("SELECT COUNT (\"Id\") FROM {0} WHERE (earth_box(ll_to_earth({1}, {2}), {3}) @> ll_to_earth({4}, {5}))",
                //    TabeNameEventQ, ParLat, ParLong, ParRadius, TNameEventLat, TNameEventLong);
            }

            else
            {
                selectCountString = "SELECT COUNT (\"Id\") FROM " + TabeNameEventQ + " WHERE (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                    TNameEventLat + ", " + TNameEventLong + "))";

            }

            if (filter.Category == 0)
            {
                return selectCountString+ LimitString;
            }

            else
            {
                return selectCountString + " AND (" + ParCategory + " & " + TNameEventCat + " > 0)" + LimitString;
            }

        }



        public static string GetSelectStringEvent(Filter filter)
        {
            string selectString;


            if (filter.EndTime == DefaulTime && filter.StartTime > DefaulTime)
            {
                selectString = "SELECT * FROM " + TabeNameEventQ +
                    " WHERE (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                    TNameEventLat + ", " + TNameEventLong + ")) AND (" + ParUserStartTime + "<" + TNameEventEndTime + ")";

            }

            else if (filter.EndTime > DefaulTime && filter.StartTime > DefaulTime)
            {
                selectString = "SELECT * FROM " + TabeNameEventQ +
                    " WHERE (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                    TNameEventLat + ", " + TNameEventLong + ")) AND ((" + ParUserStartTime + "," + ParUserEndTime +
                    ")OVERLAPS(" + TNameEventStartTime + "," + TNameEventEndTime + "))";
            }
            //"SELECT * FROM \"Events\"WHERE 
            //    (earth_box(ll_to_earth(@Lat, @Long), @Radius)@> ll_to_earth(\"Events\".\"Lat\", \"Events\".\"Long\"))

            //    AND ((@UserStartTime, @UserEndTime)OVERLAPS(\"Events\".\"StartTime\",\"Events\".\"EndTime\")) AND (@Category & \"Events\".\"Category\" > 0) ",
            else
            {
                selectString = "SELECT * FROM " + TabeNameEventQ +
                    " WHERE (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                    TabeNameEventQ + "." + LatQ + ", " + TabeNameEventQ + "." + LongQ + "))";
            }

            if (filter.Category == 0)
            {
                return selectString + LimitString;
            }

            else
            {
                return selectString + " AND (" + ParCategory + " & " + TNameEventCat + " > 0)" + LimitString;
            }
        }

        public static string GetInsertStringEvent()
        {

            string insertString = "INSERT INTO " + TabeNameEventQ + "values(" + ParId + "," + ParStartTime + "," + ParEndTime +
                "," + ParLat + "," + ParLong + "," + ParName + "," + ParDescription + "," + ParCategory + ")";

            return insertString;
        }

        public static string GetSelectStringImages(Guid eventID)
        {

            string selectString = "SELECT * FROM " + TabeNameImagesQ + "WHERE (" + ParEventId + " = " + TabeNameImagesQ + "." + EventIdQ + ")";

            return selectString + LimitString;
        }

        public static string GetInsertStringImages()
        {

            string insertString = "INSERT INTO " + TabeNameImagesQ +
                " VALUES(" + ParId + "," + ParContent + "," + ParEventId + ")";

            return insertString;
        }
        #endregion
    }
}
