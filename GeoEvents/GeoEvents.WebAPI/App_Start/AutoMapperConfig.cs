using AutoMapper;
using GeoEvents.Model.Mapping;

namespace GeoEvents.WebAPI.App_Start
{
    public class AutoMapperConfig : Ninject.Modules.NinjectModule
    {
        /// <summary>
        /// Loads the module into the kernel.
        /// </summary>
        public override void Load()
        {
            Bind<IMapper>().ToConstant(Initialize());
        }

        /// <summary>
        /// Initializes AutoMapper
        /// </summary>
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