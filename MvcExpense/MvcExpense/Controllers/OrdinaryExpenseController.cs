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

        //
        // GET: /OrdinaryExpense/

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

            var ordinaryExpensesQuery = ordinaryExpensesQuery1//db.OrdinaryExpenses.Where( x => x.Date >= beginOfCurrentMonth )
                .OrderByDescending( x => new { x.Date, x.Sequence } )
                .Include( o => o.Category )
                .Include( o => o.Consumer );

            IList<OrdinaryExpense> ordinaryExpenses = ordinaryExpensesQuery.ToList();

            return View( ordinaryExpenses );
        }

        //
        // GET: /OrdinaryExpense/Details/5

        [AutoMap( typeof( OrdinaryExpense ), typeof( OrdinaryExpenseViewModel ) )]
        public ViewResult Details(long id)
        {
            OrdinaryExpense ordinaryexpense = db.OrdinaryExpenses.Find(id);
            return View(ordinaryexpense);
        }

        //
        // GET: /OrdinaryExpense/Create

        public ActionResult Create()
        {
            IList<Category> categories = db.Categories.ToList();
            var leafCategories =
                from x in categories
                where x.Children.Count == 0
                orderby x.Parent != null ? x.Parent.Name : string.Empty, x.Name
                select new
                {
                    x.Id,
                    x.Name,
                    Display = x.Parent != null ? string.Format( "{0} - {1}", x.Name, x.Parent.Name ) : x.Name
                };

            ViewBag.CategoryId = new SelectList( leafCategories, "Id", "Display" );
            //ViewBag.ConsumerId = new SelectList( db.Consumers, "Id", "Name", 1 );
            ViewBag.PaymentMethodId = new SelectList( db.PaymentMethods, "Id", "Name" );
            var ordinaryExpenseViewModel = new OrdinaryExpenseViewModel();
            ordinaryExpenseViewModel.Date = GetMostRecentDate();
            ordinaryExpenseViewModel.SelectedConsumerIds = new long[] { 1 };
            return View( ordinaryExpenseViewModel );
        }

        DateTime GetMostRecentDate()
        {
            IQueryable<DateTime> query =
                from x in db.OrdinaryExpenses
                where x.Id == db.OrdinaryExpenses.Max( y => y.Id )
                select x.Date;
            DateTime mostRecentDate = query.Single();
            return mostRecentDate;
        }

        // todo shift out
        int NewSequence( DateTime date )
        {
            IQueryable<OrdinaryExpense> ordinaryExpenses = db.OrdinaryExpenses.Where( x => x.Date == date );
            int maxSequence = ordinaryExpenses.Max( x => (int?) x.Sequence ) ?? 0;
            return maxSequence + 1;
        }

        //
        // POST: /OrdinaryExpense/Create

        [HttpPost]
        [MultiButton( MatchFormKey = "action", MatchFormValue = "Create and New" )]
        public ActionResult CreateAndNew( OrdinaryExpenseViewModel ordinaryExpenseViewModel )
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [MultiButton( MatchFormKey = "action", MatchFormValue = "Create" )] 
        public ActionResult Create( OrdinaryExpenseViewModel ordinaryExpenseViewModel ) //OrdinaryExpense ordinaryexpense
        {
            if ( ModelState.IsValid )
            {
                //int additionalConsumerCount = GetAdditionalConsumerCount( ordinaryExpenseViewModel );
                //if ( additionalConsumerCount > 0 )
                //{
                //    int totalConsumerCount = additionalConsumerCount + 1;
                //    double pricePerAdditonalConsumer = EnhancedMath.RoundDown( ordinaryExpenseViewModel.Price / totalConsumerCount, 2 );
                //    double priceForPrimaryConsumer = ordinaryExpenseViewModel.Price - pricePerAdditonalConsumer * additionalConsumerCount;
                //}

                if ( ordinaryExpenseViewModel.SelectedConsumerIds.Count() == 1 )
                {
                    ordinaryExpenseViewModel.ConsumerId = ordinaryExpenseViewModel.SelectedConsumerIds.Single();
                }

                OrdinaryExpense ordinaryExpense = Mapper.Map<OrdinaryExpenseViewModel, OrdinaryExpense>( ordinaryExpenseViewModel );
                ordinaryExpense.Sequence = NewSequence( ordinaryExpense.Date );
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

                return RedirectToAction( "Index" );
            }

            var leafCategories = db.Categories.Where( x => x.Children.Count == 0 );
            ViewBag.CategoryId = new SelectList( leafCategories, "Id", "Name" );
            ViewBag.PaymentMethodId = new SelectList( db.PaymentMethods, "Id", "Name" );

            //    ViewBag.CategoryId = new SelectList( db.Categories, "Id", "Name", ordinaryExpenseViewModel.CategoryId );
            ViewBag.ConsumerId = new SelectList( db.Consumers, "Id", "Name", ordinaryExpenseViewModel.ConsumerId );
            return View( ordinaryExpenseViewModel );
        }
        
        //
        // GET: /OrdinaryExpense/Edit/5

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

        //
        // POST: /OrdinaryExpense/Edit/5

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

        //
        // GET: /OrdinaryExpense/Delete/5
 
        public ActionResult Delete(long id)
        {
            OrdinaryExpense ordinaryexpense = db.OrdinaryExpenses.Find(id);
            return View(ordinaryexpense);
        }

        //
        // POST: /OrdinaryExpense/Delete/5

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