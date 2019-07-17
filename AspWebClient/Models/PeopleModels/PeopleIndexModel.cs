using System.Linq;
using DavidTielke.PersonManagementApp.CrossCutting.DataClasses;

namespace DavidTielke.PersonManagementApp.UI.AspWebClient.Models.PeopleModels
{
    public class PeopleIndexModel
    {
        public IQueryable<Person> Adults { get; set; }
        public IQueryable<Person> Children { get; set; }
    }
}