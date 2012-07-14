using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MvcExpense.Core.Models;
using MvcExpense.Infrastructure.EntityFramework;
using MvcExpense.MvcExpenseHelper;

namespace MvcExpense.UI.Controllers
{
    public partial class CategoryController : Controller
    {
        private MvcExpenseDbContext db = MvcExpenseFactory.NewDbContext(); // new MvcExpenseDbContext();

        //
        // GET: /Category/

        public virtual ViewResult Index()
        {
            var categories = db.Categories.Include(c => c.Parent);
            return View(categories.ToList());
        }

        public virtual ViewResult Tree()
        {
            List<Category> categories = db.Categories.ToList();

            Category root = categories.Where( x => x.lft == 1 ).Single();

            return View( categories );
        }

        //
        // GET: /Category/Details/5

        public virtual ViewResult Details( long id )
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        //
        // GET: /Category/Create

        public virtual ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.Categories, "Id", "Name");
            return View();
        } 

        //
        // POST: /Category/Create

        [HttpPost]
        public virtual ActionResult Create( Category category )
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.ParentId = new SelectList(db.Categories, "Id", "Name", category.ParentId);
            return View(category);
        }
        
        //
        // GET: /Category/Edit/5

        public virtual ActionResult Edit( long id )
        {
            Category category = db.Categories.Find(id);
            ViewBag.ParentId = new SelectList(db.Categories, "Id", "Name", category.ParentId);
            return View(category);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public virtual ActionResult Edit( Category category )
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                ExpenseEntitiesCache.RefreshCategories(MvcExpenseFactory.NewUnitOfWork());
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = new SelectList(db.Categories, "Id", "Name", category.ParentId);
            return View(category);
        }

        //
        // GET: /Category/Delete/5

        public virtual ActionResult Delete( long id )
        {
            Category category = db.Categories.Find(id);
            return View(category);
        }

        //
        // POST: /Category/Delete/5

        [HttpPost, ActionName("Delete")]
        public virtual ActionResult DeleteConfirmed( long id )
        {            
            Category category = db.Categories.Find(id);
            db.Categories.Remove(category);
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