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
    public class PaymentMethodController : Controller
    {
        private zExpenseEntities db = new zExpenseEntities();

        //
        // GET: /PaymentMethod/

        public ViewResult Index()
        {
            return View(db.PaymentMethods.ToList());
        }

        //
        // GET: /PaymentMethod/Details/5

        public ViewResult Details(long id)
        {
            PaymentMethod paymentmethod = db.PaymentMethods.Find(id);
            return View(paymentmethod);
        }

        //
        // GET: /PaymentMethod/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /PaymentMethod/Create

        [HttpPost]
        public ActionResult Create(PaymentMethod paymentmethod)
        {
            if (ModelState.IsValid)
            {
                db.PaymentMethods.Add(paymentmethod);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(paymentmethod);
        }
        
        //
        // GET: /PaymentMethod/Edit/5
 
        public ActionResult Edit(long id)
        {
            PaymentMethod paymentmethod = db.PaymentMethods.Find(id);
            return View(paymentmethod);
        }

        //
        // POST: /PaymentMethod/Edit/5

        [HttpPost]
        public ActionResult Edit(PaymentMethod paymentmethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentmethod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymentmethod);
        }

        //
        // GET: /PaymentMethod/Delete/5
 
        public ActionResult Delete(long id)
        {
            PaymentMethod paymentmethod = db.PaymentMethods.Find(id);
            return View(paymentmethod);
        }

        //
        // POST: /PaymentMethod/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            PaymentMethod paymentmethod = db.PaymentMethods.Find(id);
            db.PaymentMethods.Remove(paymentmethod);
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