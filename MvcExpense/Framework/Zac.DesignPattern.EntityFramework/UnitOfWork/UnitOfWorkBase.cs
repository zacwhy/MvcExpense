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

        protected EnhancedDbContext EnhancedDbContext
        {
            get
            {
                return _context;
            }
        }

        public void Save()
        {
            foreach ( SqlCommand sqlCommand in EnhancedDbContext.SqlCommandsBeforeSaveChanges )
            {
                object[] parameters = UnitOfWorkBaseHelper.CopyParameters( sqlCommand.Parameters ).ToArray();
                EnhancedDbContext.Database.ExecuteSqlCommand( sqlCommand.CommandText, parameters );
            }

            EnhancedDbContext.SaveChanges();
        }

        private bool _disposed;

        protected virtual void Dispose( bool disposing )
        {
            if ( !_disposed )
            {
                if ( disposing )
                {
                    EnhancedDbContext.Dispose();
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