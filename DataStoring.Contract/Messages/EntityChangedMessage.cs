using System;

namespace DavidTielke.PersonManagementApp.Data.DataStoring.Contract.Messages
{
    public class EntityChangedMessage
    {
        public Type Type { get; set; }
        public ChangeType ChangeType { get; set; }
        public object Entity { get; set; }
    }
}
