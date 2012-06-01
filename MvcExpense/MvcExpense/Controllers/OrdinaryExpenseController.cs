using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using MvcExpense.DAL;
using MvcExpense.Models;
using MvcExpense.ViewModels;
using MvcSiteMapProvider;
using Zac.DateRange;
using Zac.MvcAutoMapAttribute;
using Zac.MvcFlashMessage;
using Zac.MvcMultiButtonAttribute;

namespace MvcExpense.Controllers
{
    public class OrdinaryExpenseController : Controller
    {
        private IMvcExpenseUnitOfWork unitOfWork;

        public OrdinaryExpenseController()
        {
            unitOfWork = new MvcExpenseUnitOfWork();
        }

        public OrdinaryExpenseController( IMvcExpenseUnitOfWork unitOfWork )
        {
            this.unitOfWork = unitOfWork;
        }

        public ViewResult Index( int? year, int? month, int? day )
        {
            DateRange dateRange = DateRange.CreateDateRange( year, month, day, DateTime.Now );

            IQueryable<OrdinaryExpense> query = unitOfWork.OrdinaryExpenseRepository.GetWithDateRange( dateRange );

            IQueryable<OrdinaryExpenseViewModel> queryWithOrderAndProjection =
                from x in query
                orderby new { x.Date, x.Sequence } descending
                select new OrdinaryExpenseViewModel
                {
                    Id = x.Id,
                    Date = x.Date,
                    Sequence = x.Sequence,
                    Price = x.Price,
                    ConsumerName = x.Consumer.Name,
                    CategoryName = x.Category.Name,
                    Description = x.Description,
                    PaymentMethodName = x.PaymentMethod.Name
                };

            List<OrdinaryExpenseViewModel> list = queryWithOrderAndProjection.ToList();

            ViewBag.StartDate = dateRange.StartDate;
            ViewBag.EndDate = dateRange.EndDate.AddDays( -1 );

            return View( list );
        }

        [AutoMap( typeof( OrdinaryExpense ), typeof( OrdinaryExpenseViewModel ) )]
        public ViewResult Details( long id )
        {
            OrdinaryExpense model = unitOfWork.OrdinaryExpenseRepository.GetById( id );
            return View( model );
        }

        public ActionResult Create()
        {
            var createModel = new OrdinaryExpenseCreateModel();
            createModel.Date = OrdinaryExpenseService.GetMostRecentDate( unitOfWork );
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
                List<OrdinaryExpense> list = OrdinaryExpenseService.GetOrdinaryExpenses( unitOfWork, createModel, Categories );

                foreach ( OrdinaryExpense item in list )
                {
                    unitOfWork.OrdinaryExpenseRepository.Insert( item );
                }

                object flashArguments;

                try
                {
                    unitOfWork.Save();

                    string flashMessage;

                    if ( list.Count == 1 )
                    {
                        OrdinaryExpense model = list.First();
                        flashMessage = string.Format( "Added {0}.", model.ToString() );
                    }
                    else //if ( list.Count > 1 )
                    {
                        flashMessage = string.Format( "Added {0} items.", list.Count );
                    }

                    flashArguments = new { notice = flashMessage };
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
                    flashArguments = new { error = "Error message." };
                    throw;
                }

                return RedirectToAction( actionNameToRedirectTo ).WithFlash( flashArguments );
            }

            PopulateCreateModel( createModel );
            return View( createModel );
        }

        [MvcSiteMapNode( Title = "Edit", ParentKey = "OrdinaryExpense" )]
        public ActionResult Edit( long id )
        {
            OrdinaryExpense model = unitOfWork.OrdinaryExpenseRepository.GetById( id );
            OrdinaryExpenseEditModel editModel = Mapper.Map<OrdinaryExpense, OrdinaryExpenseEditModel>( model );
            PopulateEditModel( editModel );
            return View( editModel );
        }

        [HttpPost]
        public ActionResult Edit( OrdinaryExpenseEditModel editModel )
        {
            if ( ModelState.IsValid )
            {
                OrdinaryExpense model = Mapper.Map<OrdinaryExpenseEditModel, OrdinaryExpense>( editModel );
                unitOfWork.OrdinaryExpenseRepository.Update( model );
                unitOfWork.Save();
                return RedirectToAction( "Index" );
            }

            PopulateEditModel( editModel );
            return View( editModel );
        }

        public ActionResult Delete( long id )
        {
            OrdinaryExpense model = unitOfWork.OrdinaryExpenseRepository.GetById( id );
            return View( model );
        }

        [HttpPost, ActionName( "Delete" )]
        public ActionResult DeleteConfirmed( long id )
        {
            unitOfWork.OrdinaryExpenseRepository.Delete( id );
            unitOfWork.Save();
            string flashMessage = string.Format( "Deleted Id {0}.", id );
            return RedirectToAction( "Index" ).WithFlash( new { notice = flashMessage } );
        }

        protected override void Dispose( bool disposing )
        {
            unitOfWork.Dispose();
            base.Dispose( disposing );
        }

        private IList<Category> Categories
        {
            get
            {
                return ExpenseEntitiesCache.GetCategories( unitOfWork );
            }
        }

        private void PopulateCreateModel( OrdinaryExpenseCreateModel createModel )
        {
            PopulateModelBase( createModel );
        }

        private void PopulateEditModel( OrdinaryExpenseEditModel editModel )
        {
            PopulateModelBase( editModel );
        }

        private void PopulateModelBase( OrdinaryExpenseCreateEditModelBase modelBase )
        {
            modelBase.Categories = Categories;
            modelBase.PaymentMethods = ExpenseEntitiesCache.GetPaymentMethods( unitOfWork );
            modelBase.Consumers = ExpenseEntitiesCache.GetConsumers( unitOfWork );
        }

    }
}