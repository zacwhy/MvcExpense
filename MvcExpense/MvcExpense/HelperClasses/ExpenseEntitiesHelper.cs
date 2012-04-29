using System;
using System.Linq;
using MvcExpense.Models;

namespace MvcExpense
{
    public static class ExpenseEntitiesHelper
    {
        public static DateTime GetMostRecentDate( zExpenseEntities db )
        {
            IQueryable<DateTime> query =
                from x in db.OrdinaryExpenses
                where x.Id == db.OrdinaryExpenses.Max( y => y.Id )
                select x.Date;
            DateTime mostRecentDate = query.Single();
            return mostRecentDate;
        }

        public static int NewSequence( zExpenseEntities db, DateTime date )
        {
            IQueryable<OrdinaryExpense> ordinaryExpenses = db.OrdinaryExpenses.Where( x => x.Date == date );
            int maxSequence = ordinaryExpenses.Max( x => (int?) x.Sequence ) ?? 0;
            return maxSequence + 1;
        }

    }
}