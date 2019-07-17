using System;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.DataClasses;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection
{
    public interface ICoCoKernel
    {
        void Register<TContract, TImplementation>(RegisterScope scope = RegisterScope.PerInject)
            where TImplementation : TContract;

        void Register(Type contract, Type implementation, RegisterScope scope = RegisterScope.PerInject);

        void RegisterToSelf<TImplementation>(RegisterScope scope = RegisterScope.PerInject);

        void RegisterComponent<TComponent>()
            where TComponent : IComponentActivator;

        TContract Get<TContract>();
        TContract Get<TContract>(params ConstructorParameter[] parameters);

        object Get(Type contractType);
        object Get(Type contractType, params ConstructorParameter[] parameters);

        void RegisterConfiguration<T>();
    }
}