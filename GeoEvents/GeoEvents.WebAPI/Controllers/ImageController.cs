using AutoMapper;
using GeoEvents.Model.Common;
using GeoEvents.Service.Common;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeoEvents.WebAPI.Controllers
{
    [RoutePrefix("api/images")]
    public class ImageController : ApiController
    {
        protected IImageService Service { get; private set; }
        protected IMapper Mapper { get; private set; }

        public ImageController(IImageService service, IMapper mapper)
        {
            this.Service = service;
            this.Mapper = mapper;
        }

        //async
        [HttpGet]
        [Route("get/{eventId}")]
        public async Task<IEnumerable<ImageModel>> GetImagesAsync(Guid eventId)
        {
            return Mapper.Map<IEnumerable<ImageModel>>(await Service.GetImagesAsync(eventId));
        }

        //async
        [HttpPost]
        [Route("create/{eventId:guid}")]
        public async Task<HttpResponseMessage> CreateImageAsync(Guid eventId)
        {
            ImageModel img = new ImageModel();
            img.EventId = eventId;
           
            
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            
            var provider = new MultipartMemoryStreamProvider();

            // Read the form data.
            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                img.Content = await file.ReadAsByteArrayAsync();
                //Do whatever you want with filename and its binaray data.
                await Service.CreateImageAsync(Mapper.Map<IImage>(img));
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Upload successful");
        }
    }

    public class ImageModel
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public byte[] Content { get; set; }
    }
}

