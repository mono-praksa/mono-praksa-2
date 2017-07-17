using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GeoEvents.WebAPI.Startup))]

namespace GeoEvents.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}