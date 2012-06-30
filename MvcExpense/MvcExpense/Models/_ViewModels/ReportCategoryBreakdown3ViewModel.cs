using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcExpense.ViewModels
{
    public class ReportCategoryBreakdown3ViewModel
    {
        //public DataTable DataTable
        //{
        //    get;
        //    set;
        //}

        //public IList<string> Periods
        //{
        //    get;
        //    set;
        //}

        //private IList<CategoryBreakdownRecord3> _records;
        //public IList<CategoryBreakdownRecord3> Records
        //{
        //    get
        //    {
        //        if ( _records == null )
        //        {
        //            _records = new List<CategoryBreakdownRecord3>();
        //        }
        //        return _records;
        //    }
        //    set { _records = value; } // todo remove temp
        //}

        //public CategoryBreakdownRecord3 Denominator2
        //{
        //    get
        //    {
        //        return Records.Where( x => x.Category == "Breakfast" ).Single();
        //    }
        //}

        // -----

        public IDictionary<string, CategoryBreakdownRecord4> Dictionary { get; set; }

        public IList<string> Periods
        {
            get
            {
                return Dictionary["Breakfast"].TotalPrices.Select( x => x.Key ).ToList();
            }
        }

        public CategoryBreakdownRecord4 Denominator
        {
            get
            {
                var denominator = new CategoryBreakdownRecord4();

                foreach ( var item in Dictionary )
                {
                    string category = item.Key;
                    CategoryBreakdownRecord4 record = item.Value;


                }
                //denominator.TotalPrices[

                return Dictionary["Breakfast"];
            }
        }
    }

    public class CategoryBreakdownRecord4
    {
        public string CssClass { get; set; }
        public IDictionary<string, double> TotalPrices { get; set; }
    }

    //public class CategoryBreakdownRecord3
    //{
    //    public string Category
    //    {
    //        get;
    //        set;
    //    }

    //    public CategoryBreakdownRecord3 Parent
    //    {
    //        get;
    //        set;
    //    }

    //    public IList<double> Values
    //    {
    //        get;
    //        set;
    //    }

    //    public string CssClass
    //    {
    //        get
    //        {
    //            if ( Parent == null )
    //            {
    //                return string.Empty;
    //            }
    //            return "child-of-" + Parent.Category;
    //        }
    //    }

    //    public CategoryBreakdownRecord3( string category )
    //        : this( category, null )
    //    {

    //    }

    //    public CategoryBreakdownRecord3( string category, CategoryBreakdownRecord3 parent )
    //    {
    //        Category = category;
    //        Parent = parent;
    //    }

    //    public override string ToString()
    //    {
    //        var s = new StringBuilder( Category );
    //        s.Append( " { " );
    //        foreach ( double value in Values )
    //        {
    //            s.AppendFormat( "{0} ", value );
    //        }
    //        s.Append( "} " );
    //        return s.ToString();
    //    }
    //}
}