using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping
{
    public interface IComponentActivator
    {
        void Activating();
        void Activated();
        void Deactivating();
        void Deactivated();
        void RegisterMappings(ICoCoKernel kernel);
        void AddMessageSubscriptions(IEventBroker broker);
        void Configure(IConfigurator config);
    }
}