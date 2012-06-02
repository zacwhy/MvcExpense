using Zac.DesignPattern;

namespace MvcExpense.DAL
{
    public interface IStandardRepository<TEntity> : IRepository<TEntity, long> where TEntity : class
    {
    }
}