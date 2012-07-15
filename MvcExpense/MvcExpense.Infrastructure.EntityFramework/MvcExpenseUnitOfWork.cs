using MvcExpense.Core;
using MvcExpense.Core.Models;
using MvcExpense.Core.Repositories;
using MvcExpense.Infrastructure.EntityFramework.Repositories;
using Zac.DesignPattern.EntityFramework.Repositories;
using Zac.DesignPattern.Repositories;
using Zac.StandardInfrastructure.EntityFramework;

namespace MvcExpense.Infrastructure.EntityFramework
{
    public class MvcExpenseUnitOfWork : StandardUnitOfWork, IMvcExpenseUnitOfWork
    {
        public MvcExpenseUnitOfWork( MvcExpenseDbContext context )
            : base( context )
        {
        }

        protected MvcExpenseDbContext MvcExpenseDbContext
        {
            get { return (MvcExpenseDbContext) StandardDbContext; }
        }

        private IStandardRepository<Category> _categoryRepository;
        public IStandardRepository<Category> CategoryRepository
        {
            get { return _categoryRepository ?? ( _categoryRepository = new StandardRepository<Category>( MvcExpenseDbContext ) ); }
        }

        private IStandardRepository<Consumer> _consumerRepository;
        public IStandardRepository<Consumer> ConsumerRepository
        {
            get { return _consumerRepository ?? ( _consumerRepository = new StandardRepository<Consumer>( MvcExpenseDbContext ) ); }
        }

        private IStandardRepository<PaymentMethod> _paymentMethodRepository;
        public IStandardRepository<PaymentMethod> PaymentMethodRepository
        {
            get {
                return _paymentMethodRepository ??
                       ( _paymentMethodRepository = new StandardRepository<PaymentMethod>( MvcExpenseDbContext ) );
            }
        }

        private IOrdinaryExpenseRepository _ordinaryExpenseRepository;
        public IOrdinaryExpenseRepository OrdinaryExpenseRepository
        {
            get {
                return _ordinaryExpenseRepository ??
                       ( _ordinaryExpenseRepository = new OrdinaryExpenseRepository( MvcExpenseDbContext ) );
            }
        }

    }
}