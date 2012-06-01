using System;

namespace Zac.DesignPattern
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
