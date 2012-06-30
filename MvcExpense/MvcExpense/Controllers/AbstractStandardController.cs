using System.Web.Mvc;
using AutoMapper;
using Zac.StandardCore;

namespace MvcExpense.UI.Controllers
{
    public abstract class AbstractStandardController<TModel, TCreateDisplay, TCreateInput> : Controller
        where TCreateDisplay : new()
    {
        public IStandardUnitOfWork UnitOfWork { get; private set; }

        protected AbstractStandardController( IStandardUnitOfWork unitOfWork )
        {
            UnitOfWork = unitOfWork;
        }

        protected override void Dispose( bool disposing )
        {
            UnitOfWork.Dispose();
            base.Dispose( disposing );
        }

        public virtual ActionResult Create()
        {
            var display = new TCreateDisplay();
            PopulateCreateDisplay( display );
            return View( display );
        }

        protected virtual void PopulateCreateDisplay( TCreateDisplay display )
        {
            //PopulateDisplay( display, UnitOfWork );
        }

        [HttpPost]
        public ActionResult Create( TCreateInput input )
        {
            if ( ModelState.IsValid )
            {
                try
                {
                    // TODO: Add insert logic here

                    return RedirectToAction( "Index" );
                }
                catch
                {
                    return View();
                }
            }

            return View();
        }

        protected virtual TCreateDisplay ToCreateDisplay( TCreateInput input )
        {
            return Mapper.Map<TCreateInput, TCreateDisplay>( input );
        }

    }
}