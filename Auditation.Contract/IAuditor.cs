namespace DavidTielke.PersonManagementApp.CrossCutting.Auditation.Contract
{
    public interface IAuditor
    {
        void Log(string message);
    }
}