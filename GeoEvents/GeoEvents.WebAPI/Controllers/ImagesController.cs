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

        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> Upload()
        {
            List<IImage> img = new List<IImage>();
            ImagesViewModel img1 = new ImagesViewModel();
            img1.Id = Guid.NewGuid();
            img1.EventId = new Guid(("837e7b0b-84e0-4597-a93e-06323a36775e").ToString());

            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
      
            var provider = new MultipartMemoryStreamProvider();

                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(provider);

                foreach (var file in provider.Contents)
                {
                    var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                    img1.Content = await file.ReadAsByteArrayAsync();
                    //Do whatever you want with filename and its binaray data.
                    img.Add(Mapper.Map<IImage>(img1));
                    Service.CreateImages(img);
                }
                return Request.CreateResponse(HttpStatusCode.OK, "Upload successful");
        }
    }
}

