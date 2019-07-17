using System.Collections.Generic;
using System.IO;
using System.Linq;
using DavidTielke.PersonManagementApp.CrossCutting.CoCo.Core.Contract.EventBrokerage;
using DavidTielke.PersonManagementApp.CrossCutting.DataClasses;
using DavidTielke.PersonManagementApp.Data.DataStoring.Contract;
using DavidTielke.PersonManagementApp.Data.DataStoring.Contract.Messages;

namespace DavidTielke.PersonManagementApp.Data.DataStoring
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : IEntity
    {
        private readonly IEventBroker _eventBroker;
        private readonly DataStoringConfiguration _config;
        private List<TEntity> _entities;

        public Repository(IEventBroker eventBroker, DataStoringConfiguration config)
        {
            _eventBroker = eventBroker;
            _config = config;
            Load();
        }

        private void Load()
        {
            var path = Path.Combine(_config.RootPath, Filename);
            var fileAlreadyExist = File.Exists(path);
            if (!fileAlreadyExist)
            {
                _entities = new List<TEntity>();
            }
            else
            {
                var serializer = new CsvSerializer();
                _entities = serializer.Deserialize<TEntity>(path).ToList();
            }
        }

        private string Filename => typeof(TEntity).Name + ".csv";

        private int FindIndexOfEntity(TEntity entity) => _entities.FindIndex(e => e.Id == entity.Id);

        private void Save()
        {
            var serializer = new CsvSerializer();
            serializer.Serialize(_entities, Filename);
        }

        public void Add(TEntity entity)
        {
            int newId = _entities.Max(e => e.Id) + 1;
            entity.Id = newId;
            _entities.Add(entity);
            Save();

            _eventBroker.Raise(new EntityChangedMessage
            {
                Type = typeof(TEntity),
                ChangeType = ChangeType.Created,
                Entity = entity
            });
        }

        public void Update(TEntity entity)
        {
            var indexOfEntity = FindIndexOfEntity(entity);
            _entities[indexOfEntity] = entity;
            Save();

            _eventBroker.Raise(new EntityChangedMessage
            {
                Type = typeof(TEntity),
                ChangeType = ChangeType.Updated,
                Entity = entity
            });
        }

        public void Delete(TEntity entity)
        {
            var indexOfEntity = FindIndexOfEntity(entity);
            _entities.RemoveAt(indexOfEntity);
            Save();

            _eventBroker.Raise(new EntityChangedMessage
            {
                Type = typeof(TEntity),
                ChangeType = ChangeType.Removed,
                Entity = entity
            });
        }
        
        public IQueryable<TEntity> Query => _entities.AsQueryable();
    }
}