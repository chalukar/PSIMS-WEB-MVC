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
using System.IO;

namespace PSIMS.Controllers.Sales
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.OrderByDescending(c=> c.ID). ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            var UName = User.Identity.GetUserName();
            Customer cust = new Customer();
            cust.CreateOn = DateTime.Now;
            cust.CreateBy = UName;
            return View(cust);
        }


        public ActionResult _CustomerCreatePV()
        {
            return PartialView();
        }
        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            CustomerRepository repo = new CustomerRepository();

            if (ModelState.IsValid)
            {
                int CountCustomer = repo.CustomerDuplicationCheck(customer);

                if(Request.IsAjaxRequest())
                {
                    if(CountCustomer >0)
                    {
                        ViewBag.DuplicateError = "Already Exists";
                        return Json("duplicate", JsonRequestBehavior.AllowGet);
                    }
                    
                    //Add supplier to dataSet
                    db.Customers.Add(customer);
                    //save changes ToString database
                    db.SaveChanges();
                    //redirect to index 
                    return Json("success", JsonRequestBehavior.AllowGet);

                }

                //If already exists. display an error.
                if(CountCustomer>0)
                {
                    ViewBag.DuplicateError = "Already Exists";
                    return View(customer);

                }
                customer.CreateBy = User.Identity.GetUserId();
                customer.CreateOn = DateTime.Now;
                customer.CustNameIsActive = true;
                //Add supplier to dataSet
                db.Customers.Add(customer);
                //save changes ToString database

                db.SaveChanges();
                if (customer.File != null)
                {
                    customer.File.SaveAs(Server.MapPath("~/IMG/") + customer.ID + ".jpg");
                }  
                //redirect to index
                
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        //----------------------------------------------------
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,CustomerName,TelPhoneNo,MobileNo,Email,Companyname,Address,Address_line2,City,State,Gender,CreateBy,CreateOn")] Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Customers.Add(customer);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(customer);
        //}
        //----------------------------------------------------

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(Customer customer)
        {


            if (ModelState.IsValid && customer != null)
            {

                var original = db.Customers.Find(customer.ID);

                if (original.CustomerName != customer.CustomerName)
                {
                    CustomerRepository repo = new CustomerRepository();
                    int countCustomer = repo.CustomerDuplicationCheck(customer);
                    //If already exists. display an error.
                    if (countCustomer > 0)
                    {
                        ViewBag.DuplicateError = "Already Exists!";
                        return View(customer);
                    }
                }
                customer.LastUpdateBy = User.Identity.GetUserId();
                customer.LastUpdateOn = DateTime.Now;
               
                db.Entry(original).CurrentValues.SetValues(customer);
                db.Entry(original).Property(x => x.CreateOn).IsModified = false;
                db.Entry(original).Property(x => x.CreateBy).IsModified = false;
                 
                //Save changes to Database
                db.SaveChanges();
                if (customer.File != null)
                {
                    customer.File.SaveAs(Server.MapPath("~/IMG/") + customer.ID + ".jpg");

                } 
                //redirect to Index page
                
                return RedirectToAction("Index");
            }
           
            return View(customer);
        }
        //---------------------------------------------------
        //public ActionResult Edit([Bind(Include = "ID,CustomerName,TelPhoneNo,MobileNo,Email,Companyname,Address,Address_line2,City,State,Gender,CreateBy,CreateOn")] Customer customer)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(customer).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(customer);
        //}
        //----------------------------------------------------


        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public FileContentResult ExportToCSV()
        {
            var customer = db.Customers.ToList();
            StringWriter sw = new StringWriter();
            sw.WriteLine("\"Customer ID\",\"Customer Name\",\"Company Name\",\"Registration No\",\"Telephone No\",\"Mobile No\",\"Fax No\",\"Email Address\",\"Address\",\"Address line2\",\"City\",\"State\",\"Latitude\",\"Longitude\",\"Status\",\"Create By\",\"Create On\",\"Last Update By\",\"Last Update On\"");
            foreach (var cut in customer)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",\"{12}\",\"{13}\",\"{14}\",\"{15}\",\"{16}\",\"{17}\",\"{18}\"",
                     cut.ID,
                     cut.CustomerName,
                     cut.Companyname,
                     cut.Registation_No,
                     cut.TelPhoneNo,
                     cut.MobileNo,
                     cut.FaxNo,
                     cut.Email,
                     cut.Address,
                     cut.Address_line2,
                     cut.City,
                     cut.State,
                     cut.Latitude,
                     cut.Longitude,
                     cut.Status,
                     cut.CreateBy,
                     cut.CreateOn,
                     cut.LastUpdateBy,
                     cut.LastUpdateOn));
            }
            var fileName = "CustomerList" + DateTime.Now.ToString() + ".csv";
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
