using System;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.DataClasses
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ConfigMapAttribute : Attribute
    {
        public string Category { get; }
        public string Key { get; }
        public bool Persist { get; }

        public ConfigMapAttribute(string category, 
            string key, 
            bool persist = false)
        {
            Category = category;
            Key = key;
            Persist = persist;
        }
    }
}
