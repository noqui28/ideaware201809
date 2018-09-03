using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PruebaIdeaware2018.Startup))]
namespace PruebaIdeaware2018
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
