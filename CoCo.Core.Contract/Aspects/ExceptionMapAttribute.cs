using System;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Aspects
{
    [AttributeUsage(AttributeTargets.Interface, Inherited = true)]
    public sealed class MapExceptionAttribute : Attribute
    {
        public Type TargetException { get; }
        public string Message { get; }

        public MapExceptionAttribute(Type targetException, string message = null)
        {
            TargetException = targetException;
            Message = message;
        }
    }
}
