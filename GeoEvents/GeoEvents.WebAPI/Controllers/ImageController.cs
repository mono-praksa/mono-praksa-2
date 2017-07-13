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
    public class ImageController : ApiController
    {
        protected IImageService Service { get; private set; }
        protected IMapper Mapper { get; private set; }

        public ImageController(IImageService service, IMapper mapper)
        {
            this.Service = service;
            this.Mapper = mapper;
        }

        //[HttpGet]
        //[Route("get/{eventId:guid}")]
        //public List<ImageModel> GetImages(Guid eventId)
        //{
        //    return Mapper.Map<List<ImageModel>>(Service.GetImages(eventId));
        //}

        //async
        [HttpGet]
        [Route("get/{eventId}")]
        public async Task<IEnumerable<ImageModel>> GetImagesAsync(Guid eventId)
        {
            return Mapper.Map<IEnumerable<ImageModel>>(await Service.GetImagesAsync(eventId));
        }

        //[HttpPost]
        //[Route("create")]
        //public async Task<HttpResponseMessage> CreateImages()
        //{
        //    List<ImageModel> img = new List<ImageModel>();
        //    ImageModel img1 = new ImageModel();
        //    img1.Id = Guid.NewGuid();
        //    img1.EventId = new Guid(("0b6b497b-7717-4a3e-bf1e-8bf7fca151d8").ToString());

        //    if (!Request.Content.IsMimeMultipartContent())
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

        //    var provider = new MultipartMemoryStreamProvider();

        //    // Read the form data.
        //    await Request.Content.ReadAsMultipartAsync(provider);

        //    foreach (var file in provider.Contents)
        //    {
        //        var filename = file.Headers.ContentDisposition.FileName.Trim('\"');

        //        if (!System.Web.MimeMapping.GetMimeMapping(filename).StartsWith("image/")) {
        //            throw new Exception("You can upload images only!");
        //        }

        //        img1.Content = await file.ReadAsByteArrayAsync();
        //        //Do whatever you want with filename and its binaray data.
        //        img.Add(img1);
        //        Service.CreateImages(Mapper.Map<List<IImage>>(img));
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, "Upload successful");
        //}

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
}
public class ImageModel
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public byte[] Content { get; set; }
}

