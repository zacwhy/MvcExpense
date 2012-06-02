using System.Data.Entity;
using Zac.DesignPattern;

namespace MvcExpense.DAL
{
    public class StandardRepository<TEntity> : GenericRepository<TEntity, long>, IStandardRepository<TEntity>
        where TEntity : class
    {
        public StandardRepository( DbContext context )
            : base( context )
        {
        }
    }
}