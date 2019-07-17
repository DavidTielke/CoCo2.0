using System;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.ConfigObjects;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.DatabaseSource;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.DataClasses;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.EventBrokerage;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.NinjectAdapter;
using Owin;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Owin
{
    public static class AppBuilderExtensions
    {
        public static ICoCoKernel Kernel { get; private set; }

        private static bool _isKernelInitialized = false;
        private static bool _isBootstrapperInitialized = false;
        private static bool _isEventBrokerInitialized = false;
        private static bool _isConfigurationInitialized = false;

        public static IAppBuilder UseCoCoCore<TKernelInitializer>(this IAppBuilder source, Action<ICoCoKernel> localKernelInitializer = null)
            where TKernelInitializer : IKernelInitializer, new()
        {
            // Framework can't be activated twice
            if (_isKernelInitialized)
            {
                throw new InvalidOperationException("CoCo Framework is already initialized and activated");
            }

            // NOTE: Here you can change the implementation of dependency injection
            Kernel = new KernelContainer().Kernel;

            // Add all app specific contract mappings on startup
            localKernelInitializer?.Invoke(Kernel);

            // Add all component contract mappings
            var kernelInitializer = Activator.CreateInstance<TKernelInitializer>();
            kernelInitializer.Initialize(Kernel);

            _isKernelInitialized = true;

            return source;
        }

        public static IAppBuilder UseCoCoCoreBootstrapper(this IAppBuilder source)
        {
            // Activating bootstrapper requires the existence of a configured kernel for contract mappings.
            if (!_isKernelInitialized)
            {
                throw new InvalidOperationException($"Can't use bootstrapper without activated CoCo Framework. Call {nameof(UseCoCoCore)}() first.");
            }

            // Cant activate bootstrapper twice (double contract bindings)
            if (_isBootstrapperInitialized)
            {
                throw new InvalidOperationException($"Bootstrapper was already activated. Can't activate twice. ");
            }

            Kernel.Register<IBootstrapper, Bootstrapper>(RegisterScope.Unique);

            // Activate all known components and initialize their contract mappings in kernel
            var bootstrapper = Kernel.Get<IBootstrapper>();
            bootstrapper.ActivatedAll();
            bootstrapper.RegisterAllMappings(Kernel);

            _isBootstrapperInitialized = true;

            return source;
        }

        /// <summary>
        /// Activates the usage of Event Broker
        /// </summary>
        public static IAppBuilder UseCoCoCoreEventBroker(this IAppBuilder source)
        {
            // Activating event broker requires the existence of a configured DI kernel
            if (!_isKernelInitialized)
            {
                throw new InvalidOperationException($"Can't use event broker without activated CoCo Framework. Call {nameof(UseCoCoCore)}() first.");
            }

            // Event broker needs a activated bootstrapper to load all message subscriptions
            if (!_isBootstrapperInitialized)
            {
                throw new InvalidOperationException($"Can't use event broker without activated bootstrapper. Call {nameof(UseCoCoCoreBootstrapper)}() first.");
            }

            // The event broker cant be initialized twice (double contract bindings)
            if (_isEventBrokerInitialized)
            {
                throw new InvalidOperationException($"Event broker was already activated. Can't activate twice.");
            }

            // Registers the IEventBroker contract as a singleton.
            // NOTE: Here your can change the implementation to your custom one.
            Kernel.Register<IEventBroker, EventBroker>(RegisterScope.Unique);

            // Setting the Resolver, when the eventbroker needs ti create an instance.
            var eventBroker = Kernel.Get<IEventBroker>();
            eventBroker.SetResolverCallback(t => Kernel.Get(t));

            // Ask all registered Component Implementations for message subscriptions.
            var bootstrapper = Kernel.Get<IBootstrapper>();
            bootstrapper.AddAllMessageSubscriptions(eventBroker);

            // Mark event broker as activated
            _isEventBrokerInitialized = true;

            return source;
        }

        /// <summary>
        /// Activates the usage of the Configuration
        /// </summary>
        /// <param name="configInitializer">Delegate to set initial config values (Per Request).</param>
        /// <returns></returns>
        public static IAppBuilder UseCoCoCoreConfiguration(this IAppBuilder source, Action<IConfigurator> configInitializer = null)
        {
            // Registers all nessesary Contracts for the configuration framework.
            // NOTE: Here you can change the implementation to your custom one.
            Kernel.Register<IConfigurationRepository, DatabaseConfigurationRepository>();
            Kernel.Register<IConfigObjectProvider, ConfigObjectProvider>();
            Kernel.Register<IConfigurator, Configurator>(RegisterScope.PerContext);

            // If the application wants to set initial config values
            if (configInitializer != null)
            {
                // Create and append new middleware to OWIN
                // to call the apps custom configuration on every request
                source.Use(new Func<AppFunc, AppFunc>(next => (async env =>
                {
                    var config = Kernel.Get<IConfigurator>();
                    configInitializer.Invoke(config);
                    await next.Invoke(env);
                })));
            }

            //Mark configuration as initialized
            _isConfigurationInitialized = true;

            return source;
        }
    }
}
