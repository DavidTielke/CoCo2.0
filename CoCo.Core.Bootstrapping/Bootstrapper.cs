using System.Collections.Generic;
using System.Linq;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Bootstrapping
{
    public class Bootstrapper : IBootstrapper
    {
        private readonly List<IComponentActivator> _components;

        public Bootstrapper(IComponentActivator[] components)
        {
            _components = components.ToList();
        }

        public void ActivatingAll() => _components.ForEach(ca => ca.Activating());
        public void ActivatedAll() => _components.ForEach(ca => ca.Activated());
        public void DeactivatedAll() => _components.ForEach(ca => ca.Deactivated());
        public void DeactivatingAll() => _components.ForEach(ca => ca.Deactivating());
        public void RegisterAllMappings(ICoCoKernel kernel) => _components.ForEach(ca => ca.RegisterMappings(kernel));
        public void AddAllMessageSubscriptions(IEventBroker broker) =>
            _components.ForEach(ca => ca.AddMessageSubscriptions(broker));
        public void ConfigureAll(IConfigurator config) => _components.ForEach(ca => ca.Configure(config));
    }
}
