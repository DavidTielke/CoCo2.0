using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Aspects;
using Ninject.Infrastructure.Language;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.NinjectAdapter.ExceptionMappingInterception
{
    public class ExceptionMapInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var interceptedType = invocation.TargetType;
            var interfaceWithMappingAttributes =
                interceptedType.GetInterfaces().FirstOrDefault(i => i.HasAttribute<MapExceptionAttribute>());
            if (interfaceWithMappingAttributes != null)
            {
                var attribute = interfaceWithMappingAttributes.GetCustomAttribute<MapExceptionAttribute>();
                var typeMessage = attribute.Message;
                var targetExceptionType = attribute.TargetException;

                try
                {
                    invocation.Proceed();
                }
                catch (Exception e) when (e.GetType() != targetExceptionType)
                {
                    var methodMessage = invocation.Method.GetCustomAttribute<ExceptionMessageAttribute>()?.Message;

                    if (methodMessage != null)
                    {
                        methodMessage = String.Format(methodMessage, invocation.Arguments);
                    }

                    var exceptionInstance = (Exception)Activator.CreateInstance(targetExceptionType, methodMessage ?? typeMessage, e);
                    throw exceptionInstance;
                }
            }
        }
    }
}
