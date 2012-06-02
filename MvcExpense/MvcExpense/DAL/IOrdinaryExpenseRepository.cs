using System.Linq;
using MvcExpense.Models;
using Zac.DateRange;
using Zac.DesignPattern;

namespace MvcExpense.DAL
{
    public interface IOrdinaryExpenseRepository : IRepository<OrdinaryExpense>
    {
        IQueryable<OrdinaryExpense> GetWithDateRange( DateRange dateRange );
    }
}