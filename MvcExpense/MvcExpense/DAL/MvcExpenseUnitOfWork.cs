using System.Data.Entity;
using MvcExpense.Models;
using Zac.DesignPattern;

namespace MvcExpense.DAL
{
    public class MvcExpenseUnitOfWork : UnitOfWorkBase, IMvcExpenseUnitOfWork
    {
        private readonly MvcExpenseDbContext _context;

        private IRepository<Category> _categoryRepository;
        private IRepository<Consumer> _consumerRepository;
        private IRepository<PaymentMethod> _paymentMethodRepository;
        private IOrdinaryExpenseRepository _ordinaryExpenseRepository;

        public MvcExpenseUnitOfWork()
        {
            _context = new MvcExpenseDbContext();
        }

        public MvcExpenseUnitOfWork( MvcExpenseDbContext context )
        {
            _context = context;
        }

        protected override DbContext Context
        {
            get
            {
                return _context;
            }
        }

        public MvcExpenseDbContext MvcExpenseDbContext
        {
            get
            {
                return _context;
            }
        }

        public IRepository<Category> CategoryRepository
        {
            get
            {
                if ( _categoryRepository == null )
                {
                    _categoryRepository = new GenericRepository<Category>( _context );
                }
                return _categoryRepository;
            }
        }

        public IRepository<Consumer> ConsumerRepository
        {
            get
            {
                if ( _consumerRepository == null )
                {
                    _consumerRepository = new GenericRepository<Consumer>( _context );
                }
                return _consumerRepository;
            }
        }

        public IRepository<PaymentMethod> PaymentMethodRepository
        {
            get
            {
                if ( _paymentMethodRepository == null )
                {
                    _paymentMethodRepository = new GenericRepository<PaymentMethod>( _context );
                }
                return _paymentMethodRepository;
            }
        }

        public IOrdinaryExpenseRepository OrdinaryExpenseRepository
        {
            get
            {
                if ( _ordinaryExpenseRepository == null )
                {
                    _ordinaryExpenseRepository = new OrdinaryExpenseRepository( _context );
                }
                return _ordinaryExpenseRepository;
            }
        }

    }
}