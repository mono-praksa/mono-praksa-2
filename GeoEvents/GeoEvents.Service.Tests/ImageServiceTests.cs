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
using AutoMapper;
using GeoEvents.Repository.Common;
using GeoEvents.Model.Common;

namespace GeoEvents.Service.Tests
{
    public class ImageServiceTests
    {
        public ImageServiceTests()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<ModelProfile>();
            });
        }

        [Theory]
        [InlineData("12345678-1234-1234-1234-123456789000", false)]
        [InlineData("01234567-0123-0123-0123-012345678910", true)]
        public void CreateImagesReturnsFalseIfImageIdExists(string id, bool expected)
        {
            var mockImageRepository = new Mock<IImageRepository>();
            var img = new Image { Id = new Guid(id), EventId = new Guid("11112222-3333-4444-5555-666677778888"), Content = new byte[10]};
            var imgs = new List<IImage>();
            imgs.Add(img);
            var db = new List<IImageEntity>();
            db.Add(Mapper.Map<IImageEntity>(new Image { Id = new Guid("12345678-1234-1234-1234-123456789000"), Content = new byte[20], EventId = new Guid("11112222-3333-4444-5555-666677778888")}));

            mockImageRepository
                .Setup(ir => ir.CreateImages(It.IsAny<List<IImageEntity>>()))
                .Returns<List<IImageEntity>>(images => !images.Exists(image => db.Exists(ie => ie.Id == image.Id)));
            var imageService = new ImageService(mockImageRepository.Object);
            var result = imageService.CreateImages(imgs);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetImagesReturnsCorrectImages()
        {
            var mockImageRepository = new Mock<IImageRepository>();
            var img1 = new Image { Id = new Guid("12345678-1234-1234-1234-123456789000"), Content = new byte[] { 0x20, 0x20, 0x20, 0x20 }, EventId = new Guid("11112222-3333-4444-5555-666677778888")};
            var img2 = new Image { Id = new Guid("02345678-1234-1234-1234-123456789000"), Content = new byte[] { 0x21, 0x21, 0x21, 0x21 }, EventId = new Guid("11112222-3333-4444-5555-666677778888")};
            var img3 = new Image { Id = new Guid("32345678-1234-1234-1234-123456789000"), Content = new byte[] { 0x22, 0x22, 0x22, 0x22 }, EventId = new Guid("01112222-3333-4444-5555-666677778888")};
            var db = new List<IImageEntity>();
            db.Add(Mapper.Map<IImageEntity>(img1));
            db.Add(Mapper.Map<IImageEntity>(img2));
            db.Add(Mapper.Map<IImageEntity>(img3));

            mockImageRepository
                .Setup(ir => ir.GetImages(It.IsAny<Guid>()))
                .Returns<Guid>(id => (from img in db where img.EventId == id select img).ToList());
            var imageService = new ImageService(mockImageRepository.Object);
            var result1 = imageService.GetImages(new Guid("11112222-3333-4444-5555-666677778888"));
            var result2 = imageService.GetImages(new Guid("22221111-3333-4444-5555-666677778888"));
            var expected1 = new List<IImage>() { Mapper.Map<IImage>(db[0]), Mapper.Map<IImage>(db[1]) };
            var expected2 = new List<IImage>();
            Assert.Equal(expected1[0].Content, result1[0].Content);
            Assert.Equal(expected1[1].Content, result1[1].Content);
            Assert.Equal(expected2, result2);
        }
    }
}
