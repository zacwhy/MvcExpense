using Zac.DesignPattern.UnitOfWork;
using Zac.StandardCore.Repositories;

namespace Zac.StandardCore
{
    public interface IStandardUnitOfWork : IUnitOfWork
    {
        IErrorLogRepository ErrorLogRepository { get; }
        ISiteMapNodeRepository SiteMapNodeRepository { get; }
        ISystemParameterRepository SystemParameterRepository { get; }
    }
}