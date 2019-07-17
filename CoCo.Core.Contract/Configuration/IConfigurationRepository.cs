using System.Collections.Generic;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration
{
    public interface IConfigurationRepository
    {
        IEnumerable<ConfigEntry> Load();
        void Save(IEnumerable<ConfigEntry> entriesToStore);
        void SaveEntry(ConfigEntry entry);
    }
}