using AutoMapper;
using GeoEvents.DAL;
using GeoEvents.Model.Common;

namespace GeoEvents.Model.Mapping
{
    public class ModelProfile : Profile
    {
        public ModelProfile()
        {
            CreateMap<EventEntity, IEvent>().ReverseMap();
            CreateMap<ImageEntity, IImage>().ReverseMap();
        }
    }
}