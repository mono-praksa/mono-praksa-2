using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoEvents.Repository
{
    class LocationQueryHelper
    {
        #region Methods
        /// <summary>
        /// Get Select Location Query string
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public static string GetSelectLocationQueryString()
        {
            return string.Format("SELECT * FROM {0} WHERE {1}={2}",
                QueryConstants.TableNameLocationQ, QueryConstants.AddressQ,
                QueryConstants.ParAddress);
        }

        /// <summary>
        /// Get Select Location By Id Query string
        /// </summary>
        /// <returns>
        /// string
        /// </returns>
        public static string GetSelectLocationByIdQueryString()
        {
            return string.Format("SELECT * FROM {0} WHERE {1}={2}",
                QueryConstants.TableNameLocationQ, QueryConstants.IdQ,
                QueryConstants.ParId);
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
                QueryConstants.TableNameLocationQ, QueryConstants.ParId,
                QueryConstants.ParAddress, QueryConstants.ParRating,
                QueryConstants.ParRateCount);
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
                QueryConstants.TableNameLocationQ, QueryConstants.RatingQ, 
                QueryConstants.ParRating, QueryConstants.RateCountQ,
                QueryConstants.ParRateCount, QueryConstants.IdQ,
                QueryConstants.ParId);
        }

        #endregion
    }
}
