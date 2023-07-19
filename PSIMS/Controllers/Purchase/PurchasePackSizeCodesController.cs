using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using PSIMS.Models.PurchaseModel;

namespace PSIMS.Controllers.Purchase
{
    public class PurchasePackSizeCodesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PurchasePackSizeCodes
        public ActionResult Index()
        {
            var purchasePackSizeCodes = db.PurchasePackSizeCodes.Include(p => p.Location);
            return View(purchasePackSizeCodes.ToList());
        }

        // GET: PurchasePackSizeCodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasePackSizeCode purchasePackSizeCode = db.PurchasePackSizeCodes.Find(id);
            if (purchasePackSizeCode == null)
            {
                return HttpNotFound();
            }
            return View(purchasePackSizeCode);
        }

        // GET: PurchasePackSizeCodes/Create
        public ActionResult Create()
        {
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "LocationCode");
            return View();
        }

        // POST: PurchasePackSizeCodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PackSize_Code,UserID,LocationID,CreatedOn")] PurchasePackSizeCode purchasePackSizeCode)
        {
            if (ModelState.IsValid)
            {
                db.PurchasePackSizeCodes.Add(purchasePackSizeCode);
                purchasePackSizeCode.UserID = User.Identity.GetUserId();
                purchasePackSizeCode.CreatedOn = DateTime.Now;
                purchasePackSizeCode.LocationID = Convert.ToInt32(Session["LocationID"]);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationID = new SelectList(db.Locations, "ID", "LocationCode", purchasePackSizeCode.LocationID);
            return View(purchasePackSizeCode);
        }

        // GET: PurchasePackSizeCodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasePackSizeCode purchasePackSizeCode = db.PurchasePackSizeCodes.Find(id);
            if (purchasePackSizeCode == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "LocationCode", purchasePackSizeCode.LocationID);
            return View(purchasePackSizeCode);
        }

        // POST: PurchasePackSizeCodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PackSize_Code,UserID,LocationID,CreatedOn")] PurchasePackSizeCode purchasePackSizeCode)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchasePackSizeCode).State = EntityState.Modified;
                purchasePackSizeCode.UserID = User.Identity.GetUserId();
                purchasePackSizeCode.CreatedOn = DateTime.Now;
                purchasePackSizeCode.LocationID = Convert.ToInt32(Session["LocationID"]);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "LocationCode", purchasePackSizeCode.LocationID);
            return View(purchasePackSizeCode);
        }

        // GET: PurchasePackSizeCodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PurchasePackSizeCode purchasePackSizeCode = db.PurchasePackSizeCodes.Find(id);
            if (purchasePackSizeCode == null)
            {
                return HttpNotFound();
            }
            return View(purchasePackSizeCode);
        }

        // POST: PurchasePackSizeCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PurchasePackSizeCode purchasePackSizeCode = db.PurchasePackSizeCodes.Find(id);
            db.PurchasePackSizeCodes.Remove(purchasePackSizeCode);
            db.SaveChanges();
            return RedirectToAction("Index");
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
