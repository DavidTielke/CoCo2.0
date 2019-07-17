using System;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.ConfigObjects
{
    public class ConfigObjectInterceptor : IInterceptor
    {
        private readonly IConfigurator _config;

        public ConfigObjectInterceptor(IConfigurator config)
        {
            if(config == null) throw new ArgumentNullException(nameof(config));
            _config = config;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.Method;

            var isSetter = method.Name.StartsWith("set_");
            var isGetter = method.Name.StartsWith("get_");

            var isProperty = isSetter || isGetter;
            if (!isProperty)
            {
                invocation.Proceed();
            }

            var propertyName = method.Name.Split('_')[1];
            var originalType = invocation.TargetType;

            var propertyInfo = originalType.GetProperties().Single(p => p.Name == propertyName);
            var attribute = propertyInfo.GetCustomAttribute<ConfigMapAttribute>();

            var hasAttribute = attribute != null;
            if (!hasAttribute)
            {
                return;
            }

            var category = attribute.Category;
            var key = attribute.Key;
            var persist = attribute.Persist;

            if (isGetter)
            {
                var value = _config.Get<object>(category, key);
                invocation.ReturnValue = value;
            }

            if (isSetter)
            {
                var value = invocation.Arguments[0];
                _config.Set(category, key, value, persist: persist);
            }
        }
    }
}
