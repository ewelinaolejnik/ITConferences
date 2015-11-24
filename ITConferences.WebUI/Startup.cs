using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ITConferences.Domain.Startup))]
namespace ITConferences.Domain
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
