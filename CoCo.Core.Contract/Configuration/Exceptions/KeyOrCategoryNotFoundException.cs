using System;
using System.Runtime.Serialization;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.Exceptions
{
    [Serializable]
    public class KeyOrCategoryNoException : ConfigurationException
    {
        public KeyOrCategoryNoException()
        {
        }

        public KeyOrCategoryNoException(string category, string key)
            : this($"No config entry found for category: {category} and/or key: {key}")
        {
            
        }

        public KeyOrCategoryNoException(string message) : base(message)
        {
        }

        public KeyOrCategoryNoException(string message, Exception inner) : base(message, inner)
        {
        }

        protected KeyOrCategoryNoException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
