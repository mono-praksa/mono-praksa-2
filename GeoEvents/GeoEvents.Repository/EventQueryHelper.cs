using GeoEvents.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository
{
    class EventQueryHelper
    {
        private static bool isNotFirst = false;

        /// <summary>
        /// Selects all events form Db
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>
        /// string
        /// </returns>
        public static string GetSelectAllEventsQueryString(IFilter filter)
        {
            StringBuilder selectString = new StringBuilder();

            selectString.AppendFormat("SELECT * FROM {0} ", QueryConstants.TableNameEventQ);

            isNotFirst = false;

            selectString = FilterValidation(selectString, filter);
          
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
                QueryConstants.TableNameEventIdQ, QueryConstants.TableNameEventQ);

            isNotFirst = false;

            selectString = FilterValidation(selectString, filter);
            
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
            return string.Format("SELECT * FROM {0} WHERE {1}={2}",
                QueryConstants.TableNameEventQ, QueryConstants.TableNameEventIdQ,
                QueryConstants.ParEventId);
        }

        /// <summary>
        /// Get Rating and reate Count
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public static string GetSelectFromEventRatingQueryString()
        {
            return string.Format("SELECT rating,ratecount FROM {0} WHERE {1}={2}",
                QueryConstants.TableNameEventQ, QueryConstants.TableNameEventIdQ,
                QueryConstants.ParEventId);
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
                selectString.AppendFormat("SELECT *, earth_distance(ll_to_earth({0},{1}), ll_to_earth({2},{3})) AS distance FROM {4} ",
                    QueryConstants.ParLatitude, QueryConstants.ParLongitude,
                    QueryConstants.TableNameEventLatQ, QueryConstants.TableNameEventLongQ,
                    QueryConstants.TableNameEventQ);
            }
            else if (filter.OrderBy == "RatingLocation")
            {
                selectString.AppendFormat("SELECT * FROM {0} INNER JOIN {1} ON ({2} = {3}) ",
                    QueryConstants.TableNameEventQ, QueryConstants.TableNameLocationQ,
                    QueryConstants.TableNameEventLocationIdQ, QueryConstants.TableNameLocationIdQ);
            }
            else
            {
                selectString.AppendFormat("SELECT * FROM {0} ",
                    QueryConstants.TableNameEventQ);
            }

            isNotFirst = false;

            selectString = FilterValidation(selectString, filter);

            ///ORDERING orderby orderAscend
            ///

            switch (filter.OrderBy)
            {
                case "Name":
                    selectString.AppendFormat(" ORDER BY {0} ",
                        QueryConstants.TableNameEventNameQ);
                    break;

                case "StartTime":
                    selectString.AppendFormat(" ORDER BY {0} ",
                        QueryConstants.TableNameEventStartTimeQ);
                    break;

                case "EndTime":
                    selectString.AppendFormat(" ORDER BY {0} ",
                        QueryConstants.TableNameEventEndTimeQ);
                    break;

                case "Distance":
                    selectString.AppendFormat(" ORDER BY {0} ",
                        QueryConstants.DistanceQ);
                    break;

                case "Price":
                    selectString.AppendFormat(" ORDER BY {0} ",
                        QueryConstants.TableNameEventPriceQ);
                    break;

                case "RatingEvent":
                    selectString.AppendFormat(" ORDER BY {0} ",
                        QueryConstants.TableNameEventRatingQ);
                    break;

                case "RatingLocation":
                    selectString.AppendFormat(" ORDER BY {0} ",
                        QueryConstants.TableNameLocationRatingQ);
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
                   QueryConstants.TableNameEventQ, QueryConstants.ParId, QueryConstants.ParName,
                   QueryConstants.ParDescription, QueryConstants.ParCategory, QueryConstants.ParLatitude,
                   QueryConstants.ParLongitude, QueryConstants.ParStartTime, QueryConstants.ParEndTime,
                   QueryConstants.ParRating, QueryConstants.ParRateCount, QueryConstants.ParPrice,
                   QueryConstants.ParCapacity, QueryConstants.ParReserved, QueryConstants.ParCustom,
                   QueryConstants.ParOccurrence, QueryConstants.ParRepeatEvery, QueryConstants.ParRepeatOn,
                   QueryConstants.ParRepeatCount, QueryConstants.ParLocationId);
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
                QueryConstants.RatingQ, QueryConstants.RateCountQ,
                QueryConstants.LatQ, QueryConstants.LongQ,
                QueryConstants.TableNameEventQ, QueryConstants.TableNameEventIdQ,
                QueryConstants.ParEventId);
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
                QueryConstants.TableNameEventQ, QueryConstants.RatingQ,
                QueryConstants.ParRating, QueryConstants.RateCountQ,
                QueryConstants.ParRateCount, QueryConstants.TableNameEventIdQ,
                QueryConstants.ParEventId);
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
                QueryConstants.ReservedQ, QueryConstants.TableNameEventQ,
                QueryConstants.TableNameEventIdQ, QueryConstants.ParEventId);
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
                QueryConstants.TableNameEventQ, QueryConstants.ReservedQ,
                QueryConstants.ParReserved, QueryConstants.TableNameEventIdQ,
                QueryConstants.ParEventId);
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

        /// <summary>
        /// Validets filter parametrs
        /// and adds it to query
        /// </summary>
        /// <param name="selectString"></param>
        /// <param name="filter"></param>
        /// <returns>
        /// string
        /// </returns>
        private static StringBuilder FilterValidation(StringBuilder selectString,IFilter filter)
        {
            /// adding Distance filter to query if there is Location and radius
            if (filter.ULat != null && filter.ULong != null && filter.Radius != 0)
            {
                selectString.Append(" WHERE ");
                selectString.AppendFormat(" (earth_box(ll_to_earth({0},{1}),{2})@> ll_to_earth({3},{4})) ",
                        QueryConstants.ParLatitude, QueryConstants.ParLongitude, QueryConstants.ParRadius,
                        QueryConstants.TableNameEventLatQ, QueryConstants.TableNameEventLongQ);
                isNotFirst = true;
            }

            ///Adding Price filter query if there is price
            if (filter.Price != null)
            {
                selectString = ConditionValidation(selectString);
                selectString.AppendFormat("({0} <= {1}) ",
                    QueryConstants.TableNameEventPriceQ, QueryConstants.ParPrice);
            }

            ///Adding Rating Event filter query if there is rating event
            if (filter.RatingEvent != 0)
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat("({0} >= {1}) ",
                    QueryConstants.TableNameEventRatingQ, QueryConstants.ParRating);
            }

            /// Adding Time filter query if there is time in filter
            if (filter.EndTime > filter.StartTime)
            {
                selectString = ConditionValidation(selectString);

                //selectString.AppendFormat(" (({0},{1}) OVERLAPS ({2},{3})) ",
                //    ParUserStartTime, ParUserEndTime, TableNameEventStartTimeQ, TableNameEventEndTimeQ);

                selectString.AppendFormat(" (check_recurrence({0},{1},{2},{3},{4},{5},{6},{7})) ",
                    QueryConstants.ParUserStartTime, QueryConstants.ParUserEndTime,
                    QueryConstants.TableNameEventStartTimeQ, QueryConstants.TableNameEventEndTimeQ,
                    QueryConstants.TableNameEventOccurrenceQ, QueryConstants.TableNameEventRepeatEveryQ,
                    QueryConstants.TableNameEventRepeatOnQ, QueryConstants.TableNameEventRepeatCountQ);
            }

            ///Adding searcstring filter in queri if there is searchstring
            if (!String.IsNullOrWhiteSpace(filter.SearchString))
            {
                selectString = ConditionValidation(selectString);


                selectString.AppendFormat(" (lower({0}) LIKE lower({1})) ",
                    QueryConstants.NameQ, QueryConstants.ParSearchString);
            }

            /// adding category filter in query if there is category
            if (filter.Category > 0)
            {
                selectString = ConditionValidation(selectString);


                selectString.AppendFormat(" ({0} & {1} > 0)",
                    QueryConstants.ParCategory, QueryConstants.TableNameEventCatQ);
            }

            /// adding custom search
            if (!String.IsNullOrWhiteSpace(filter.Custom))
            {
                selectString = ConditionValidation(selectString);

                selectString.AppendFormat(" ({0} @> {1}) ",
                    QueryConstants.CustomQ, QueryConstants.ParCustom);
            }

            return selectString;
        }

    }
}
