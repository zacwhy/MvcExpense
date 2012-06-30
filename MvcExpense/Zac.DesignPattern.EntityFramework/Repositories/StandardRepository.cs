using System.Data.Entity;
using Zac.DesignPattern.Repositories;

namespace Zac.DesignPattern.EntityFramework.Repositories
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