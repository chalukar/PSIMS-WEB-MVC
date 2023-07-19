using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using System.IO;
using PSIMS.Repository;
using PSIMS.Service;
using PSIMS.ViewModel;

namespace PSIMS.Controllers
{
     [Authorize]
    public class SalesController : Controller
    {
        private SalesEntryRepository repo = new SalesEntryRepository();
        private SalesEntryService service = new SalesEntryService();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Sales
        public ActionResult SalesHistory()
        {   
            
            //return View(db.Sales.OrderByDescending(s=>s.ID).ToList());

            int _year = DateTime.Now.Year;
            int _month = DateTime.Now.Month;

            var customerSales = (from s in db.Sales
                                 //where s.IsActive == 1
                                 where s.Date.Year == _year && s.Date.Month == _month
                                 select s);

            return View(customerSales.OrderByDescending(s=>s.ID).ToList());
        }

        [HttpPost]
        public ActionResult SalesHistory(SalesSearchVM PSVM)
        {
            var filter = new SalesFilterRepository();
            var model = filter.FilterSalesHistory(PSVM);

            return View(model);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _salesItems = (from si in db.SalesItems
                              where si.SalesID == id
                              select si).ToList();

            if (_salesItems == null)
            {
                return HttpNotFound();
            }
            return View(_salesItems.ToList());
        }

        

        // GET: Sales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PSIMS.Models.Sales sales = db.Sales.Find(id);
            if (sales == null)
            {
                return HttpNotFound();
            }
            return View(sales);
        }

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Date,Amount,Discount,Tax,GrandTotal,UserID,Remarks")]PSIMS.Models.Sales sales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sales);
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
