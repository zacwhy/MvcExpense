using System.Web.Mvc;

namespace MvcExpense.UI.Controllers
{
    public partial class HomeController : Controller
    {
        public virtual ActionResult Index()
        {
            //return RedirectToAction( "index", "Report" );

            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public virtual ActionResult About()
        {
            return View();
        }
    }
}
