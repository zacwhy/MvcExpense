using MvcExpense.Models;
using Zac.DesignPattern;

namespace MvcExpense.DAL
{
    public interface IMvcExpenseUnitOfWork : IUnitOfWork
    {
        IStandardRepository<Category> CategoryRepository { get; }
        IStandardRepository<Consumer> ConsumerRepository { get; }
        IStandardRepository<PaymentMethod> PaymentMethodRepository { get; }
        IOrdinaryExpenseRepository OrdinaryExpenseRepository { get; }
    }
}