using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GooglePlacesExample.Startup))]
namespace GooglePlacesExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
