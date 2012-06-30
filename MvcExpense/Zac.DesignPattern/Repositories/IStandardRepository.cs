namespace Zac.DesignPattern.Repositories
{
    public interface IStandardRepository<TEntity> : IRepository<TEntity, long> where TEntity : class
    {
    }
}