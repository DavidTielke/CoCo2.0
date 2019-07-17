using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses;

namespace DavidTielke.PersonManagementApp.Data.DataStoring
{
    public class DataStoringConfiguration
    {
        [ConfigMap("DataStoring.Csv","FilePath")]
        public virtual string FilePath { get; set; }

        [ConfigMap("DataStoring", "RootPath")]
        public virtual string RootPath { get; set; }
    }
}
