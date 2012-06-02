using System.Linq;
using MvcExpense.Models;
using Zac.DateRange;

namespace MvcExpense.DAL
{
    public interface IOrdinaryExpenseRepository : IStandardRepository<OrdinaryExpense>
    {
        IQueryable<OrdinaryExpense> GetWithDateRange( DateRange dateRange );
    }
}