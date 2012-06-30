using System;

namespace Zac.DesignPattern.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        void Save();
    }
}
