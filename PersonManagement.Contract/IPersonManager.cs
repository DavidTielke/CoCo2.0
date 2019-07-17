using System.Linq;
using DavidTielke.PersonManagementApp.CrossCutting.DataClasses;
using DavidTielke.PersonManagementApp.Logic.PersonManagement.Contract.DataClasses;

namespace DavidTielke.PersonManagementApp.Logic.PersonManagement.Contract
{
    public interface IPersonManager
    {
        IQueryable<Person> GetAllAdults();
        IQueryable<Person> GetAllChildren();
        AgeStatistic GetAgeStatistic();

        void Update(Person person);
        void Add(Person person);
        void Remove(Person person);
    }
}