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
        static string IdQ = "\"Id\"";
        static string EventIdQ = "\"EventId\"";
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
        static string TNameEventStartTime = TabeNameEventQ + "." + StartTimeQ;
        static string TNameEventEndTime = TabeNameEventQ + "." + EndTimeQ;
        static string TNameEventLong = TabeNameEventQ + "." + LongQ;
        static string TNameEventLat = TabeNameEventQ + "." + LatQ;
        static string TNameEventCat = TabeNameEventQ + "." + CategoryQ;
        static DateTime DefaulTime = new DateTime(0001, 01, 01);
        static int PageSize;
        static int PageNum;
        static string LimitString = "  LIMIT("+PageSize.ToString()+")";
        #endregion

        #region Metods

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

            

            else
            {
                selectString = "SELECT * FROM " + TabeNameEventQ +
                    " WHERE (earth_box(ll_to_earth(" + ParLat + "," + ParLong + ")," + ParRadius + ")@> ll_to_earth(" +
                    TabeNameEventQ + "." + LatQ + ", " + TabeNameEventQ + "." + LongQ + "))";
            }


            selectString=selectString + LimitString;

            if (filter.Category == 0)
            {
                return selectString;
            }

            else
            {
                return selectString + " AND (" + ParCategory + " & " + TNameEventCat + " > 0)" ;
            }
        }

        public static string GetInsertStringEvent()
        {

            string insertString = "INSERT INTO " + TabeNameEventQ + "values(" + ParId + "," + ParStartTime + "," + ParEndTime +
                "," + ParLat + "," + ParLong + "," + ParName + "," + ParDescription + "," + ParCategory + ")";

            return insertString;
        }

        public static string GetSelectStringImages()
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

        public static string GetSelectStringImage()
        {

            string selectString = "SELECT * FROM " + TabeNameImagesQ + " WHERE "+TabeNameImagesQ+"."+IdQ+"="+ParId ;
            return selectString;
        }


        #endregion
    }
}
