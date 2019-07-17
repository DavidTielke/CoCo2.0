using System.Linq;
using DavidTielke.PersonManagementApp.CrossCutting.DataClasses;

namespace DavidTielke.PersonManagementApp.Data.DataStoring.Contract
{
    public interface IRepository<TEntity>
        where TEntity : IEntity
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> Query { get; }
    }
}
