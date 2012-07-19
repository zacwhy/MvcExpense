using System.Collections.Generic;

namespace Zac.DesignPattern.Services
{
    public interface IService<TEntity, TId> where TEntity : class
    {
        IEnumerable<TEntity> FindAll();
        TEntity FindById( TId id );
        void Save( TEntity entity );
    }
}
