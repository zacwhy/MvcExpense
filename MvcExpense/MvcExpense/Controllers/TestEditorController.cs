using System;
using System.Web.Mvc;
using MvcExpense.ViewModels;

namespace MvcExpense.UI.Controllers
{
    public partial class TestEditorController : Controller
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Create()
        {
            var model = new TestEditorViewModel();
            model.SampleDate = DateTime.Now;
            return View( model );
        }

        [HttpPost]
        public virtual ActionResult Create( TestEditorViewModel model )
        {
            return RedirectToAction( "Create" );
        }

    }
}
