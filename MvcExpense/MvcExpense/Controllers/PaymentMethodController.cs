using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AutoMapper;
using MvcExpense.Models;
using MvcExpense.ViewModels;

namespace MvcExpense.Controllers
{ 
    public class PaymentMethodController : Controller
    {
        private zExpenseEntities db = new zExpenseEntities();

        //
        // GET: /PaymentMethod/

        public ViewResult Index()
        {
            List<PaymentMethod> list = db.PaymentMethods.OrderBy( x => x.Sequence ).ToList();
            return View( list );
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
                RefreshCachedPaymentMethods();
                return RedirectToAction( "Index" );  
            }

            return View(paymentmethod);
        }
        
        //
        // GET: /PaymentMethod/Edit/5

        public ActionResult Edit( long id )
        {
            PaymentMethod paymentMethod = db.PaymentMethods.Find( id );
            PaymentMethodEditModel editModel = Mapper.Map<PaymentMethod, PaymentMethodEditModel>( paymentMethod );
            editModel.PreviousItemSequence = paymentMethod.Sequence - 1;
            PopulateEditModel( editModel );
            return View( editModel );
        }

        //
        // POST: /PaymentMethod/Edit/5

        [HttpPost]
        public ActionResult Edit( PaymentMethodEditModel editModel )//PaymentMethod paymentmethod)
        {
            if ( ModelState.IsValid )
            {
                int originalSequence = editModel.Sequence;
                int targetSequence = editModel.PreviousItemSequence + 1;
                PaymentMethod paymentMethod = Mapper.Map<PaymentMethodEditModel, PaymentMethod>( editModel );

                var sql = new StringBuilder( "update PaymentMethods set Sequence = (select max(Sequence) from PaymentMethods) + 1 where Id = @p0;" );

                if ( targetSequence < originalSequence ) // shift forward
                {
                    sql.Append( "update PaymentMethods set Sequence = Sequence + 1 where Sequence >= @p1 and Sequence < @p2;" );
                    db.Database.ExecuteSqlCommand( sql.ToString(), editModel.Id, targetSequence, originalSequence );
                }
                else if ( targetSequence > originalSequence ) // shift backward
                {
                    targetSequence--;

                    sql.Append( "update PaymentMethods set Sequence = Sequence - 1 where Sequence > @p1 and Sequence <= @p2;" );
                    db.Database.ExecuteSqlCommand( sql.ToString(), editModel.Id, originalSequence, targetSequence );
                }

                if ( targetSequence != originalSequence )
                {
                    paymentMethod.Sequence = targetSequence;
                }

                db.Entry( paymentMethod ).State = EntityState.Modified;
                db.SaveChanges();

                RefreshCachedPaymentMethods();
                return RedirectToAction( "Index" );
            }
            return View( editModel );
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
            RefreshCachedPaymentMethods();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void PopulateEditModel( PaymentMethodEditModel editModel )
        {
            editModel.PaymentMethods = db.PaymentMethods.ToList();
        }

        private void RefreshCachedPaymentMethods()
        {
            ExpenseEntitiesCache.RefreshPaymentMethods( db );
        }

    }
}