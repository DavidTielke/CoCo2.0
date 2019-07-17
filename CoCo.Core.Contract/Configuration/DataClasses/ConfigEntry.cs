namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses
{
    public class ConfigEntry
    {
        public string Category { get; set; }
        public string Key { get; set; }
        public object Value { get; set; }
        public bool Persist { get; set; }
        public IConfigurationRepository Source { get; set; }
    }
}