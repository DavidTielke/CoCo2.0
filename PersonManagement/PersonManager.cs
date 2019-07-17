using System.Linq;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage;
using DavidTielke.PersonManagementApp.CrossCutting.DataClasses;
using DavidTielke.PersonManagementApp.Data.DataStoring.Contract;
using DavidTielke.PersonManagementApp.Logic.PersonManagement.Contract;
using DavidTielke.PersonManagementApp.Logic.PersonManagement.Contract.DataClasses;
using DavidTielke.PersonManagementApp.Logic.PersonManagement.Contract.Messages;

namespace DavidTielke.PersonManagementApp.Logic.PersonManagement
{
    class PersonManager : IPersonManager
    {
        private readonly IEventBroker _eventBroker;
        private readonly IRepository<Person> _repository;
        private readonly PersonManagementConfiguration _config;

        public PersonManager(IEventBroker eventBroker, 
            IRepository<Person> repository, 
            PersonManagementConfiguration config)
        {
            _eventBroker = eventBroker;
            _repository = repository;
            _config = config;
        }

        public IQueryable<Person> GetAllAdults()
        {
            var configAgeThreshold = _config.AgeThreshold;
            _eventBroker.Raise(new PersonLoadedMessage());
            return _repository.Query.Where(p => p.Age >= configAgeThreshold);
        }

        public IQueryable<Person> GetAllChildren()
        {
            var configAgeThreshold = _config.AgeThreshold;
            _eventBroker.Raise(new PersonLoadedMessage());
            return _repository.Query.Where(p => p.Age < configAgeThreshold);
        }

        public AgeStatistic GetAgeStatistic() => new AgeStatistic
        {
            AmountAdults = GetAllAdults().Count(),
            AmountChildren = GetAllChildren().Count()
        };

        public void Update(Person person) => _repository.Update(person);

        public void Add(Person person) => _repository.Add(person);

        public void Remove(Person person) => _repository.Delete(person);
    }
}
