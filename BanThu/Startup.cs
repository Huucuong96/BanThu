using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BanThu.Startup))]
namespace BanThu
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
