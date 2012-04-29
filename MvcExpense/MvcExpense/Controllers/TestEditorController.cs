using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcExpense.ViewModels;

namespace MvcExpense.Controllers
{
    public class TestEditorController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var model = new TestEditorViewModel();
            model.SampleDate = DateTime.Now;
            return View( model );
        }

        [HttpPost]
        public ActionResult Create( TestEditorViewModel model )
        {
            return RedirectToAction( "Create" );
        }

    }
}
