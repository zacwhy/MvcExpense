using System.Data.Entity;
using MvcExpense.Models;
using Zac.DesignPattern;

namespace MvcExpense.DAL
{
    public class MvcExpenseUnitOfWork : UnitOfWorkBase, IMvcExpenseUnitOfWork
    {
        private readonly MvcExpenseDbContext _context;

        private IStandardRepository<Category> _categoryRepository;
        private IStandardRepository<Consumer> _consumerRepository;
        private IStandardRepository<PaymentMethod> _paymentMethodRepository;
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

        public IStandardRepository<Category> CategoryRepository
        {
            get
            {
                if ( _categoryRepository == null )
                {
                    _categoryRepository = new StandardRepository<Category>( _context );
                }
                return _categoryRepository;
            }
        }

        public IStandardRepository<Consumer> ConsumerRepository
        {
            get
            {
                if ( _consumerRepository == null )
                {
                    _consumerRepository = new StandardRepository<Consumer>( _context );
                }
                return _consumerRepository;
            }
        }

        public IStandardRepository<PaymentMethod> PaymentMethodRepository
        {
            get
            {
                if ( _paymentMethodRepository == null )
                {
                    _paymentMethodRepository = new StandardRepository<PaymentMethod>( _context );
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