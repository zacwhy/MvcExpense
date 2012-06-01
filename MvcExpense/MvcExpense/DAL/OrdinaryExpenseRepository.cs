using System.Linq;
using MvcExpense.Models;
using Zac.DateRange;
using Zac.DesignPattern;

namespace MvcExpense.DAL
{
    public class OrdinaryExpenseRepository : GenericRepository<OrdinaryExpense>//, IOrdinaryExpenseRepository
    {
        public OrdinaryExpenseRepository( MvcExpenseDbContext context )
            : base( context )
        {
        }

        public IQueryable<OrdinaryExpense> GetWithDateRange( DateRange dateRange )
        {
            IQueryable<OrdinaryExpense> query =
                from x in DbSet
                where x.Date >= dateRange.StartDate && x.Date < dateRange.EndDate
                select x;

            return query;
        }

    }
}