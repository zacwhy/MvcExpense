using MvcExpense.Infrastructure.EntityFramework;

namespace MvcExpense.MvcExpenseHelper
{
    public class MvcExpenseFactory
    {
        public const string CurrentConnectionName = "zExpenseConnectionString";

        public static MvcExpenseUnitOfWork NewUnitOfWork()
        {
            return new MvcExpenseUnitOfWork( NewDbContext() );
        }

        public static MvcExpenseDbContext NewDbContext()
        {
            return new MvcExpenseDbContext( CurrentConnectionName );
        }
    }
}