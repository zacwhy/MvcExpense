using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Zac.DesignPattern
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        IQueryable<TEntity> GetQueryable();
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy,
            string includeProperties);

        TEntity GetById( TId id );
        void Insert( TEntity ordinaryExpense );
        void Delete( TId id );
        void Update( TEntity ordinaryExpense );
    }
}
