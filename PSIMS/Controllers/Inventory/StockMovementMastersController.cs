using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using PSIMS.Models.InventoryModel;

namespace PSIMS.Controllers.Inventory
{
    public class StockMovementMastersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockMovementMasters
        public ActionResult Index()
        {
            var stockMovementMasters = db.StockMovementMasters.Include(s => s.Location);
            return View(stockMovementMasters.ToList());
        }

        // GET: StockMovementMasters/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMovementMaster stockMovementMaster = db.StockMovementMasters.Find(id);
            if (stockMovementMaster == null)
            {
                return HttpNotFound();
            }
            return View(stockMovementMaster);
        }

        // GET: StockMovementMasters/Create
        public ActionResult Create()
        {
            ViewBag.locationID = new SelectList(db.Locations, "ID", "LocationCode");
            return View();
        }

        // POST: StockMovementMasters/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,locationID,Date")] StockMovementMaster stockMovementMaster)
        {
            if (ModelState.IsValid)
            {
                db.StockMovementMasters.Add(stockMovementMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.locationID = new SelectList(db.Locations, "ID", "LocationCode", stockMovementMaster.locationID);
            return View(stockMovementMaster);
        }

        // GET: StockMovementMasters/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMovementMaster stockMovementMaster = db.StockMovementMasters.Find(id);
            if (stockMovementMaster == null)
            {
                return HttpNotFound();
            }
            ViewBag.locationID = new SelectList(db.Locations, "ID", "LocationCode", stockMovementMaster.locationID);
            return View(stockMovementMaster);
        }

        // POST: StockMovementMasters/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,locationID,Date")] StockMovementMaster stockMovementMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockMovementMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.locationID = new SelectList(db.Locations, "ID", "LocationCode", stockMovementMaster.locationID);
            return View(stockMovementMaster);
        }

        // GET: StockMovementMasters/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMovementMaster stockMovementMaster = db.StockMovementMasters.Find(id);
            if (stockMovementMaster == null)
            {
                return HttpNotFound();
            }
            return View(stockMovementMaster);
        }

        // POST: StockMovementMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            StockMovementMaster stockMovementMaster = db.StockMovementMasters.Find(id);
            db.StockMovementMasters.Remove(stockMovementMaster);
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
