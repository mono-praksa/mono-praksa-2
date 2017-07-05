using GeoEvents.Model.Common;
using GeoEvents.Service.Common;
using GeoEvents.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GeoEvents.WebAPI.Controllers
{
    [RoutePrefix("api/images")]
    public class ImagesController : ApiController
    {
        [HttpGet]
        [Route("get/{eventId}")]
        public List<IImage> GetImages(Guid eventId)
        {
            return Service.GetImages(eventId);
        }
    }
}
