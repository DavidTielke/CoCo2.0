using System;
using System.IO;
using System.Linq;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.ConfigObjects;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.DatabaseSource;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.DataClasses;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.EventBrokerage;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.NinjectAdapter;
using DavidTielke.PersonManagementApp.CrossCutting.DataClasses;
using DavidTielke.PersonManagementApp.Logic.PersonManagement.Contract;
using DavidTielke.PersonManagementApp.Mappings;

namespace DavidTielke.PersonManagementApp.UI.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            // ----- App-Init
            var kernelContainer = new KernelContainer();
            var kernel = kernelContainer.Kernel;

            //Register CoCo.Core
            //Add Bootstrapper
            kernel.Register<IBootstrapper, Bootstrapper>(RegisterScope.Unique);
            //Add EventBroker
            kernel.Register<IEventBroker, EventBroker>(RegisterScope.Unique);
            //Add Configuration
            kernel.Register<IConfigurationRepository, DatabaseConfigurationRepository>();
            kernel.Register<IConfigurator, Configurator>(RegisterScope.Unique);
            kernel.Register<IConfigObjectProvider, ConfigObjectProvider>();
            
            //Register components
            new KernelInitializer().Initialize(kernel);

            var currentDirectory = Directory.GetCurrentDirectory();
            var configurator = kernel.Get<IConfigurator>();
            configurator.Set("Persons", "AgeThreshold", 18);
            configurator.Set("DataStoring", "RootPath", currentDirectory);

            //Activate components
            var bootstrapper = kernel.Get<IBootstrapper>();
            bootstrapper.ActivatingAll();
            bootstrapper.ActivatedAll();
            bootstrapper.RegisterAllMappings(kernel);

            var eventBroker = kernel.Get<IEventBroker>();
            eventBroker.SetResolverCallback(t => kernel.Get(t));
            bootstrapper.AddAllMessageSubscriptions(eventBroker);

            // ----- Program
            var manager = kernel.Get<IPersonManager>();

            var adults = manager.GetAllAdults().ToList();
            var children = manager.GetAllChildren().ToList();

            var fuchsi = new Person
            {
                Name = "Fuchsi",
                Age = 17
            };
            manager.Add(fuchsi);

            fuchsi.Age = 10;
            manager.Update(fuchsi);

            manager.Remove(fuchsi);

            Console.WriteLine($"### Erwachsene ({adults.Count}) ###");
            foreach (var adult in adults)
            {
                Console.WriteLine(adult.Name);
            }

            Console.WriteLine($"### Kinder ({children.Count}) ###");
            foreach (var child in children)
            {
                Console.WriteLine(child.Name);
            }

            bootstrapper.DeactivatingAll();
            bootstrapper.DeactivatedAll();
        }
    }
}
