using System;
using System.Collections.Generic;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Configuration.DatabaseSource
{
    public class DatabaseConfigurationRepository : IConfigurationRepository
    {
        public IEnumerable<ConfigEntry> Load() => new List<ConfigEntry>
        {
            new ConfigEntry{Category = "Persons", Key = "AgeThreshold", Value = 6},
            new ConfigEntry{Category = "DataStoring.Csv", Key = "FilePath", Value = "data.csv"},
        };

        public void Save(IEnumerable<ConfigEntry> entriesToStore)
        {
            throw new NotImplementedException();
        }

        public void SaveEntry(ConfigEntry entry)
        {
            throw new NotImplementedException();
        }
    }
}
