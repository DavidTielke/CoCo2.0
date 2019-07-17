using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping
{
    public interface IBootstrapper
    {
        void ActivatingAll();
        void ActivatedAll();
        void DeactivatedAll();
        void DeactivatingAll();
        void RegisterAllMappings(ICoCoKernel kernel);
        void AddAllMessageSubscriptions(IEventBroker broker);
        void ConfigureAll(IConfigurator config);
    }
}
