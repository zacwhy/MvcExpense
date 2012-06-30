using MvcExpense.Core.Models;
using MvcExpense.Core.Repositories;
using Zac.DesignPattern.Repositories;
using Zac.StandardCore;

namespace MvcExpense.Core
{
    public interface IMvcExpenseUnitOfWork : IStandardUnitOfWork
    {
        IStandardRepository<Category> CategoryRepository { get; }
        IStandardRepository<Consumer> ConsumerRepository { get; }
        IStandardRepository<PaymentMethod> PaymentMethodRepository { get; }
        IOrdinaryExpenseRepository OrdinaryExpenseRepository { get; }
    }
}