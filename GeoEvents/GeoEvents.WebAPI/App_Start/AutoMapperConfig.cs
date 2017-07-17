using AutoMapper;
using GeoEvents.Model.Mapping;

namespace GeoEvents.WebAPI.App_Start
{
    public class AutoMapperConfig : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IMapper>().ToConstant(Initialize());
        }

        private IMapper Initialize()
        {
            Mapper.Initialize(config =>
            {
                config.AddProfile<ModelProfile>();
                config.AddProfile<WebProfile>();
            });

            return Mapper.Instance;
        }
    }
}