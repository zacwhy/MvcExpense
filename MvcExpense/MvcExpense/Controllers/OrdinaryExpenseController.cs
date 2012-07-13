using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.Web.Mvc;
using MvcExpense.Core;
using MvcExpense.Core.Models;
using MvcExpense.MvcExpenseHelper;
using MvcExpense.Services;
using MvcExpense.UI.Models.Display;
using MvcExpense.UI.Models.Input;
using MvcExpense.ViewModels;
using Zac.DateRange;
using Zac.MvcAutoMapAttribute;
using Zac.MvcFlashMessage;
using Zac.MvcMultiButtonAttribute;

namespace MvcExpense.UI.Controllers
{
    public partial class OrdinaryExpenseController : AbstractMvcExpenseController
    {
        public OrdinaryExpenseController( IMvcExpenseUnitOfWork unitOfWork )
            : base( unitOfWork )
        {
        }

        // todo make this work
        //public ViewResult Index()
        //{
        //    return Index( null, null, null );
        //}

        public virtual ViewResult Index( int? year, int? month, int? day )
        {
            DateRange dateRange = DateRange.CreateDateRange( year, month, day, DateTime.Now );

            IQueryable<OrdinaryExpense> query = MvcExpenseUnitOfWork.OrdinaryExpenseRepository.GetWithDateRange( dateRange );
            query = query.OrderByDescending( x => new { x.Date, x.Sequence } );

            IQueryable<OrdinaryExpenseViewModel> queryWithProjection =
                from x in query
                select new OrdinaryExpenseViewModel
                {
                    Id = x.Id,
                    Date = x.Date,
                    Sequence = x.Sequence,
                    Price = x.Price,
                    ConsumerName = x.Consumer != null ? x.Consumer.Name : string.Empty, // todo remove null check
                    CategoryName = x.Category != null ? x.Category.Name : string.Empty, // todo remove null check
                    Description = x.Description,
                    PaymentMethodName = x.PaymentMethod != null ? x.PaymentMethod.Name : string.Empty // todo remove null check
                };

            List<OrdinaryExpenseViewModel> list = queryWithProjection.ToList();

            ViewBag.StartDate = dateRange.StartDate;
            ViewBag.EndDate = dateRange.EndDate.AddDays( -1 );

            return View( list );
        }

        [AutoMap( typeof( OrdinaryExpense ), typeof( OrdinaryExpenseViewModel ) )]
        public virtual ViewResult Details( long id )
        {
            OrdinaryExpense model = MvcExpenseUnitOfWork.OrdinaryExpenseRepository.GetById( id );
            return View( model );
        }

        public virtual ActionResult Create()
        {
            var display = new CreateOrdinaryExpenseDisplay
            {
                Date = MvcExpenseUnitOfWork.OrdinaryExpenseRepository.GetMostRecentDate(),
                SelectedConsumerIds = new long[] { 1 } // todo remove hardcode
            };
            PopulateDisplay( display );
            return View( display );
        }

        [HttpPost]
        [MultiButton( MatchFormKey = "action", MatchFormValue = "Create" )]
        public virtual ActionResult Create( CreateOrdinaryExpenseInput input )
        {
            return CreateAndRedirect<OrdinaryExpenseController>( input, x => x.Index( null, null, null ) );
        }

        [HttpPost]
        [MultiButton( MatchFormKey = "action", MatchFormValue = "Create and New" )]
        public virtual ActionResult CreateAndNew( CreateOrdinaryExpenseInput input )
        {
            return CreateAndRedirect<OrdinaryExpenseController>( input, x => x.Create() );
        }

