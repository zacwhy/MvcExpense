using System;

namespace Zac.DateRange
{
    public struct DateRange
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public static DateRange CreateDateRange( int? year, int? month, int? day, DateTime current )
        {
            DateRange dateRange = new DateRange();

            if ( !year.HasValue && !month.HasValue && !day.HasValue )
            {
                dateRange.StartDate = new DateTime( current.Year, current.Month, 1 );
                dateRange.EndDate = dateRange.StartDate.AddMonths( 1 );
                return dateRange;
            }

            if ( day.HasValue )
            {
                if ( !year.HasValue )
                {
                    year = current.Year;
                }

                if ( !month.HasValue )
                {
                    month = current.Month;
                }

                dateRange.StartDate = new DateTime( year.Value, month.Value, day.Value );
                dateRange.EndDate = dateRange.StartDate.AddDays( 1 );
                return dateRange;
            }

            if ( month.HasValue )
            {
                if ( !year.HasValue )
                {
                    year = current.Year;
                }

                dateRange.StartDate = new DateTime( year.Value, month.Value, 1 );
                dateRange.EndDate = dateRange.StartDate.AddMonths( 1 );
                return dateRange;
            }

            dateRange.StartDate = new DateTime( year.Value, 1, 1 );
            dateRange.EndDate = dateRange.StartDate.AddYears( 1 );
            return dateRange;
        }
    }
}