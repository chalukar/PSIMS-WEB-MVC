using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using PSIMS.Models.Locations;
using Microsoft.AspNet.Identity;
using PSIMS.Repository;

namespace PSIMS.Controllers.Locations
{
    public class LocationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Locations
        public ActionResult Index()
        {

            return View(db.Locations.OrderByDescending(b => b.ID).ToList());

        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            //var UName = User.Identity.GetUserName();
            //Location location = new Location();
            //location.CreateOn = DateTime.Now;
            //location.CreateBy = UName;
            return View();

        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create(Location location)
        {
            LocationRepository repo = new LocationRepository();

            if (ModelState.IsValid)
            {

                int CountLocation = repo.LocationDuplicationCheck(location);

                if (Request.IsAjaxRequest())
                {
                    if (CountLocation > 0)
                    {
                        ViewBag.DuplicateError = "Already Exists";
                        return Json("duplicate", JsonRequestBehavior.AllowGet);
                    }


                    //Add supplier to dataSet
                    db.Locations.Add(location);
                    //save changes ToString database
                    db.SaveChanges();
                    //redirect to index 
                    return Json("success", JsonRequestBehavior.AllowGet);

                }

                //If already exists. display an error.
                if (CountLocation > 0)
                {
                    ViewBag.DuplicateError = "Already Exists";
                    return View(location);

                }
                location.UserID = User.Identity.GetUserId();
                location.CreateOn = DateTime.Now;
                //Add supplier to dataSet
                db.Locations.Add(location);
                //save changes ToString database
                db.SaveChanges();
                //redirect to index        
                return RedirectToAction("Index");
            }
            return View(location);
          
        }
        //public ActionResult Create([Bind(Include = "ID,LocationCode,LocationName,Status,CreateBy,CreateOn")] Location location)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Locations.Add(location);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(location);
        //}

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location location)
        {


            if (ModelState.IsValid && location != null)
            {

                var original = db.Locations.Find(location.ID);

                if (original.LocationCode != location.LocationCode)
                {
                    LocationRepository repo = new LocationRepository();
                    int CountLocation = repo.LocationDuplicationCheck(location);
                    //If already exists. display an error.
                    if (CountLocation > 0)
                    {
                        ViewBag.DuplicateError = "Already Exists!";
                        return View(location);
                    }
                }

                
                db.Entry(original).CurrentValues.SetValues(location);
                db.Entry(original).Property(x => x.CreateOn).IsModified = false;

                //Save changes to Database
                db.SaveChanges();
                //redirect to Index page
                return RedirectToAction("Index");
            }

            return View(location);
        

        }
        //public ActionResult Edit([Bind(Include = "ID,LocationCode,LocationName,Status,CreateBy,CreateOn")] Location location)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(location).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(location);
        //}

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
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
