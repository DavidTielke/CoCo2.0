using DavidTielke.PersonManagementApp.CrossCutting.Auditation.Contract;
using DavidTielke.PersonManagementApp.Data.DataStoring.Contract.Messages;

namespace DavidTielke.PersonManagementApp.CrossCutting.AuditationWorkflows
{
    internal class EntityChangedAuditationWorkflow
    {
        private readonly IAuditor _auditor;

        public EntityChangedAuditationWorkflow(IAuditor auditor)
        {
            _auditor = auditor;
        }

        public void Process(EntityChangedMessage message)
        {
            _auditor.Log($"{message.Type.Name} changed: {message.ChangeType}");
        }
    }
}
