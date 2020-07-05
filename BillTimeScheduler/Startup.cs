using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BillTimeScheduler.Startup))]
namespace BillTimeScheduler
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
