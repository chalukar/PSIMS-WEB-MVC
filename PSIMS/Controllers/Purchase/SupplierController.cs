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
using PSIMS.Repository;
using PSIMS.Models.SalesModel;
using Microsoft.AspNet.Identity;
using PSIMS.ViewModel;
using System.IO;

namespace PSIMS.Controllers
{
     [Authorize]
    public class SupplierController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Supplier
        public ActionResult Index()
        {
            var supplier = db.Suppliers.Include(i => i.country).OrderByDescending(i => i.ID);
            return View(supplier.ToList());
            //return View(db.Suppliers.OrderByDescending(s=>s.ID).ToList());
        }

        // GET: Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            
           Supplier supp = new Supplier();

           supp.CreateBy = User.Identity.GetUserName();
           ViewBag.CountryID = new SelectList(db.Countries.OrderByDescending(m => m.CountryID), "CountryID", "CountryName");
           return View(supp);
        }


        public ActionResult _SupplierCreatePV()
        {
            return PartialView();
        }
        

        // POST: Supplier/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Supplier supplier)
        {

            SupplierRepository repo = new SupplierRepository();
            if (ModelState.IsValid)
            {                
                int countSupplier = repo.SupplierDuplicationCheck(supplier);

                if (Request.IsAjaxRequest())
                {
                    if (countSupplier > 0)
                    {
                        ViewBag.DuplicateError = "Already Exists!";
                        return Json("duplicate" , JsonRequestBehavior.AllowGet);
                    }
                    
                    //Add supplier to dataSet
                    db.Suppliers.Add(supplier);
                    //save changes ToString database
                    db.SaveChanges();
                    //redirect to index 
                    return Json("success", JsonRequestBehavior.AllowGet);
                }


                //If already exists. display an error.
                if (countSupplier > 0)
                {
                    ViewBag.DuplicateError = "Already Exists!";
                    return View(supplier);
                }
                supplier.CreateBy = User.Identity.GetUserId();
                supplier.CreatedOn = DateTime.Now;

                //Add supplier to dataSet
                db.Suppliers.Add(supplier);
                //save changes ToString database
                db.SaveChanges();
                //redirect to index        
                return RedirectToAction("Index");
            }


           

            return View(supplier);
        }

        // GET: Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            Supplier supplier = new Supplier();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier = db.Suppliers.Where(s => s.CountryID == id).FirstOrDefault();
            ViewBag.CountryList = db.Countries.Distinct().Select(i => new SelectListItem() { Text = i.CountryName, Value = i.CountryID.ToString() }).ToList();

            supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            
            return View(supplier);
        }

        // POST: Supplier/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Supplier supplier)
        {

            
            if (ModelState.IsValid &&  supplier != null)
            {

                var original = db.Suppliers.Find(supplier.ID);

                if(original.SupplierName != supplier.SupplierName)
                {
                    SupplierRepository repo = new SupplierRepository();
                    int countSupplier = repo.SupplierDuplicationCheck(supplier);
                    //If already exists. display an error.
                    if (countSupplier > 0)
                    {
                        ViewBag.DuplicateError = "Already Exists! --";
                        return View(supplier);
                    }
                }
                supplier.LastUpdateBy = User.Identity.GetUserId();
                supplier.LastUpdateOn = DateTime.Now;
                db.Entry(original).CurrentValues.SetValues(supplier);
                db.Entry(original).Property(x => x.CreatedOn).IsModified = false;
                db.Entry(original).Property(x => x.CreateBy).IsModified = false;

                //Save changes to Database
                db.SaveChanges();
                //redirect to Index page
                return RedirectToAction("Index");
            }

            return View(supplier);         
        }

        // GET: Supplier/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }

            return View(supplier);
        }

        // POST: Supplier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public FileContentResult ExportToCSV()
        {
            var supplier = db.Suppliers.ToList();
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"Supplier ID\",\"Supplier Name\",\"Company Name\",\"Registration No\",\"Telephone No\",\"Mobile No\",\"Fax No\",\"Email Address\",\"Address\",\"Address line2\",\"City\",\"State\",\"Status\",\"Create By\",\"Created By\",\"LastUpdate By\",\"LastUpdated On\"");
            foreach (var supp in supplier)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\"",
                     supp.ID,
                     supp.SupplierName,
                     supp.Companyname,
                     supp.Registation_No,
                     supp.TelPhoneNo,
                     supp.MobileNo,
                     supp.FaxNo,
                     supp.Email,
                     supp.Address,
                     supp.Address_line2,
                     supp.City,
                     supp.State,
                     supp.status,
                     supp.CreateBy,
                     supp.CreatedOn,
                     supp.LastUpdateBy,
                     supp.LastUpdateOn));
            }
            var fileName = "SupplierList" + DateTime.Now.ToString() + ".csv";
            return File(new System.Text.UTF8Encoding().GetBytes(sw.ToString()), "text/csv", fileName);
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
