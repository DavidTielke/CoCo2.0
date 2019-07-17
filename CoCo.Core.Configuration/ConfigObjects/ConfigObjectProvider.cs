using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.DynamicProxy;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.ConfigObjects
{
    public class ConfigObjectProvider : IConfigObjectProvider
    {
        private readonly IConfigurator _configurator;
        private readonly IDictionary<Type, object> _configObjects;
        private ProxyGenerator _proxyGenerator;

        public ConfigObjectProvider(IConfigurator configurator)
        {
            if (configurator == null) throw new ArgumentNullException(nameof(configurator));

            _configObjects = new ConcurrentDictionary<Type, object>();
            _configurator = configurator;

            _proxyGenerator = new ProxyGenerator();
        }

        public TConfig Get<TConfig>()
        {
            var configType = typeof(TConfig);

            ValidateType(configType);

            var configObj = GetConfigObject(configType);
            return (TConfig)configObj;
        }

        public object Get(Type configType)
        {
            if (configType == null) throw new ArgumentNullException(nameof(configType));

            ValidateType(configType);

            var configObj = GetConfigObject(configType);
            return configObj;
        }

        private void ValidateType(Type type)
        {
            var properties = type.GetProperties();

            var allAreVirtual = properties.All(p => p.GetMethod.IsVirtual);
            var allHaveAttributes = properties.All(p => p.GetCustomAttributes<ConfigMapAttribute>().Any());

            if (!allHaveAttributes || !allAreVirtual)
            {
                throw new InvalidOperationException("Requested type can only consist of virtual properties with ConfigMapAttribute");
            }
        }

        private object GetConfigObject(Type configType)
        {
            var alreadyGenerated = _configObjects.ContainsKey(configType);
            if (alreadyGenerated)
            {
                var obj = _configObjects[configType];
                return obj;
            }
            else
            {
                var obj = _proxyGenerator.CreateClassProxy(configType, ProxyGenerationOptions.Default,
                    new ConfigObjectInterceptor(_configurator));

                _configObjects[configType] = obj;

                return obj;
            }
        }
    }
}