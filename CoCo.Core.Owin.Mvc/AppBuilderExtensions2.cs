using System.Web.Mvc;
using Owin;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Owin.Mvc
{
    public static class AppBuilderExtensions2
    {
        public static IAppBuilder UseMvcWithCoCo(this IAppBuilder source)
        {
            ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory(new CoCoControllerActivator()));

            return source;
        }
    }
}
