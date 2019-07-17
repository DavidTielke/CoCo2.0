using System;
using System.Runtime.Serialization;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration.Exceptions
{
    [Serializable]
    public class ConfigurationException : Exception
    {
        public ConfigurationException()
        {
        }

        public ConfigurationException(string message) : base(message)
        {
        }

        public ConfigurationException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ConfigurationException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
