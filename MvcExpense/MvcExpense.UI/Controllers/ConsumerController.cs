using System.Data;
using System.Linq;
using System.Web.Mvc;
using MvcExpense.Core.Models;
using MvcExpense.Infrastructure.EntityFramework;
using MvcExpense.MvcExpenseHelper;

namespace MvcExpense.UI.Controllers
{
    public partial class ConsumerController : Controller
    {
        private MvcExpenseDbContext db = MvcExpenseFactory.NewDbContext();// new MvcExpenseDbContext();

        //
        // GET: /Consumer/

        public virtual ViewResult Index()
        {
            return View(db.Consumers.ToList());
        }

        //
        // GET: /Consumer/Details/5

        public virtual ViewResult Details( long id )
        {
            Consumer consumer = db.Consumers.Find(id);
            return View(consumer);
        }

        //
        // GET: /Consumer/Create

        public virtual ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Consumer/Create

        [HttpPost]
        public virtual ActionResult Create( Consumer consumer )
        {
            if (ModelState.IsValid)
            {
                db.Consumers.Add(consumer);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(consumer);
        }
        
        //
        // GET: /Consumer/Edit/5

        public virtual ActionResult Edit( long id )
        {
            Consumer consumer = db.Consumers.Find(id);
            return View(consumer);
        }

        //
        // POST: /Consumer/Edit/5

        [HttpPost]
        public virtual ActionResult Edit( Consumer consumer )
        {
            if (ModelState.IsValid)
            {
                db.Entry(consumer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(consumer);
        }

        //
        // GET: /Consumer/Delete/5

        public virtual ActionResult Delete( long id )
        {
            Consumer consumer = db.Consumers.Find(id);
            return View(consumer);
        }

        //
        // POST: /Consumer/Delete/5

        [HttpPost, ActionName("Delete")]
        public virtual ActionResult DeleteConfirmed( long id )
        {            
            Consumer consumer = db.Consumers.Find(id);
            db.Consumers.Remove(consumer);
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