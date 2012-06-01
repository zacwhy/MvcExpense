using System;
using System.Data.Entity;

namespace Zac.DesignPattern
{
    public abstract class UnitOfWorkBase : IDisposable
    {
        protected abstract DbContext Context { get; }

        public void Save()
        {
            Context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose( bool disposing )
        {
            if ( !this.disposed )
            {
                if ( disposing )
                {
                    Context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }
    }
}