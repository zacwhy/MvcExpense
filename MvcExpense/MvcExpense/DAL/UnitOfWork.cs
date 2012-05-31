using System;
using MvcExpense.Models;

namespace MvcExpense.DAL
{
    public class UnitOfWork : IDisposable
    {
        private zExpenseEntities context = new zExpenseEntities();
        private GenericRepository<Category> categoryRepository;
        private GenericRepository<OrdinaryExpense> ordinaryExpenseRepository;

        public GenericRepository<Category> CategoryRepository
        {
            get
            {
                if ( this.categoryRepository == null )
                {
                    this.categoryRepository = new GenericRepository<Category>( context );
                }
                return categoryRepository;
            }
        }

        public GenericRepository<OrdinaryExpense> OrdinaryExpenseRepository
        {
            get
            {
                if ( this.ordinaryExpenseRepository == null )
                {
                    this.ordinaryExpenseRepository = new GenericRepository<OrdinaryExpense>( context );
                }
                return ordinaryExpenseRepository;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose( bool disposing )
        {
            if ( !this.disposed )
            {
                if ( disposing )
                {
                    context.Dispose();
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