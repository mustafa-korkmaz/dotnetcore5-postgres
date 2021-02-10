using System.Collections.Generic;

namespace dotnetpostgres.Dal.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(object id);
        IEnumerable<TEntity> GetAll();
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
