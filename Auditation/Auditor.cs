using System;
using DavidTielke.PersonManagementApp.CrossCutting.Auditation.Contract;

namespace DavidTielke.PersonManagementApp.CrossCutting.Auditation
{
    class Auditor : IAuditor
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
