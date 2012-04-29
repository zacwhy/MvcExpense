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
            IList<Category> categories = ExpenseEntitiesCache.GetCategories(db);
            var leafCategories =
                from x in categories
                select new
                {
                    x.Id,
                    x.Name,
                    Display = x.Name
                };

            ViewBag.CategoryId = new SelectList( leafCategories, "Id", "Display" );
            //ViewBag.ConsumerId = new SelectList( db.Consumers, "Id", "Name", 1 );
            ViewBag.PaymentMethodId = new SelectList( ExpenseEntitiesCache.GetPaymentMethods( db ), "Id", "Name" );

            var model = new OrdinaryExpenseViewModel();
            model.ConsumerList = ExpenseEntitiesCache.GetConsumers( db );
            model.Date = ExpenseEntitiesHelper.GetMostRecentDate( db );
            model.SelectedConsumerIds = new long[] { 1 };
            return View( model );
        }

        [HttpPost]
        [MultiButton( MatchFormKey = "action", MatchFormValue = "Create" )] 
        public ActionResult Create( OrdinaryExpenseViewModel model )
        {
            return CreateAndRedirect( model, "Index" );
        }

        [HttpPost]
        [MultiButton( MatchFormKey = "action", MatchFormValue = "Create and New" )]
        public ActionResult CreateAndNew( OrdinaryExpenseViewModel model )
        {
            return CreateAndRedirect( model, "Create" );
        }

        private ActionResult CreateAndRedirect( OrdinaryExpenseViewModel model, string actionNameToRedirectTo )
        {
            if ( ModelState.IsValid )
            {
                if ( model.SelectedConsumerIds.Count() == 1 )
                {
                    model.ConsumerId = model.SelectedConsumerIds.Single();
                }

                OrdinaryExpense ordinaryExpense = Mapper.Map<OrdinaryExpenseViewModel, OrdinaryExpense>( model );
                ordinaryExpense.Sequence = ExpenseEntitiesHelper.NewSequence( db, ordinaryExpense.Date );
                db.OrdinaryExpenses.Add( ordinaryExpense );

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

            var leafCategories = db.Categories.Where( x => x.Children.Count == 0 );
            ViewBag.CategoryId = new SelectList( leafCategories, "Id", "Name" );
            ViewBag.PaymentMethodId = new SelectList( db.PaymentMethods, "Id", "Name" );

            //    ViewBag.CategoryId = new SelectList( db.Categories, "Id", "Name", ordinaryExpenseViewModel.CategoryId );
            ViewBag.ConsumerId = new SelectList( db.Consumers, "Id", "Name", model.ConsumerId );
            return View( model );
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