using System;
using System.Data.SqlClient;
using System.Linq;

namespace Zac.DesignPattern.EntityFramework.UnitOfWork
{
    public abstract class UnitOfWorkBase : IDisposable
    {
        private readonly EnhancedDbContext _context;

        protected UnitOfWorkBase( EnhancedDbContext context )
        {
            _context = context;
        }

        protected EnhancedDbContext Context
        {
            get
            {
                return _context;
            }
        }

        public void Save()
        {
            foreach ( SqlCommand sqlCommand in Context.SqlCommandsBeforeSaveChanges )
            {
                object[] parameters = UnitOfWorkBaseHelper.CopyParameters( sqlCommand.Parameters ).ToArray();
                Context.Database.ExecuteSqlCommand( sqlCommand.CommandText, parameters );
            }

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