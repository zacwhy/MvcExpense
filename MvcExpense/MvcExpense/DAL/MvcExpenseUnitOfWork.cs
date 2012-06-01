using System.Data.Entity;
using MvcExpense.Models;
using Zac.DesignPattern;

namespace MvcExpense.DAL
{
    public partial class MvcExpenseUnitOfWork : UnitOfWorkBase, IMvcExpenseUnitOfWork
    {
        private MvcExpenseDbContext context;// = new MvcExpenseDbContext();

        private GenericRepository<Category> categoryRepository;
        private GenericRepository<Consumer> consumerRepository;
        private GenericRepository<PaymentMethod> paymentMethodRepository;
        private OrdinaryExpenseRepository ordinaryExpenseRepository;

        public MvcExpenseUnitOfWork()
        {
            context = new MvcExpenseDbContext();
        }

        public MvcExpenseUnitOfWork( MvcExpenseDbContext context )
        {
            this.context = context;
        }

        protected override DbContext Context
        {
            get
            {
                return context;
            }
        }

        public MvcExpenseDbContext MvcExpenseDbContext
        {
            get
            {
                return context;
            }
        }

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

        public GenericRepository<Consumer> ConsumerRepository
        {
            get
            {
                if ( this.consumerRepository == null )
                {
                    this.consumerRepository = new GenericRepository<Consumer>( context );
                }
                return consumerRepository;
            }
        }

        public GenericRepository<PaymentMethod> PaymentMethodRepository
        {
            get
            {
                if ( this.paymentMethodRepository == null )
                {
                    this.paymentMethodRepository = new GenericRepository<PaymentMethod>( context );
                }
                return paymentMethodRepository;
            }
        }

        public OrdinaryExpenseRepository OrdinaryExpenseRepository
        {
            get
            {
                if ( this.ordinaryExpenseRepository == null )
                {
                    this.ordinaryExpenseRepository = new OrdinaryExpenseRepository( context );
                }
                return ordinaryExpenseRepository;
            }
        }

    }
}