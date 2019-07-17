using DavidTielke.PersonManagementApp.CrossCutting.Auditation;
using DavidTielke.PersonManagementApp.CrossCutting.AuditationWorkflows;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection;
using DavidTielke.PersonManagementApp.Data.DataStoring;
using DavidTielke.PersonManagementApp.Logic.PersonManagement;

namespace DavidTielke.PersonManagementApp.Mappings
{
    public class KernelInitializer : IKernelInitializer
    {
        public void Initialize(ICoCoKernel kernel)
        {
            kernel.RegisterComponent<DataStoringActivator>();
            kernel.RegisterComponent<PersonManagementActivator>();
            kernel.RegisterComponent<AuditationActivator>();
            kernel.RegisterComponent<AuditationWorkflowsActivator>();
        }
    }
}
