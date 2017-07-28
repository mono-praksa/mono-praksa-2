using AutoMapper;
using GeoEvents.Model.Common;
using GeoEvents.WebAPI.Controllers;

namespace GeoEvents.WebAPI.App_Start
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<IEvent, EventController.EventModel>().ReverseMap();
            CreateMap<IImage, ImageController.ImageModel>().ReverseMap();
            CreateMap<ILocation, LocationController.LocationModel>().ReverseMap();
            CreateMap<GoogleMaps.Net.Clustering.Data.Geometry.MapPoint, EventController.MyMarker>();
        }
    }
}