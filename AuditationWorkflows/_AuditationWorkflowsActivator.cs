using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage;
using DavidTielke.PersonManagementApp.Data.DataStoring.Contract.Messages;
using DavidTielke.PersonManagementApp.Logic.PersonManagement.Contract.Messages;

namespace DavidTielke.PersonManagementApp.CrossCutting.AuditationWorkflows
{
    public class AuditationWorkflowsActivator : IComponentActivator
    {
        public void Activating()
        {
        }

        public void Activated()
        {
        }

        public void Deactivating()
        {
        }

        public void Deactivated()
        {
        }

        public void RegisterMappings(ICoCoKernel kernel)
        {
        }

        public void AddMessageSubscriptions(IEventBroker broker)
        {
            broker.Subscribe<EntityChangedAuditationWorkflow, EntityChangedMessage>((handler, msg) => handler.Process(msg));
            broker.Subscribe<PersonLoadedAuditationWorkflow, PersonLoadedMessage>((handler, msg) => handler.Process(msg));
        }

        public void Configure(IConfigurator config)
        {
        }
    }
}
