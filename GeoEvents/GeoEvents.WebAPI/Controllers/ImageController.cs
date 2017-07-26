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
        #region Properties

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <value>
        /// The service.
        /// </value>
        protected IImageService Service { get; private set; }

        /// <summary>
        /// Gets the mapper.
        /// </summary>
        /// <value>
        /// The mapper.
        /// </value>
        protected IMapper Mapper { get; private set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageController"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="mapper">The mapper.</param>
        public ImageController(IImageService service, IMapper mapper)
        {
            this.Service = service;
            this.Mapper = mapper;
        }

        #endregion Constructor

        #region Methods

        //async
        /// <summary>
        /// Gets the images asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("get/{eventId}")]
        public async Task<HttpResponseMessage> GetImagesAsync(Guid eventId)
        {
            var result = Mapper.Map<IEnumerable<ImageModel>>(await Service.GetImagesAsync(eventId));

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// Creates the image asynchronous.
        /// </summary>
        /// <param name="eventId">The event identifier.</param>
        /// <returns>
        /// Returns created image
        /// </returns>
        /// <exception cref="HttpResponseException"></exception>
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

        #endregion Methods

        #region Model

        public class ImageModel
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>
            /// The identifier.
            /// </value>
            public Guid Id { get; set; }

            /// <summary>
            /// Gets or sets the event identifier.
            /// </summary>
            /// <value>
            /// The event identifier.
            /// </value>
            public Guid EventId { get; set; }

            /// <summary>
            /// Gets or sets the content.
            /// </summary>
            /// <value>
            /// The content.
            /// </value>
            public byte[] Content { get; set; }
        }

        #endregion Model
    }
}