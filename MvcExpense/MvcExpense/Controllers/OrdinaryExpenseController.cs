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

        void PopulateEditModel( OrdinaryExpenseEditModel editModel, zExpenseEntities db )
        {
            editModel.Categories = ExpenseEntitiesCache.GetCategories( db );
            editModel.PaymentMethods = ExpenseEntitiesCache.GetPaymentMethods( db );
            editModel.Consumers = ExpenseEntitiesCache.GetConsumers( db );
            editModel.Date = ExpenseEntitiesHelper.GetMostRecentDate( db );
        }

        public ActionResult Create()
        {
            var editModel = new OrdinaryExpenseEditModel();
            PopulateEditModel( editModel, db );
            editModel.SelectedConsumerIds = new long[] { 1 }; // todo remove hardcode
            return View( editModel );
        }

        [HttpPost]
        [MultiButton( MatchFormKey = "action", MatchFormValue = "Create" )]
        public ActionResult Create( OrdinaryExpenseEditModel editModel )
        {
            return CreateAndRedirect( editModel, "Index" );
        }

        [HttpPost]
        [MultiButton( MatchFormKey = "action", MatchFormValue = "Create and New" )]
        public ActionResult CreateAndNew( OrdinaryExpenseEditModel editModel )
        {
            return CreateAndRedirect( editModel, "Create" );
        }

        private ActionResult CreateAndRedirect( OrdinaryExpenseEditModel editModel, string actionNameToRedirectTo )
        {
            if ( ModelState.IsValid )
            {
                OrdinaryExpense ordinaryExpense = Mapper.Map<OrdinaryExpenseEditModel, OrdinaryExpense>( editModel );
                int sequence = ExpenseEntitiesHelper.NewSequence( db, ordinaryExpense.Date );

                int consumerCount = editModel.SelectedConsumerIds.Count();

                if ( consumerCount == 1 )
                {
                    ordinaryExpense.Sequence = sequence;
                    ordinaryExpense.ConsumerId = editModel.SelectedConsumerIds.Single();
                    db.OrdinaryExpenses.Add( ordinaryExpense );
                }
                else if ( consumerCount > 1 )
                {
                    double averagePrice = EnhancedMath.RoundDown( editModel.Price / consumerCount, 2 );
                    double primaryConsumerPrice = editModel.Price - averagePrice * ( consumerCount - 1 );

                    int i = 0;
                    foreach ( long consumerId in editModel.SelectedConsumerIds )
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

            PopulateEditModel( editModel, db );
            return View( editModel );
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
    }
}