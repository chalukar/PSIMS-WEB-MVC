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
    public class StockMovementDetalsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StockMovementDetals
        public ActionResult Index()
        {
            var stockMovementDetals = db.StockMovementDetals.Include(s => s.FromLocation).Include(s => s.Item).Include(s => s.Stock).Include(s => s.ToLocation);
            return View(stockMovementDetals.ToList());
        }

        // GET: StockMovementDetals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMovementDetals stockMovementDetals = db.StockMovementDetals.Find(id);
            if (stockMovementDetals == null)
            {
                return HttpNotFound();
            }
            return View(stockMovementDetals);
        }

        // GET: StockMovementDetals/Create
        public ActionResult Create()
        {
            ViewBag.FromLocationID = new SelectList(db.Locations, "ID", "LocationCode");
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name");
            ViewBag.StockID = new SelectList(db.Stocks, "ID", "BatchNo");
            ViewBag.ToLocationID = new SelectList(db.Locations, "ID", "LocationCode");
            return View();
        }

        // POST: StockMovementDetals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,TransferID,ItemID,StockID,FromLocationID,ToLocationID,InitQty,DistributedQty,Remarks,TransferdBy,TransferdOn,ReceivedBy,ReceivedOn,ReceivedQty,Status,LocationID")] StockMovementDetals stockMovementDetals)
        {
            if (ModelState.IsValid)
            {
                db.StockMovementDetals.Add(stockMovementDetals);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FromLocationID = new SelectList(db.Locations, "ID", "LocationCode", stockMovementDetals.FromLocationID);
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name", stockMovementDetals.ItemID);
            ViewBag.StockID = new SelectList(db.Stocks, "ID", "BatchNo", stockMovementDetals.StockID);
            ViewBag.ToLocationID = new SelectList(db.Locations, "ID", "LocationCode", stockMovementDetals.ToLocationID);
            return View(stockMovementDetals);
        }

        // GET: StockMovementDetals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMovementDetals stockMovementDetals = db.StockMovementDetals.Find(id);
            if (stockMovementDetals == null)
            {
                return HttpNotFound();
            }
            ViewBag.FromLocationID = new SelectList(db.Locations, "ID", "LocationCode", stockMovementDetals.FromLocationID);
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name", stockMovementDetals.ItemID);
            ViewBag.StockID = new SelectList(db.Stocks, "ID", "BatchNo", stockMovementDetals.StockID);
            ViewBag.ToLocationID = new SelectList(db.Locations, "ID", "LocationCode", stockMovementDetals.ToLocationID);
            return View(stockMovementDetals);
        }

        // POST: StockMovementDetals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TransferID,ItemID,StockID,FromLocationID,ToLocationID,InitQty,DistributedQty,Remarks,TransferdBy,TransferdOn,ReceivedBy,ReceivedOn,ReceivedQty,Status,LocationID")] StockMovementDetals stockMovementDetals)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockMovementDetals).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FromLocationID = new SelectList(db.Locations, "ID", "LocationCode", stockMovementDetals.FromLocationID);
            ViewBag.ItemID = new SelectList(db.Items, "ID", "Name", stockMovementDetals.ItemID);
            ViewBag.StockID = new SelectList(db.Stocks, "ID", "BatchNo", stockMovementDetals.StockID);
            ViewBag.ToLocationID = new SelectList(db.Locations, "ID", "LocationCode", stockMovementDetals.ToLocationID);
            return View(stockMovementDetals);
        }

        // GET: StockMovementDetals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMovementDetals stockMovementDetals = db.StockMovementDetals.Find(id);
            if (stockMovementDetals == null)
            {
                return HttpNotFound();
            }
            return View(stockMovementDetals);
        }

        // POST: StockMovementDetals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockMovementDetals stockMovementDetals = db.StockMovementDetals.Find(id);
            db.StockMovementDetals.Remove(stockMovementDetals);
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
