using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using AutoMapper;
using MvcExpense.Common;
using MvcExpense.DAL;
using MvcExpense.Models;
using MvcExpense.ViewModels;
using MvcSiteMapProvider;

namespace MvcExpense.Controllers
{
    public class OrdinaryExpenseController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();
        private zExpenseEntities db = new zExpenseEntities(); // todo remove

        public ViewResult Index( int? year, int? month, int? day )
        {
            DateRange dateRange = DateRange.CreateDateRange( year, month, day, DateTime.Now );
            Expression<Func<OrdinaryExpense, bool>> filter = x => x.Date >= dateRange.StartDate && x.Date < dateRange.EndDate;
            Func<IQueryable<OrdinaryExpense>, IOrderedQueryable<OrdinaryExpense>> orderBy = o => o.OrderByDescending( x => new { x.Date, x.Sequence } );
            string includeProperties = "Category,Consumer,PaymentMethod";
            //query = query.Include( o => o.PaymentMethod ).Include( o => o.Consumer );
            IEnumerable<OrdinaryExpense> enumerable = unitOfWork.OrdinaryExpenseRepository.Get( filter: filter, orderBy: orderBy, includeProperties: includeProperties );
            List<OrdinaryExpense> list = enumerable.ToList();

            ViewBag.StartDate = dateRange.StartDate;
            ViewBag.EndDate = dateRange.EndDate.AddDays( -1 );

            return View( list );
        }

        [AutoMap( typeof( OrdinaryExpense ), typeof( OrdinaryExpenseViewModel ) )]
        public ViewResult Details(long id)
        {
            OrdinaryExpense ordinaryexpense = unitOfWork.OrdinaryExpenseRepository.GetById( id );
            //OrdinaryExpense ordinaryexpense = db.OrdinaryExpenses.Find(id);
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

                    Category treat = ExpenseEntitiesCache.GetCategories( db ).Where( x => x.Name == "Treat" ).Single();

                    foreach ( long consumerId in createModel.SelectedConsumerIds )
                    {
                        OrdinaryExpense clone = ordinaryExpense.CloneOrdinaryExpense();
                        clone.Sequence = sequence + i;
                        clone.ConsumerId = consumerId;

                        bool isPrimaryConsumer = ( i == 0 );
                        if ( isPrimaryConsumer )
                        {
                            clone.Price = primaryConsumerPrice;
                        }
                        else
                        {
                            clone.CategoryId = treat.Id;
                            clone.Price = averagePrice;
                        }

                        db.OrdinaryExpenses.Add( clone );
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