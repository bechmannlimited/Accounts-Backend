using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Accounts_IOU.Startup))]
namespace Accounts_IOU
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
