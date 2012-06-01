using MvcExpense.Models;
using Zac.DesignPattern;

namespace MvcExpense.DAL
{
    public interface IMvcExpenseUnitOfWork : IUnitOfWork
    {
        MvcExpenseDbContext MvcExpenseDbContext { get; }

        GenericRepository<Category> CategoryRepository { get; }
        GenericRepository<Consumer> ConsumerRepository { get; }
        GenericRepository<PaymentMethod> PaymentMethodRepository { get; }
        OrdinaryExpenseRepository OrdinaryExpenseRepository { get; }
    }
}