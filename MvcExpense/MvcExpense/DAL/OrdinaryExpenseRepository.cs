using System.Linq;
using MvcExpense.Models;
using Zac.DateRange;

namespace MvcExpense.DAL
{
    public class OrdinaryExpenseRepository : StandardRepository<OrdinaryExpense>, IOrdinaryExpenseRepository
    {
        public OrdinaryExpenseRepository( MvcExpenseDbContext context )
            : base( context )
        {
        }

        public IQueryable<OrdinaryExpense> GetWithDateRange( DateRange dateRange )
        {
            IQueryable<OrdinaryExpense> query =
                from x in GetQueryable()
                where x.Date >= dateRange.StartDate && x.Date < dateRange.EndDate
                select x;

            return query;
        }

        //private StandardRepository<OrdinaryExpense> _internalStandardRepository;

        //public OrdinaryExpenseRepository( DbContext context )
        //{
        //    _internalStandardRepository = new StandardRepository<OrdinaryExpense>( context );
        //}

        //public IQueryable<OrdinaryExpense> GetWithDateRange( DateRange dateRange )
        //{
        //    IQueryable<OrdinaryExpense> query =
        //        from x in _internalStandardRepository.GetQueryable()
        //        where x.Date >= dateRange.StartDate && x.Date < dateRange.EndDate
        //        select x;

        //    return query;
        //}

    }
}