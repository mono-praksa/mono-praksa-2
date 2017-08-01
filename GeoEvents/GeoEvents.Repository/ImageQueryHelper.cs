using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GeoEvents.Repository
{
    class ImageQueryHelper
    {

        /// <summary>
        /// Gets query select string for Images
        /// </summary>
        /// <returns>Query string</returns>
        public static string GetSelectImagesQueryString()
        {
            return string.Format(" SELECT * FROM {0} WHERE ({1}={2}.{3})",
                QueryConstants.TableNameImagesQ, QueryConstants.ParEventId,
                QueryConstants.TableNameImagesQ, QueryConstants.EventIdQ);
        }

        /// <summary>
        /// get query Insert string for images
        /// </summary>
        /// <returns>guery string</returns>
        public static string GetInsertImagesQueryString()
        {
            return string.Format(" INSERT INTO {0} VALUES({1},{2},{3}) ",
                QueryConstants.TableNameImagesQ, QueryConstants.ParId,
                QueryConstants.ParContent, QueryConstants.ParEventId);
        }

        /// <summary>
        /// get query select string form images
        /// </summary>
        /// <returns></returns>
        public static string GetSelectImageQueryString()
        {
            return string.Format(" SELECT * FROM {0} WHERE {1}.{2}={3} ",
                QueryConstants.TableNameImagesQ, QueryConstants.TableNameImagesQ,
                QueryConstants.IdQ, QueryConstants.ParId);
        }
    }
}
