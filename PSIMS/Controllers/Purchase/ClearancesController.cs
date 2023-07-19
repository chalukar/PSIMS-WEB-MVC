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
using Microsoft.AspNet.Identity;
using PSIMS.Repository;

namespace PSIMS.Controllers.Purchase
{
    public class ClearancesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Clearances
        public ActionResult Index()
        {
            var clearances = db.Clearances.Include(c => c.Location).OrderByDescending(c =>c.ID);
            return View(clearances.ToList());
        }

        // GET: Clearances/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clearance clearance = db.Clearances.Find(id);
            if (clearance == null)
            {
                return HttpNotFound();
            }
            return View(clearance);
        }

        // GET: Clearances/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Clearances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Clearance clearance)
        {
            ClearanceRepostory repo = new ClearanceRepostory();

            if (ModelState.IsValid)
            {

                int CountClearance = repo.ClearanceDuplicationCheck(clearance);

                if (Request.IsAjaxRequest())
                {
                    if (CountClearance > 0)
                    {
                        ViewBag.DuplicateError = "Clearance Invoice No already exists";
                        return Json("duplicate", JsonRequestBehavior.AllowGet);
                    }


                    //Add supplier to dataSet
                    db.Clearances.Add(clearance);
                    //save changes ToString database
                    db.SaveChanges();
                    //redirect to index 
                    return Json("success", JsonRequestBehavior.AllowGet);

                }

                //If already exists. display an error.
                if (CountClearance > 0)
                {
                    ViewBag.DuplicateError = "Clearance Invoice No already exists";
                    return View(clearance);

                }
                clearance.CretaedDate = DateTime.Now;
                clearance.UserID = User.Identity.GetUserId();
                clearance.LocationID = Convert.ToInt32(Session["LocationID"]);
                //Add supplier to dataSet
                db.Clearances.Add(clearance);
                //save changes ToString database
                db.SaveChanges();
                //redirect to index        
                return RedirectToAction("Index");
            }
            return View(clearance);
        }
            
        //public ActionResult Create([Bind(Include = "ID,InvoiceNo,InvoiceDate,Qty,ShippingCost,DollerPrice,ClearanceAmt,CretaedDate,UserID,LocationID")] Clearance clearance)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        clearance.CretaedDate = DateTime.Now;
        //        clearance.UserID = User.Identity.GetUserId();
        //        clearance.LocationID =Convert.ToInt32(Session["LocationID"]);
        //        db.Clearances.Add(clearance);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.LocationID = new SelectList(db.Locations, "ID", "LocationCode", clearance.LocationID);
        //    return View(clearance);
        //}

        // GET: Clearances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clearance clearance = db.Clearances.Find(id);
            if (clearance == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "LocationCode", clearance.LocationID);
            return View(clearance);
        }

        // POST: Clearances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,InvoiceNo,InvoiceDate,Qty,ShippingCost,DollerPrice,ClearanceAmt,CretaedDate,UserID,LocationID")] Clearance clearance)
        {
            if (ModelState.IsValid)
            {
                clearance.CretaedDate = DateTime.Now;
                clearance.UserID = User.Identity.GetUserId();
                clearance.LocationID = Convert.ToInt32(Session["LocationID"]);
                db.Entry(clearance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.Locations, "ID", "LocationCode", clearance.LocationID);
            return View(clearance);
        }

        // GET: Clearances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clearance clearance = db.Clearances.Find(id);
            if (clearance == null)
            {
                return HttpNotFound();
            }
            return View(clearance);
        }

        // POST: Clearances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clearance clearance = db.Clearances.Find(id);
            db.Clearances.Remove(clearance);
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
