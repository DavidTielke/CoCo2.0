using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses;

namespace DavidTielke.PersonManagementApp.Logic.PersonManagement
{
    public class PersonManagementConfiguration
    {
        [ConfigMap("Persons","AgeThreshold")]
        public virtual int AgeThreshold { get; set; }
    }
}
