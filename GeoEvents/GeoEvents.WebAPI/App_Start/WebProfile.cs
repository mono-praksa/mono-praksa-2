using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using GeoEvents.Model.Common;
using GeoEvents.WebAPI.Controllers;

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