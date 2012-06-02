using MvcExpense.Models;
using Zac.DesignPattern;

namespace MvcExpense.DAL
{
    public interface IMvcExpenseUnitOfWork : IUnitOfWork
    {
        IRepository<Category> CategoryRepository { get; }
        IRepository<Consumer> ConsumerRepository { get; }
        IRepository<PaymentMethod> PaymentMethodRepository { get; }
        IOrdinaryExpenseRepository OrdinaryExpenseRepository { get; }
    }
}