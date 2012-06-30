using System;
using System.Web.Mvc;
using MvcExpense.Core;

namespace MvcExpense.UI.Controllers
{
    public abstract partial class AbstractMvcExpenseController : Controller
    {
        public IMvcExpenseUnitOfWork UnitOfWork { get; private set; }

        // todo remove. required for T4MVC.
        protected AbstractMvcExpenseController()
        {
            //throw new Exception( "todo remove. required for T4MVC." );
        }

        protected AbstractMvcExpenseController( IMvcExpenseUnitOfWork unitOfWork )
        {
            UnitOfWork = unitOfWork;
        }

        protected override void Dispose( bool disposing )
        {
            UnitOfWork.Dispose();
            base.Dispose( disposing );
        }

    }
}