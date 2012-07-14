using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MvcExpense.Core.Models;
using MvcExpense.HelperModels;
using MvcExpense.Infrastructure.EntityFramework;
using MvcExpense.MvcExpenseHelper;
using MvcExpense.ViewModels;

//using OrdinaryExpense = MvcExpense.Core.Models.OrdinaryExpense;

namespace MvcExpense.UI.Controllers
{
    public partial class ReportController : Controller
    {
        private MvcExpenseDbContext db = MvcExpenseFactory.NewDbContext();// new MvcExpenseDbContext();

        public virtual ViewResult Index()
        {
            DateTime beginOfCurrentYear = new DateTime( DateTime.Now.Year, 1, 1 );

            var currentYearOrdinaryExpenses = db.OrdinaryExpenses.Where( x => x.Date >= beginOfCurrentYear );

            var ordinaryexpenses = currentYearOrdinaryExpenses
                //.OrderByDescending( x => new { x.Date, x.Sequence } )
                .Include( o => o.Category )
                .Include( o => o.Consumer );

            var reportViewModel = new ReportViewModel();
            reportViewModel.OrdinaryExpenses = ordinaryexpenses.ToList();

            return View( reportViewModel );
        }

        public virtual ViewResult GiveAndTake()
        {
            string consumerNamesString = "i,m,p,r,x,s,other";
            string[] consumerNames = consumerNamesString.Split( ',' );

            var viewModel = new ReportGiveAndTakeViewModel();
            IList<OrdinaryExpense> allExpenses = db.OrdinaryExpenses.ToList();
            foreach ( string consumerName in consumerNames )
            {
                IEnumerable<OrdinaryExpense> expenses = allExpenses.Where( x => x.Consumer.Name == consumerName );
                if ( expenses.Count() > 0 )
                {
                    var record = new GiveAndTakeRecord( consumerName );
                    record.GiveAmount = expenses.Where( x => x.Price > 0 ).Sum( x => x.Price );
                    record.TakeAmount = 0 - expenses.Where( x => x.Price < 0 ).Sum( x => x.Price );
                    viewModel.Records.Add( record );
                }
            }
            return View( viewModel );
        }

        //IList<CategoryBreakdownRecord> GetCategoryBreakdownRecordList( List<OrdinaryExpense> expenses )
        //{
        //    var queryGroupByCategoryGroupByMonth =
        //        from e in expenses
        //        group e by e.Date.ToString( "yyyy MMM" ) into mg
        //        select new
        //        {
        //            Month = mg.Key,
        //            CategoryGroups =
        //                from x in mg
        //                group x by x.Category into cg
        //                select new
        //                {
        //                    CategoryName = cg.Key.Name,
        //                    TotalPrice = cg.Sum( x => x.Price )
        //                }
        //        };

        //    var categoryBreakdownRecordList = new List<CategoryBreakdownRecord>();
        //    foreach ( var item in queryGroupByCategoryGroupByMonth )
        //    {
        //        string month = item.Month;
        //        var categoryBreakdownRecord = new CategoryBreakdownRecord( month.ToString() );
        //        foreach ( var category in item.CategoryGroups )
        //        {
        //            string categoryName = category.CategoryName;
        //            double totalPrice = category.TotalPrice;

        //            DoSomething( categoryBreakdownRecord, categoryName, totalPrice );
        //        }
        //        categoryBreakdownRecordList.Add( categoryBreakdownRecord );
        //    }

        //    return categoryBreakdownRecordList;
        //}

        //void DoSomething( CategoryBreakdownRecord categoryBreakdownRecord, string categoryName, double totalPrice )
        //{
        //    switch ( categoryName )
        //    {
        //        case "Breakfast":
        //            categoryBreakdownRecord.Breakfast = totalPrice;
        //            break;
        //        case "Lunch":
        //            categoryBreakdownRecord.Lunch = totalPrice;
        //            break;
        //        case "Dinner":
        //            categoryBreakdownRecord.Dinner = totalPrice;
        //            break;
        //        case "OtherFood":
        //            categoryBreakdownRecord.OtherFood = totalPrice;
        //            break;

        //        case "Bus":
        //            categoryBreakdownRecord.Bus = totalPrice;
        //            break;
        //        case "Train":
        //            categoryBreakdownRecord.Train = totalPrice;
        //            break;
        //        case "Taxi":
        //            categoryBreakdownRecord.Taxi = totalPrice;
        //            break;

        //        case "Petrol":
        //            categoryBreakdownRecord.Petrol = totalPrice;
        //            break;
        //        case "Parking":
        //            categoryBreakdownRecord.Parking = totalPrice;
        //            break;
        //        case "Erp":
        //            categoryBreakdownRecord.Erp = totalPrice;
        //            break;
        //    }
        //}

        public virtual ViewResult CategoryBreakdown()
        {
            var startDate = new DateTime( DateTime.Now.Year, 1, 1 );
            List<OrdinaryExpense> expenses = db.OrdinaryExpenses.Where( x => x.Date >= startDate ).ToList();

            var query =
                from x in expenses
                group x by x.Category into cg
                select new
                {
                    Category = cg.Key.Name,
                    PeriodGroups =
                        from y in cg
                        group y by y.Date.ToString( "yyyy MMM" ) into pg
                        select new
                        {
                            Period = pg.Key,
                            TotalPrice = pg.Sum( x => x.Price )
                        }
                };

            var dictionary = new Dictionary<string, CategoryBreakdownRecord4>();
            foreach ( var item in query )
            {
                string category = item.Category;
                var record = new CategoryBreakdownRecord4();
                record.TotalPrices = new Dictionary<string, double>();
                foreach ( var period in item.PeriodGroups )
                {
                    record.TotalPrices[period.Period] = period.TotalPrice;
                }
                dictionary.Add( category, record );
            }

            var viewModel = new ReportCategoryBreakdown3ViewModel();


            //dictionary.Add( "Breakfast", new CategoryBreakdownRecord4() );
            ////dictionary["Breakfast"].CssClass = "";
            //dictionary["Breakfast"].TotalPrices = new Dictionary<string, double>();
            //dictionary["Breakfast"].TotalPrices["2012 Jan"] = 1;
            //dictionary["Breakfast"].TotalPrices["2012 Feb"] = 2;
            //dictionary["Breakfast"].TotalPrices["2012 Mar"] = 3;

            //dictionary.Add( "Lunch", new CategoryBreakdownRecord4() );
            ////dictionary["Lunch"].CssClass = "";
            //dictionary["Lunch"].TotalPrices = new Dictionary<string, double>();
            //dictionary["Lunch"].TotalPrices["2012 Jan"] = 12;
            //dictionary["Lunch"].TotalPrices["2012 Feb"] = 22;
            //dictionary["Lunch"].TotalPrices["2012 Mar"] = 32;

            viewModel.Dictionary = dictionary;

            return View( viewModel );
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}