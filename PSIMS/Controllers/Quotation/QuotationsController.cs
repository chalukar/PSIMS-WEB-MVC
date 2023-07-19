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
using PSIMS.Models;
using Microsoft.AspNet.Identity;

namespace PSIMS.Controllers.Quotation
{
    public class QuotationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Quotations
        public ActionResult Index()
        {
            var quotations = db.Quotations.OrderByDescending(q=>q.ID).Include(c => c.Customer);
            return View(quotations.ToList());
        }

        // GET: Quotations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _quotationItems = from q in db.QuotationItems
                                  where q.QuotationID == id
                                  select q;

            if (_quotationItems == null)
            {
                return HttpNotFound();
            }
            return View(_quotationItems);
        }

        //Header Edit
        public ActionResult HeaderEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PSIMS.Models.QuotationModel.Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();  
            }
            ViewBag.QuotationCategoryID = new SelectList(db.QuotationCategories.OrderByDescending(q => q.CategoryName), "ID", "CategoryName", quotation.QuotationCategoryID);
            ViewBag.CustomerID = new SelectList(db.Customers.OrderByDescending(c => c.CustomerName), "ID", "CustomerName", quotation.CustomerID);
            return View(quotation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HeaderEdit(PSIMS.Models.QuotationModel.Quotation quotation)
        {
            if (ModelState.IsValid && quotation != null)
            {
                var original = db.Quotations.Find(quotation.ID);

                db.Entry(original).CurrentValues.SetValues(quotation);

                db.Entry(original).Property(x => x.CreatedOn).IsModified = false;
                db.Entry(original).Property(x => x.UserID).IsModified = false;
                db.Entry(original).Property(x => x.LocationID).IsModified = false;
                //Save changes to Database
                db.SaveChanges();
                //redirect to Index page
                return RedirectToAction("Index");
            }

            return View(quotation);


        }


        // GET: Quotations/DetailsEdit/5
        public ActionResult DetailsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PSIMS.Models.QuotationModel.QuotationItem quotationitem = db.QuotationItems.Find(id);
            if (quotationitem == null)
            {
                return HttpNotFound();
            }
           // ViewBag.CustomerID = new SelectList(db.Customers, "ID", "CustomerName", quotationitem.CustomerID);
            return View(quotationitem);
        }



        // POST: Quotations/DetailsEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsEdit(PSIMS.Models.QuotationModel.QuotationItem quotationitem)
        {
            if (ModelState.IsValid && quotationitem != null)
            {
                var original = db.QuotationItems.Find(quotationitem.ID);

                db.Entry(original).CurrentValues.SetValues(quotationitem);

                
                //Save changes to Database
                db.SaveChanges();
                //redirect to Index page
                return RedirectToAction("Index");
            }

            return View(quotationitem);


        }

        

       
        // GET: Quotations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PSIMS.Models.QuotationModel.Quotation quotation = db.Quotations.Find(id);
            if (quotation == null)
            {
                return HttpNotFound();
            }
            return View(quotation);
        }

        // POST: Quotations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PSIMS.Models.QuotationModel.Quotation quotation = db.Quotations.Find(id);
            db.Quotations.Remove(quotation);
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
