using AutoMapper;
using GeoEvents.Model.Common;

namespace GeoEvents.WebAPI.App_Start
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<IEvent, EventModel>().ReverseMap();
            CreateMap<IImage, ImageModel>().ReverseMap();
        }
    }
}