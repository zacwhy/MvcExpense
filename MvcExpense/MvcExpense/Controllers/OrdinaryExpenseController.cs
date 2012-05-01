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
            PopulateCreateModel( createModel, db );
            createModel.SelectedConsumerIds = new long[] { 1 }; // todo remove hardcode
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

            PopulateCreateModel( createModel, db );
            return View( createModel );
        }

        [MvcSiteMapNode( Title = "Edit", ParentKey = "OrdinaryExpense" )]
        [AutoMap( typeof( OrdinaryExpense ), typeof( OrdinaryExpenseEditModel ) )]
        public ActionResult Edit( long id )
        {
            ViewBag.Id = id;
            OrdinaryExpense ordinaryexpense = db.OrdinaryExpenses.Find( id );
            ViewBag.CategoryId = new SelectList( db.Categories, "Id", "Name", ordinaryexpense.CategoryId );
            ViewBag.ConsumerId = new SelectList( db.Consumers, "Id", "Name", ordinaryexpense.ConsumerId );
            ViewBag.PaymentMethodId = new SelectList( db.PaymentMethods, "Id", "Name", ordinaryexpense.PaymentMethodId );
            return View( ordinaryexpense );
        }

        [HttpPost]
        public ActionResult Edit(OrdinaryExpense ordinaryexpense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordinaryexpense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", ordinaryexpense.CategoryId);
            ViewBag.ConsumerId = new SelectList(db.Consumers, "Id", "Name", ordinaryexpense.ConsumerId);
            return View(ordinaryexpense);
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

        private void PopulateCreateModel( OrdinaryExpenseCreateModel createModel, zExpenseEntities db )
        {
            createModel.Categories = ExpenseEntitiesCache.GetCategories( db );
            createModel.PaymentMethods = ExpenseEntitiesCache.GetPaymentMethods( db );
            createModel.Consumers = ExpenseEntitiesCache.GetConsumers( db );
            createModel.Date = ExpenseEntitiesHelper.GetMostRecentDate( db );
        }

    }
}