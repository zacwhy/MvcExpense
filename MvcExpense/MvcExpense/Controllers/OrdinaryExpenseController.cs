using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using MvcExpense.DAL;
using MvcExpense.Models;
using MvcExpense.MvcExpenseHelper;
using MvcExpense.Services;
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
        private readonly IMvcExpenseUnitOfWork _unitOfWork;

        public OrdinaryExpenseController()
        {
            _unitOfWork = new MvcExpenseUnitOfWork();
        }

        public OrdinaryExpenseController( IMvcExpenseUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
        }

        public ViewResult Index( int? year, int? month, int? day )
        {
            DateRange dateRange = DateRange.CreateDateRange( year, month, day, DateTime.Now );

            IQueryable<OrdinaryExpense> query = _unitOfWork.OrdinaryExpenseRepository.GetWithDateRange( dateRange );
            query = query.OrderByDescending( x => new { x.Date, x.Sequence } );

            IQueryable<OrdinaryExpenseViewModel> queryWithProjection =
                from x in query
                select new OrdinaryExpenseViewModel
                {
                    Id = x.Id,
                    Date = x.Date,
                    Sequence = x.Sequence,
                    Price = x.Price,
                    ConsumerName = x.Consumer != null ? x.Consumer.Name : string.Empty,
                    CategoryName = x.Category != null ? x.Category.Name : string.Empty,
                    Description = x.Description,
                    PaymentMethodName = x.PaymentMethod != null ? x.PaymentMethod.Name : string.Empty
                };

            //IQueryable<OrdinaryExpenseViewModel> queryWithProjection =
            //    from x in query
            //    select new OrdinaryExpenseViewModel
            //    {
            //        Id = x.Id,
            //        Date = x.Date,
            //        Sequence = x.Sequence,
            //        Price = x.Price,
            //        ConsumerName = x.Consumer.Name,
            //        CategoryName = x.Category.Name,
            //        Description = x.Description,
            //        PaymentMethodName = x.PaymentMethod.Name
            //    };

            List<OrdinaryExpenseViewModel> list = queryWithProjection.ToList();

            ViewBag.StartDate = dateRange.StartDate;
            ViewBag.EndDate = dateRange.EndDate.AddDays( -1 );

            return View( list );
        }

        [AutoMap( typeof( OrdinaryExpense ), typeof( OrdinaryExpenseViewModel ) )]
        public ViewResult Details( long id )
        {
            OrdinaryExpense model = _unitOfWork.OrdinaryExpenseRepository.GetById( id );
            return View( model );
        }

        public ActionResult Create()
        {
            var createModel = new OrdinaryExpenseCreateModel();
            createModel.Date = OrdinaryExpenseService.GetMostRecentDate( _unitOfWork );
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
                List<OrdinaryExpense> list = OrdinaryExpenseService.GetOrdinaryExpenses( _unitOfWork, createModel, Categories );

                foreach ( OrdinaryExpense item in list )
                {
                    _unitOfWork.OrdinaryExpenseRepository.Insert( item );
                }

                object flashArguments;

                try
                {
                    _unitOfWork.Save();

                    string flashMessage;

                    if ( list.Count == 1 )
                    {
                        OrdinaryExpense model = list.First();
                        flashMessage = string.Format( "Added {0}.", model );
                    }
                    else //if ( list.Count > 1 )
                    {
                        flashMessage = string.Format( "Added {0} items.", list.Count );
                    }

                    flashArguments = new { notice = flashMessage };
                }
                catch ( DbEntityValidationException ex )
                {
                    var sb = new StringBuilder("<ol>");
                    foreach ( DbEntityValidationResult dbEntityValidationResult in ex.EntityValidationErrors )
                    {
                        foreach ( DbValidationError dbValidationError in dbEntityValidationResult.ValidationErrors )
                        {
                            string errorMessage = dbValidationError.ErrorMessage;
                            sb.AppendFormat( "<li>{0}</li>", errorMessage );
                        }
                    }
                    sb.AppendLine( "</ol>" );
                    flashArguments = new { error = "Errors: " + sb };
                    //throw;
                }

                return RedirectToAction( actionNameToRedirectTo ).WithFlash( flashArguments );
            }

            PopulateCreateModel( createModel );
            return View( createModel );
        }

        [MvcSiteMapNode( Title = "Edit", ParentKey = "OrdinaryExpense" )]
        public ActionResult Edit( long id )
        {
            OrdinaryExpense model = _unitOfWork.OrdinaryExpenseRepository.GetById( id );
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
                _unitOfWork.OrdinaryExpenseRepository.Update( model );
                _unitOfWork.Save();
                return RedirectToAction( "Index" );
            }

            PopulateEditModel( editModel );
            return View( editModel );
        }

        public ActionResult Delete( long id )
        {
            OrdinaryExpense model = _unitOfWork.OrdinaryExpenseRepository.GetById( id );
            return View( model );
        }

        [HttpPost, ActionName( "Delete" )]
        public ActionResult DeleteConfirmed( long id )
        {
            _unitOfWork.OrdinaryExpenseRepository.Delete( id );
            _unitOfWork.Save();
            string flashMessage = string.Format( "Deleted Id {0}.", id );
            return RedirectToAction( "Index" ).WithFlash( new { notice = flashMessage } );
        }

        protected override void Dispose( bool disposing )
        {
            _unitOfWork.Dispose();
            base.Dispose( disposing );
        }

        private IList<Category> Categories
        {
            get
            {
                return ExpenseEntitiesCache.GetCategories( _unitOfWork );
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
            modelBase.PaymentMethods = ExpenseEntitiesCache.GetPaymentMethods( _unitOfWork );
            modelBase.Consumers = ExpenseEntitiesCache.GetConsumers( _unitOfWork );
        }

    }
}