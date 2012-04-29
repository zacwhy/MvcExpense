using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcExpense.Models;

namespace MvcExpense.Controllers
{ 
    public class ConsumerController : Controller
    {
        private zExpenseEntities db = new zExpenseEntities();

        //
        // GET: /Consumer/

        public ViewResult Index()
        {
            return View(db.Consumers.ToList());
        }

        //
        // GET: /Consumer/Details/5

        public ViewResult Details(long id)
        {
            Consumer consumer = db.Consumers.Find(id);
            return View(consumer);
        }

        //
        // GET: /Consumer/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Consumer/Create

        [HttpPost]
        public ActionResult Create(Consumer consumer)
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
 
        public ActionResult Edit(long id)
        {
            Consumer consumer = db.Consumers.Find(id);
            return View(consumer);
        }

        //
        // POST: /Consumer/Edit/5

        [HttpPost]
        public ActionResult Edit(Consumer consumer)
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
 
        public ActionResult Delete(long id)
        {
            Consumer consumer = db.Consumers.Find(id);
            return View(consumer);
        }

        //
        // POST: /Consumer/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
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