using System.Data.Entity;
using MvcExpense.Core.Models;
using Zac.StandardInfrastructure.EntityFramework;

namespace MvcExpense.Infrastructure.EntityFramework
{
    public class MvcExpenseDbContext : StandardDbContext
    {
        public MvcExpenseDbContext( string nameOrConnectionString )
            : base( nameOrConnectionString )
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Consumer> Consumers { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<OrdinaryExpense> OrdinaryExpenses { get; set; }
        public DbSet<SpecialExpense> SpecialExpenses { get; set; }
    }
}