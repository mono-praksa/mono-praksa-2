using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using GeoEvents.Model.Common;

namespace GeoEvents.WebAPI.App_Start
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<IEvent, EventsViewModel>().ReverseMap();
            CreateMap<IImage, ImagesViewModel>().ReverseMap();
        }
    }
}