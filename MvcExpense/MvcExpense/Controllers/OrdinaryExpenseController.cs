using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MvcExpense.Common;
using MvcExpense.Models;
using MvcExpense.ViewModels;
using MvcSiteMapProvider;

namespace MvcExpense.Controllers
{
    public class OrdinaryExpenseController : Controller
    {
        private zExpenseEntities db = new zExpenseEntities();

        public ViewResult Index( int? year, int? month )
        {
            if ( !year.HasValue )
            {
                year = DateTime.Now.Year;
            }

            if ( !month.HasValue )
            {
                month = DateTime.Now.Month;
            }

            DateTime beginOfCurrentMonth = new DateTime( year.Value, month.Value, 1 );

            var ordinaryExpensesQuery1 = db.OrdinaryExpenses.Where( x => x.Date >= beginOfCurrentMonth );

            // if there are no records in current month, display records from most recent date
            if ( ordinaryExpensesQuery1.Count() == 0 )
            {
                ordinaryExpensesQuery1 =
                    from x in db.OrdinaryExpenses
                    where x.Date == db.OrdinaryExpenses.Max( y => y.Date )
                    select x;
            }

            var ordinaryExpensesQuery = ordinaryExpensesQuery1
                .OrderByDescending( x => new { x.Date, x.Sequence } )
                .Include( o => o.Category )
                .Include( o => o.Consumer );

            IList<OrdinaryExpense> ordinaryExpenses = ordinaryExpensesQuery.ToList();

            return View( ordinaryExpenses );
        }

        [AutoMap( typeof( OrdinaryExpense ), typeof( OrdinaryExpenseViewModel ) )]
        public ViewResult Details(long id)
        {
            OrdinaryExpense ordinaryexpense = db.OrdinaryExpenses.Find(id);
            return View(ordinaryexpense);
        }

        public ActionResult Create()
        {
            var createModel = new OrdinaryExpenseCreateModel();
            createModel.Date = ExpenseEntitiesHelper.GetMostRecentDate( db );
            createModel.SelectedConsumerIds = new long[] { 1 }; // todo remove hardcode
            PopulateCreateModel( createModel );
            return View( createModel );
        }

        [HttpPost]
        [MultiButton( MatchFormKey = "action", MatchFormValue = "Create" )]
        public ActionResult Create( OrdinaryExpenseCreateModel createModel )
        {
            return CreateAndRedirect( createModel, "Index" );
        }

        [HttpPost]
        [MultiButton( MatchFormKey = "action", MatchFormValue = "Create and New" )]
        public ActionResult CreateAndNew( OrdinaryExpenseCreateModel createModel )
        {
            return CreateAndRedirect( createModel, "Create" );
        }

        private ActionResult CreateAndRedirect( OrdinaryExpenseCreateModel createModel, string actionNameToRedirectTo )
        {
            if ( ModelState.IsValid )
            {
                OrdinaryExpense ordinaryExpense = Mapper.Map<OrdinaryExpenseCreateModel, OrdinaryExpense>( createModel );
                int sequence = ExpenseEntitiesHelper.NewSequence( db, ordinaryExpense.Date );
                int consumerCount = createModel.SelectedConsumerIds.Count();

                if ( consumerCount == 1 )
                {
                    ordinaryExpense.Sequence = sequence;
                    ordinaryExpense.ConsumerId = createModel.SelectedConsumerIds.Single();
                    db.OrdinaryExpenses.Add( ordinaryExpense );
                }
                else if ( consumerCount > 1 )
                {
                    double averagePrice = EnhancedMath.RoundDown( createModel.Price / consumerCount, 2 );
                    double primaryConsumerPrice = createModel.Price - averagePrice * ( consumerCount - 1 );
                    int i = 0;

                    foreach ( long consumerId in createModel.SelectedConsumerIds )
                    {
                        ordinaryExpense.Sequence = sequence + i;
                        ordinaryExpense.ConsumerId = consumerId;
                        ordinaryExpense.Price = i == 0 ? primaryConsumerPrice : averagePrice;
                        db.OrdinaryExpenses.Add( ordinaryExpense );
                        i++;
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch ( DbEntityValidationException ex )
                {
                    foreach ( DbEntityValidationResult dbEntityValidationResult in ex.EntityValidationErrors )
                    {
                        foreach ( DbValidationError dbValidationError in dbEntityValidationResult.ValidationErrors )
                        {
                            string errorMessage = dbValidationError.ErrorMessage;
                        }
                    }
                    throw;
                }

                return RedirectToAction( actionNameToRedirectTo );
            }

            PopulateCreateModel( createModel );
            return View( createModel );
        }

        [MvcSiteMapNode( Title = "Edit", ParentKey = "OrdinaryExpense" )]
        public ActionResult Edit( long id )
        {
            OrdinaryExpense ordinaryExpense = db.OrdinaryExpenses.Find( id );
            OrdinaryExpenseEditModel editModel = Mapper.Map<OrdinaryExpense, OrdinaryExpenseEditModel>( ordinaryExpense );
            PopulateEditModel( editModel );
            return View( editModel );
        }

        [HttpPost]
        public ActionResult Edit( OrdinaryExpenseEditModel editModel )
        {
            if ( ModelState.IsValid )
            {
                OrdinaryExpense ordinaryExpense = Mapper.Map<OrdinaryExpenseEditModel, OrdinaryExpense>( editModel );
                db.Entry( ordinaryExpense ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction( "Index" );
            }

            PopulateEditModel( editModel );
            return View( editModel );
        }
 
        public ActionResult Delete(long id)
        {
            OrdinaryExpense ordinaryexpense = db.OrdinaryExpenses.Find(id);
            return View(ordinaryexpense);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            OrdinaryExpense ordinaryexpense = db.OrdinaryExpenses.Find(id);
            db.OrdinaryExpenses.Remove(ordinaryexpense);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void PopulateCreateModel( OrdinaryExpenseCreateModel createModel )
        {
            createModel.Categories = ExpenseEntitiesCache.GetCategories( db );
            createModel.PaymentMethods = ExpenseEntitiesCache.GetPaymentMethods( db );
            createModel.Consumers = ExpenseEntitiesCache.GetConsumers( db );
        }

        private void PopulateEditModel( OrdinaryExpenseEditModel editModel )
        {
            editModel.Categories = ExpenseEntitiesCache.GetCategories( db );
            editModel.PaymentMethods = ExpenseEntitiesCache.GetPaymentMethods( db );
            editModel.Consumers = ExpenseEntitiesCache.GetConsumers( db );
        }

    }
}