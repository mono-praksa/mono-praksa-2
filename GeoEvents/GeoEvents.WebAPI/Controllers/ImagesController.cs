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
using AutoMapper;

namespace GeoEvents.WebAPI.Controllers
{
    [RoutePrefix("api/images")]
    public class ImagesController : ApiController
    {
        protected IImageService Service { get; private set; }

        public ImagesController(IImageService service)
        {
            this.Service = service;
        }

        [HttpGet]
        [Route("get")]
        //public List<IImage> GetImages(Guid eventId)
        public List<IImage> GetImages()
        {
            return Service.GetImages(new Guid(("98909782-c5b6-4d4c-8a89-0f9186018de4").ToString()));
        }

        [HttpGet]
        [Route("create")]
        //public List<IImage> GetImages(Guid eventId)
        public bool CreateImages()
        {
            List<IImage> img = new List<IImage>();
            ImagesViewModel img1 = new ImagesViewModel();
            img1.Id = Guid.NewGuid();
            img1.EventId = new Guid(("98909782-c5b6-4d4c-8a89-0f9186018de4").ToString());
            img1.Content = new byte[2];
            img.Add(Mapper.Map<IImage>(img1));
            return Service.CreateImages(img1.EventId, img);
        }
    }
}

