using System;
using System.Linq;
using MvcExpense.Core.Models;
using MvcExpense.Core.Repositories;
using Zac.DateRange;
using Zac.DesignPattern.EntityFramework.Repositories;

namespace MvcExpense.Infrastructure.EntityFramework.Repositories
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

        public DateTime GetMostRecentDate()
        {
            IQueryable<OrdinaryExpense> query = GetQueryable();

            IQueryable<DateTime> query2 =
                from x in query
                where x.Id == query.Max( y => y.Id )
                select x.Date;

            DateTime date = query2.Single();

            return date;
        }

        public int NewSequence( DateTime date )
        {
            IQueryable<OrdinaryExpense> query = GetQueryable().Where( x => x.Date == date );
            int sequence = query.Max( x => (int?) x.Sequence ) ?? 0;
            return sequence + 1;
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