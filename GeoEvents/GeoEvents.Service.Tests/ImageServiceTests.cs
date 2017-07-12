using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Common;
using GeoEvents.Model;
using GeoEvents.Model.Mapping;
using Xunit;
using Moq;
using GeoEvents.Repository.Common;
using GeoEvents.Model.Common;

namespace GeoEvents.Service.Tests
{
    public class ImageServiceTests
    {
        [Fact]
        public async Task CreateImageReturnsImage()
        {
            var mockImageRepository = new Mock<IImageRepository>();
            var img = new Image { Id = Guid.NewGuid(), EventId = Guid.NewGuid(), Content = new byte[] { 0x20, 0x20, 0x20 } };

            mockImageRepository
                .Setup(ir => ir.CreateImageAsync(img))
                .ReturnsAsync(img);

            var imageService = new ImageService(mockImageRepository.Object);
            var result = await imageService.CreateImageAsync(img);
            var expected = await new Task<IImage>(() => img);

            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetImagesReturnsImages()
        {
            var id = new Guid("11112222-3333-4444-5555-666677778888");

            var repoReturn = new List<IImage>();
            var img1 = new Image() { Id = new Guid(), EventId = new Guid("11112222-3333-4444-5555-666677778888"), Content = new byte[4] };
            var img2 = new Image() { Id = new Guid(), EventId = new Guid("11112222-3333-4444-5555-666677778888"), Content = new byte[4] };
            var img3 = new Image() { Id = new Guid(), EventId = new Guid("11112222-3333-4444-5555-666677778888"), Content = new byte[4] };

            repoReturn.Add(img1);
            repoReturn.Add(img2);
            repoReturn.Add(img3);

            var mockImageRepository = new Mock<IImageRepository>();
            mockImageRepository
                .Setup(ir => ir.GetImagesAsync(id))
                .ReturnsAsync(() => repoReturn);
            var imageService = new ImageService(mockImageRepository.Object);
            var result = await imageService.GetImagesAsync(id);
            var expected = await new Task<List<IImage>>(() => repoReturn);

            Assert.Equal(expected, result);
        }
    }
}
