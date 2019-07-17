using DavidTielke.PersonManagementApp.CrossCutting.Auditation.Contract;
using DavidTielke.PersonManagementApp.Logic.PersonManagement.Contract.Messages;

namespace DavidTielke.PersonManagementApp.CrossCutting.AuditationWorkflows
{
    class PersonLoadedAuditationWorkflow
    {
        private readonly IAuditor _auditor;

        public PersonLoadedAuditationWorkflow(IAuditor auditor)
        {
            _auditor = auditor;
        }

        public void Process(PersonLoadedMessage msg)
        {
            _auditor.Log("Persons are loaded...");
        }
    }
}
