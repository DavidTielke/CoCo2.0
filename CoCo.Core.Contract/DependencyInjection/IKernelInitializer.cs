namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection
{
    public interface IKernelInitializer
    {
        void Initialize(ICoCoKernel kernel);
    }
}