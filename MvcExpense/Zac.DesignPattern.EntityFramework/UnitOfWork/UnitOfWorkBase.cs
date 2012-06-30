using System;
using System.Data.Entity;

namespace Zac.DesignPattern.EntityFramework.UnitOfWork
{
    public abstract class UnitOfWorkBase : IDisposable
    {
        private readonly DbContext _context;

        protected UnitOfWorkBase( DbContext context )
        {
            _context = context;
        }

        protected DbContext Context
        {
            get
            {
                return _context;
            }
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose( bool disposing )
        {
            if ( !_disposed )
            {
                if ( disposing )
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }
    }
}