        private ActionResult CreateAndRedirect<TController>( CreateOrdinaryExpenseInput input, Expression<Action<TController>> actionExpression ) where TController : Controller
        {
            if ( ModelState.IsValid )
            {
                IEnumerable<Category> categories = ExpenseEntitiesCache.GetCategories( MvcExpenseUnitOfWork );
                List<OrdinaryExpense> list = OrdinaryExpenseService.GetOrdinaryExpenses( MvcExpenseUnitOfWork, input, categories );

                foreach ( OrdinaryExpense item in list )
                {
                    MvcExpenseUnitOfWork.OrdinaryExpenseRepository.Insert( item );
                }

                object flashArguments;

                try
                {
                    MvcExpenseUnitOfWork.Save();

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
                catch ( DbEntityValidationException dbEntityValidationException )
                {
                    string flashMessage = GetFlashMessage( dbEntityValidationException );
                    flashArguments = new { error = "Errors: " + flashMessage };
                    //throw;
                }

                return this.RedirectToAction( actionExpression ).WithFlash( flashArguments );
            }

            CreateOrdinaryExpenseDisplay display = input.ToCreateDisplay();
            PopulateDisplay( display );
            return View( display );
        }

        public virtual ActionResult Edit( long id )
        {
            OrdinaryExpense model = MvcExpenseUnitOfWork.OrdinaryExpenseRepository.GetById( id );
            EditOrdinaryExpenseDisplay display = model.ToEditDisplay();
            PopulateDisplay( display );
            return View( display );
        }

        [HttpPost]
        public virtual ActionResult Edit( EditOrdinaryExpenseInput input )
        {
            if ( ModelState.IsValid )
            {
                OrdinaryExpense model = input.ToModel();
                MvcExpenseUnitOfWork.OrdinaryExpenseRepository.Update( model );
                MvcExpenseUnitOfWork.Save();
                return this.RedirectToAction( x => x.Index( null, null, null ) );
            }

            EditOrdinaryExpenseDisplay display = input.ToEditDisplay();
            PopulateDisplay( display );
            return View( display );
        }

        public virtual ActionResult Delete( long id )
        {
            OrdinaryExpense model = MvcExpenseUnitOfWork.OrdinaryExpenseRepository.GetById( id );
            return View( model );
        }

        [HttpPost, ActionName( "Delete" )]
        public virtual ActionResult DeleteConfirmed( long id )
        {
            MvcExpenseUnitOfWork.OrdinaryExpenseRepository.Delete( id );
            MvcExpenseUnitOfWork.Save();
            string flashMessage = string.Format( "Deleted Id {0}.", id );
            return this.RedirectToAction( x => x.Index( null, null, null ) )
                .WithFlash( new { notice = flashMessage } );
            //return RedirectToAction( "Index" ).WithFlash( new { notice = flashMessage } );
        }

        private void PopulateDisplay( CreateOrdinaryExpenseDisplay display )
        {
            PopulateDisplay( display, MvcExpenseUnitOfWork );
        }

        private void PopulateDisplay( EditOrdinaryExpenseDisplay display )
        {
            PopulateDisplay( display, MvcExpenseUnitOfWork );
        }

        private static void PopulateDisplay( CreateOrdinaryExpenseDisplay display, IMvcExpenseUnitOfWork unitOfWork )
        {
            display.Categories = ExpenseEntitiesCache.GetCategories( unitOfWork );
            display.Consumers = ExpenseEntitiesCache.GetConsumers( unitOfWork );
            display.PaymentMethods = ExpenseEntitiesCache.GetPaymentMethods( unitOfWork );
        }

        private static void PopulateDisplay( EditOrdinaryExpenseDisplay display, IMvcExpenseUnitOfWork unitOfWork )
        {
            display.Categories = ExpenseEntitiesCache.GetCategories( unitOfWork );
            display.Consumers = ExpenseEntitiesCache.GetConsumers( unitOfWork );
            display.PaymentMethods = ExpenseEntitiesCache.GetPaymentMethods( unitOfWork );
        }

        private static string GetFlashMessage( DbEntityValidationException dbEntityValidationException )
        {
            var sb = new StringBuilder( "<ol>" );
            foreach ( DbEntityValidationResult dbEntityValidationResult in dbEntityValidationException.EntityValidationErrors )
            {
                foreach ( DbValidationError dbValidationError in dbEntityValidationResult.ValidationErrors )
                {
                    string errorMessage = dbValidationError.ErrorMessage;
                    sb.AppendFormat( "<li>{0}</li>", errorMessage );
                }
            }
            sb.AppendLine( "</ol>" );
            return sb.ToString();
        }

    }

    static class Helper
    {
        public static CreateOrdinaryExpenseDisplay ToCreateDisplay( this CreateOrdinaryExpenseInput input )
        {
            return Mapper.Map<CreateOrdinaryExpenseInput, CreateOrdinaryExpenseDisplay>( input );
        }

        public static EditOrdinaryExpenseDisplay ToEditDisplay( this OrdinaryExpense model )
        {
            return Mapper.Map<OrdinaryExpense, EditOrdinaryExpenseDisplay>( model );
        }

        public static EditOrdinaryExpenseDisplay ToEditDisplay( this EditOrdinaryExpenseInput input )
        {
            return Mapper.Map<EditOrdinaryExpenseInput, EditOrdinaryExpenseDisplay>( input );
        }

        public static OrdinaryExpense ToModel( this EditOrdinaryExpenseInput input )
        {
            return Mapper.Map<EditOrdinaryExpenseInput, OrdinaryExpense>( input );
        }
    }
}