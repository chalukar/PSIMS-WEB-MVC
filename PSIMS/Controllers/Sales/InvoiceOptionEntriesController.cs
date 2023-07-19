using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using PSIMS.Models.SalesModel;
using PSIMS.Repository;

namespace PSIMS.Controllers.Sales
{
    public class InvoiceOptionEntriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: InvoiceOptionEntries
        public ActionResult Index()
        {
            return View(db.InvoiceOptionEntries.ToList());
        }

        // GET: InvoiceOptionEntries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceOptionEntry invoiceOptionEntry = db.InvoiceOptionEntries.Find(id);
            if (invoiceOptionEntry == null)
            {
                return HttpNotFound();
            }
            return View(invoiceOptionEntry);
        }

        // GET: InvoiceOptionEntries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvoiceOptionEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InvoiceOptionEntry invoiceOptionEntry)
        {
            InvoiceTypeRepository repo = new InvoiceTypeRepository();

            if (ModelState.IsValid)
            {

                int CountInvoiceType = repo.InvoiceTypeDuplicationCheck(invoiceOptionEntry);

                if (Request.IsAjaxRequest())
                {
                    if (CountInvoiceType > 0)
                    {
                        ViewBag.DuplicateError = "Already Exists";
                        return Json("duplicate", JsonRequestBehavior.AllowGet);
                    }


                    //Add InvoiceOptionEntries to dataSet
                    db.InvoiceOptionEntries.Add(invoiceOptionEntry);
                    //save changes ToString database
                    db.SaveChanges();
                    //redirect to index 
                    return Json("success", JsonRequestBehavior.AllowGet);

                }

                //If already exists. display an error.
                if (CountInvoiceType > 0)
                {
                    ViewBag.DuplicateError = "Already Exists";
                    return View(invoiceOptionEntry);

                }
                //Add supplier to dataSet
                db.InvoiceOptionEntries.Add(invoiceOptionEntry);
                //save changes ToString database
                db.SaveChanges();
                //redirect to index        
                return RedirectToAction("Index");
            }
            return View(invoiceOptionEntry);
        }
        //public ActionResult Create([Bind(Include = "InvOptID,InvoiceName")] InvoiceOptionEntry invoiceOptionEntry)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.InvoiceOptionEntries.Add(invoiceOptionEntry);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(invoiceOptionEntry);
        //}

        // GET: InvoiceOptionEntries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceOptionEntry invoiceOptionEntry = db.InvoiceOptionEntries.Find(id);
            if (invoiceOptionEntry == null)
            {
                return HttpNotFound();
            }
            return View(invoiceOptionEntry);
        }

        // POST: InvoiceOptionEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InvoiceOptionEntry invoiceOptionEntry)
        {


            if (ModelState.IsValid && invoiceOptionEntry != null)
            {

                var original = db.InvoiceOptionEntries.Find(invoiceOptionEntry.InvOptID);

                if (original.InvoiceName != invoiceOptionEntry.InvoiceName)
                {
                    InvoiceTypeRepository repo = new InvoiceTypeRepository();
                    int CountInvoiceType = repo.InvoiceTypeDuplicationCheck(invoiceOptionEntry);

                    //If already exists. display an error.
                    if (CountInvoiceType > 0)
                    {
                        ViewBag.DuplicateError = "Already Exists!";
                        return View(invoiceOptionEntry);
                    }
                }


                db.Entry(original).CurrentValues.SetValues(invoiceOptionEntry);
                //Save changes to Database
                db.SaveChanges();
                //redirect to Index page
                return RedirectToAction("Index");
            }

            return View(invoiceOptionEntry);
        }
        //public ActionResult Edit([Bind(Include = "InvOptID,InvoiceName")] InvoiceOptionEntry invoiceOptionEntry)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(invoiceOptionEntry).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(invoiceOptionEntry);
        //}

        // GET: InvoiceOptionEntries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceOptionEntry invoiceOptionEntry = db.InvoiceOptionEntries.Find(id);
            if (invoiceOptionEntry == null)
            {
                return HttpNotFound();
            }
            return View(invoiceOptionEntry);
        }

        // POST: InvoiceOptionEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvoiceOptionEntry invoiceOptionEntry = db.InvoiceOptionEntries.Find(id);
            db.InvoiceOptionEntries.Remove(invoiceOptionEntry);
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
