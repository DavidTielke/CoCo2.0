using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Owin;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Owin.Mvc;
using DavidTielke.PersonManagementApp.Mappings;
using DavidTielke.PersonManagementApp.UI.AspWebClient;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace DavidTielke.PersonManagementApp.UI.AspWebClient
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var appDirectory = HttpRuntime.AppDomainAppPath;

            app.UseCoCoCore<KernelInitializer>()
                .UseCoCoCoreBootstrapper()
                .UseCoCoCoreEventBroker()
                .UseCoCoCoreConfiguration(config =>
                {
                    config.Set("Persons", "AgeThreshold", 18);
                    config.Set("DataStoring", "RootPath", appDirectory);
                })
                .UseMvcWithCoCo();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
