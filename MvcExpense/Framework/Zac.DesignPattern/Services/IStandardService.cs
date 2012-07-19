
namespace Zac.DesignPattern.Services
{
    public interface IStandardService<TEntity> : IService<TEntity, long> where TEntity : class
    {
    }
}
