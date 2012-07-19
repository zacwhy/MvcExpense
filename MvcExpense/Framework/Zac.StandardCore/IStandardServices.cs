using System;

namespace Zac.StandardCore.Services
{
    public interface IStandardServices : IDisposable
    {
        IErrorLogService ErrorLogService { get; }
        ISiteMapNodeService SiteMapNodeService { get; }
    }
}
