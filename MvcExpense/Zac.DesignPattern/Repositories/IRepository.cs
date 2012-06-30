using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Zac.DesignPattern.Repositories
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        IQueryable<TEntity> GetQueryable();
        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "" );

        TEntity GetById( TId id );
        void Insert( TEntity entity );
        void Delete( TId id );
        void Update( TEntity entity );
    }
}
