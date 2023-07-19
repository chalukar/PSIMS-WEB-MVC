using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using PSIMS.Models.Finance;
using PSIMS.Repository;
using Microsoft.AspNet.Identity;

namespace PSIMS.Controllers.Finance
{
     [Authorize]

    public class BanksController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Banks
        public ActionResult Index()
        {
            return View(db.Banks.OrderByDescending(s=>s.ID).ToList());
        }

        // GET: Banks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            return View(bank);
        }

        // GET: Banks/Create
        public ActionResult Create()
        {
            return View();


        }

        // POST: Banks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bank bank)
        {
            try
            {
                BankRepository repo = new BankRepository();

                if (ModelState.IsValid)
                {
                    int CountBank = repo.BankDuplicationCheck(bank);

                    if (Request.IsAjaxRequest())
                    {
                        if (CountBank > 0)
                        {
                            ViewBag.DuplicateError = "Already Exists";
                            return Json("duplicate", JsonRequestBehavior.AllowGet);
                        }
                        //Add supplier to dataSet
                        db.Banks.Add(bank);
                        //save changes ToString database
                        db.SaveChanges();

                        //redirect to index 
                        return Json("success", JsonRequestBehavior.AllowGet);

                    }

                    //If already exists. display an error.
                    if (CountBank > 0)
                    {
                        ViewBag.DuplicateError = "Already Exists";
                        return View(bank);

                    }
                    //Add supplier to dataSet
                    bank.UserID = User.Identity.GetUserId();
                    bank.LocationID = Convert.ToInt32(Session["LocationID"]);
                    db.Banks.Add(bank);
                    //save changes ToString database

                    db.SaveChanges();
                    //redirect to index

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                
            }
            return View(bank);
           
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,BankCode,BankName")] Bank bank)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Banks.Add(bank);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(bank);
        //}

        // GET: Banks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            return View(bank);
        }

        // POST: Banks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(Bank bank)
        {


            if (ModelState.IsValid && bank != null)
            {

                var original = db.Banks.Find(bank.ID);

                if (original.BankCode != bank.BankCode)
                {
                    BankRepository repo = new BankRepository();
                    int countBank = repo.BankDuplicationCheck(bank);
                    //If already exists. display an error.
                    if (countBank > 0)
                    {
                        ViewBag.DuplicateError = "Already Exists!";
                        return View(countBank);
                    }
                }

                db.Entry(original).CurrentValues.SetValues(bank);
                db.Entry(original).Property(x => x.UserID).IsModified = false;
                db.Entry(original).Property(x => x.LocationID).IsModified = false;
               
                //Save changes to Database
                db.SaveChanges();
                //redirect to Index page

                return RedirectToAction("Index");
            }

            return View(bank);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,BankCode,BankName")] Bank bank)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(bank).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(bank);
        //}

        // GET: Banks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bank bank = db.Banks.Find(id);
            if (bank == null)
            {
                return HttpNotFound();
            }
            return View(bank);
        }

        // POST: Banks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bank bank = db.Banks.Find(id);
            db.Banks.Remove(bank);
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
