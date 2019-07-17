namespace DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.DependencyInjection
{
    public interface IKernelContainer
    {
        ICoCoKernel Kernel { get; }
    }
}