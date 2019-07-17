using System;
using System.Linq;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Bootstrapping;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.Configuration;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection.DataClasses;
using Ninject.Parameters;
using Ninject.Syntax;
using Ninject;
using Ninject.Web.Common;

namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.NinjectAdapter
{
    internal class KernelAdapter : Contract.DependencyInjection.ICoCoKernel
    {
        private readonly IKernel _innerKernel;

        public KernelAdapter(IKernel innerKernel)
        {
            _innerKernel = innerKernel;
        }

        public void Register<TContract, TImplementation>(RegisterScope scope = RegisterScope.PerInject) 
            where TImplementation : TContract
        {
            var registration = _innerKernel.Bind<TContract>().To<TImplementation>();
            ApplyScope(registration, scope);
        }

        public void Register(Type contract, Type implementation, RegisterScope scope = RegisterScope.PerInject)
        {
            var registration = _innerKernel.Bind(contract).To(implementation);
            ApplyScope(registration, scope);
        }

        public void RegisterToSelf<TImplementation>(RegisterScope scope = RegisterScope.PerInject)
        {
            var registration = _innerKernel.Bind<TImplementation>().ToSelf();
            ApplyScope(registration, scope);
        }

        public void RegisterComponent<TComponent>() where TComponent : IComponentActivator
        {
            _innerKernel.Bind<IComponentActivator>().To<TComponent>();
        }

        public TContract Get<TContract>() => _innerKernel.Get<TContract>();

        public TContract Get<TContract>(params ConstructorParameter[] parameters)
        {
            var ninjectParameters = parameters.Select(p => new ConstructorArgument(p.Name, p.Value));
            var implementation = _innerKernel.Get<TContract>(ninjectParameters.ToArray());
            return implementation;
        }

        public object Get(Type contractType)
        {
            var implementation = _innerKernel.Get(contractType);
            return implementation;
        }

        public object Get(Type contractType, params ConstructorParameter[] parameters)
        {
            var ninjectParameters = parameters.Select(p => new ConstructorArgument(p.Name, p.Value));
            var implementation = _innerKernel.Get(contractType, ninjectParameters.ToArray());
            return implementation;
        }

        public void RegisterConfiguration<T>()
        {
            _innerKernel.Bind<T>().ToMethod(c => c.Kernel.Get<IConfigObjectProvider>().Get<T>());
        }

        private void ApplyScope<T>(IBindingWhenInNamedWithOrOnSyntax<T> registration, RegisterScope scope)
        {
            switch (scope)
            {
                case RegisterScope.PerInject:
                    registration.InTransientScope();
                    break;
                case RegisterScope.PerContext:
                    registration.InRequestScope();
                    break;
                case RegisterScope.Unique:
                    registration.InSingletonScope();
                    break;
            }
        }
    }
}