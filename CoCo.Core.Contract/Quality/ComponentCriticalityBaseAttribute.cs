using System;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Quality
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ComponentCriticalityBaseAttribute : Attribute
    {
    }
}
