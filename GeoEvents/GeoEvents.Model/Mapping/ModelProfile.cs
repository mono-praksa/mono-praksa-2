using AutoMapper;
using GeoEvents.DAL;
using GeoEvents.Model.Common;

namespace GeoEvents.Model.Mapping
{
    public class ModelProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelProfile"/> class.
        /// </summary>
        public ModelProfile()
        {
            CreateMap<EventEntity, IEvent>().ReverseMap();
            CreateMap<ImageEntity, IImage>().ReverseMap();
            CreateMap<LocationEntity, ILocation>().ReverseMap();
        }
    }
}