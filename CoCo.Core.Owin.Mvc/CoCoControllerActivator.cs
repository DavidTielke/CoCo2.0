using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Owin.Mvc
{
    internal class CoCoControllerActivator : IControllerActivator
    {
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            var controller = AppBuilderExtensions.Kernel.Get(controllerType);
            return controller as IController;
        }

    }
}