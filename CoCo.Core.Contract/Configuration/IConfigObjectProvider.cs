using System;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration
{
    public interface IConfigObjectProvider
    {
        TConfig Get<TConfig>();
        object Get(Type configType);
    }
}