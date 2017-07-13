using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoEvents.Model.Common;
using GeoEvents.Repository.Common;
using GeoEvents.DAL;
using AutoMapper;

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
