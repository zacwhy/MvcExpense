using System;
using System.Linq;
using MvcExpense.Core.Models;
using Zac.DateRange;
using Zac.DesignPattern.Repositories;

namespace MvcExpense.Core.Repositories
{
    public interface IOrdinaryExpenseRepository : IStandardRepository<OrdinaryExpense>
    {
        IQueryable<OrdinaryExpense> GetWithDateRange( DateRange dateRange );
        DateTime GetMostRecentDate();
        int NewSequence( DateTime date );
    }
}