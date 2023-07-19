using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using PSIMS.Models.QuotationModel;

namespace PSIMS.Controllers.Quotation
{
    public class QuotationCategoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: QuotationCategories
        public ActionResult Index()
        {
            return View(db.QuotationCategories.ToList());
        }

        // GET: QuotationCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuotationCategory quotationCategory = db.QuotationCategories.Find(id);
            if (quotationCategory == null)
            {
                return HttpNotFound();
            }
            return View(quotationCategory);
        }

        // GET: QuotationCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuotationCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CategoryName")] QuotationCategory quotationCategory)
        {
            if (ModelState.IsValid)
            {
                db.QuotationCategories.Add(quotationCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(quotationCategory);
        }

        // GET: QuotationCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuotationCategory quotationCategory = db.QuotationCategories.Find(id);
            if (quotationCategory == null)
            {
                return HttpNotFound();
            }
            return View(quotationCategory);
        }

        // POST: QuotationCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CategoryName")] QuotationCategory quotationCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quotationCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(quotationCategory);
        }

        // GET: QuotationCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            QuotationCategory quotationCategory = db.QuotationCategories.Find(id);
            if (quotationCategory == null)
            {
                return HttpNotFound();
            }
            return View(quotationCategory);
        }

        // POST: QuotationCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            QuotationCategory quotationCategory = db.QuotationCategories.Find(id);
            db.QuotationCategories.Remove(quotationCategory);
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
