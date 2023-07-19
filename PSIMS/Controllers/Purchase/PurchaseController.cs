using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using PSIMS.Models.PurchaseModel;
using PSIMS.Repository;

namespace PSIMS.Controllers
{
   [Authorize]
    public class PurchaseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        SupplierRepository repo = new SupplierRepository();
        // GET: Purchase
        public ActionResult Index()
        {
            var purchases = db.Purchases
                              .Include(p => p.Supplier)
                              .OrderByDescending(q => q.Date);
                           
            return View(purchases.ToList());
        }

        // GET: Purchase/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _purchase = from p in db.PurchaseItems
                                where p.PurchaseID == id
                                select p;
            if (_purchase == null)
            {
                return HttpNotFound();
            }
            return View(_purchase.ToList());
        }

      
        // GET: Purchase/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PSIMS.Models.PurchaseModel.Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "SupplierName", purchase.SupplierID);
            return View(purchase);
        }

        // POST: Purchase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Date,SupplierID,Amount,Discount,Tax,GrandTotal,IsPaid,LastUpdated,Description")] PSIMS.Models.PurchaseModel.Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.Entry(purchase).Property(x => x.LocationID).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "SupplierName", purchase.SupplierID);
            return View(purchase);
        }

        // GET: Purchase/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PSIMS.Models.PurchaseModel.Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            //var p = db.Purchases.Find(id);
            PSIMS.Models.PurchaseModel.Purchase _purchase = db.Purchases.FirstOrDefault(t => t.ID == id);

            if (_purchase.isStockTransferred == false)
            {
                foreach (var _item in _purchase.PurchaseItems.ToList())
                {
                    foreach (var puhItem in _item.Purchase.PurchaseItems.ToList())
                    {
                        db.PurchaseItems.Remove(puhItem);
                    }
                    db.PurchaseItems.Remove(_item);
                }
                db.Purchases.Remove(_purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.SuccessMessage = "Sorry! You can not delete purchase Items.Because Stock qty have transferred to Branches";
                return View(_purchase);
            }

            //if (p.isStockTransferred == false)
            //{

            //    db.Purchases.Remove(p);
                
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //else
            //{
            //    ViewBag.SuccessMessage = "Sorry! You can not delete purchase Items.Because Stock qty have transferred to Branches";
            //    return View(p);
            //}                                                           
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
