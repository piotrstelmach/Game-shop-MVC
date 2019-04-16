using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameShop.Startup))]
namespace GameShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
