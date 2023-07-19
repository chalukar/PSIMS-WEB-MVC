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
using PSIMS.Models.SalesModel;
using PSIMS.ViewModel;
using PSIMS.Repository;
using PSIMS.Service;
using Microsoft.AspNet.Identity;

namespace PSIMS.Controllers.Finance
{
    [Authorize]
    public class PaymentsController : Controller
    {
        private SalesEntryRepository repo = new SalesEntryRepository();
        private SalesEntryService service = new SalesEntryService();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Payments
        public ActionResult Index()
        {
             int UserLocationID = Convert.ToInt32(Session["LocationID"]);

            //with out locations
             var payments = db.Payments.Include(b => b.Bank);


            //with location
            //var payments = db.Payments.Include(b => b.Bank)
            //               .Where(p => p.LocationID == UserLocationID);

            return View(payments.ToList());
        }


        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {

            return View();
        }

            // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SalesID,GrandTotal,PaidAmount,PaymentDate,Balance,PayMode,ChequeNo,ChequeDate,Status,BankID,UserID,LocationID")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.Entry(payment).Property(x => x.UserID).IsModified = false;
                db.Entry(payment).Property(x => x.LocationID).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(payment);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //Get Data from Sales Table for Receipt
        [HttpGet]
        public JsonResult GetformPayment(string SearchVal)
        {
          List<PSIMS.Models.Sales> Saleslist = new List<PSIMS.Models.Sales>();


          int id = Convert.ToInt32(SearchVal);

           Saleslist = db.Sales.Where(x => x.ID == id).ToList();
           return Json(new
           {
               ID = Saleslist[0].ID,
               GrandTotal = Saleslist[0].GrandTotal,
               ComName = Saleslist[0].Customer.Companyname
               

           }, JsonRequestBehavior.AllowGet);


        }


        //Get Pass Data from Payment Table
        [HttpGet]
        public JsonResult GetPassdata(string SearchVal)
        {
            int _salesID = Convert.ToInt32(SearchVal);
            List<PSIMS.Models.Finance.Payment> paymentlist = new List<PSIMS.Models.Finance.Payment>();
            List<PSIMS.Models.Finance.Payment> _bfLastpay = new List<PSIMS.Models.Finance.Payment>();
            

            paymentlist = db.Payments.OrderByDescending(x => x.ID)
                .Where(x => x.SalesID == _salesID).ToList();

            if ( paymentlist.Count ==1)
            {
                _bfLastpay = db.Payments.OrderByDescending(x => x.ID)
                         .Where(x => x.SalesID == _salesID).Take(1).ToList();
               return Json(new
                {
                    ID = paymentlist[0].SalesID,
                    balance = _bfLastpay[0].GrandTotal,
                    LastPayment = paymentlist[0].PaidAmount,
                    Arrears = paymentlist[0].Balance


                }, JsonRequestBehavior.AllowGet);
            }
            if (paymentlist.Count > 1)
            {
                _bfLastpay = db.Payments.OrderByDescending(x => x.ID)
                         .Where(x => x.SalesID == _salesID).Take(2).Skip(1).ToList();
                return Json(new
                {
                    ID = paymentlist[0].SalesID,
                    balance = _bfLastpay[0].Balance,
                    LastPayment = paymentlist[0].PaidAmount,
                    Arrears = paymentlist[0].Balance


                }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                List<PSIMS.Models.Sales> Saleslist = new List<PSIMS.Models.Sales>();

                int id = Convert.ToInt32(SearchVal);

                Saleslist = db.Sales.Where(x => x.ID == id).ToList();
                return Json(new
                {
                    ID = Saleslist[0].ID,

                }, JsonRequestBehavior.AllowGet);
            }
            
           


        }



        [HttpGet]
        public JsonResult GetformPaymentlist(string SearchVal)
        {
           // List<PSIMS.Models.Sales> Saleslist = new List<PSIMS.Models.Sales>();
             List<PSIMS.Models.Finance.Payment> Paymentlist = new List<PSIMS.Models.Finance.Payment>();

             int id = Convert.ToInt32(SearchVal);

             Paymentlist = db.Payments.Where(x => x.ID == id).ToList();
            return Json(new
            {
                ID = Paymentlist[0].ID,
                GrandTotal = Paymentlist[0].GrandTotal,
                PaymentReceived = Paymentlist[0].PaidAmount


            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SerializePaymentFormData(FormCollection _paycollection)
        {
            //List<PSIMS.Models.Sales> custID = new List<PSIMS.Models.Sales>();
             decimal currBalance=0;
            if (_paycollection != null)
            {
                //string[] _InvoiceID, _InvoiceAmt, _ReceiptAmt, _PayMode;
                DateTime? _chqueDate = DateTime.Today;
                int? _bankID=0;
                string _chqueNo =null;
               
                int _InvoiceID = Convert.ToInt32(_paycollection["invoiceNo"]);
                string _ComName = _paycollection["ComName"];
                decimal _InvoiceAmt = Convert.ToDecimal(_paycollection["inamount"]);
                decimal _ReceiptAmt = Convert.ToDecimal(_paycollection["receiptamount"]);
                string _PayMode = _paycollection["PaymentMode"];
                string _userID = User.Identity.GetUserId();
                int _LocationID = Convert.ToInt32(Session["LocationID"]);
                decimal _arrears = Convert.ToDecimal(_paycollection["arrears"]);
                
                
                if (_PayMode.Equals("CH"))
                {
                    _bankID = Convert.ToInt32(_paycollection["selectBank"]);
                    _chqueNo = _paycollection["chqNo"];
                    _chqueDate = Convert.ToDateTime(_paycollection["chqDate"]);
                }
                else if (_PayMode.Equals("CA"))
                {
                    _chqueDate = null;
                    _chqueNo = null;
                    if (Convert.ToInt32(_paycollection["selectBank"]) == 0)
                    {
                        _bankID = null;
                    }
                    else
                    {
                        _bankID = Convert.ToInt32(_paycollection["selectBank"]);
                    }

                }

                 int _custID = (from s in db.Sales
                              where s.ID == _InvoiceID
                              select s.CustomerID).SingleOrDefault();

                DateTime _paymentDate = DateTime.Now;

                if (_arrears == 0)
                {
                    currBalance = _InvoiceAmt - _ReceiptAmt;
                }
                else
                {
                    currBalance = _arrears - _ReceiptAmt;
                }

                Payment _payment = new Payment()
               {
                   SalesID = _InvoiceID,
                   CompanyName = _ComName,
                   GrandTotal = _InvoiceAmt,
                   PaidAmount = _ReceiptAmt,
                   PaymentDate = _paymentDate,
                   PayMode = _PayMode,
                   ChequeNo = _chqueNo,
                   ChequeDate = _chqueDate,
                   BankID = _bankID,
                   UserID = _userID,
                   LocationID = _LocationID,
                   Status = true,
                   CustomerID = _custID,
                   Balance = currBalance,

               };

                int salesPayment = service.InsertPaymentSales(_payment);
                return Json(salesPayment);

            }
            return Json("null"); 
        }

        [HttpPost]
        public ActionResult formPayment(ReceiptListVM receiptlist)
        {
            var filter = new ReceiptFilterRepository();
            var model = filter.FilterReceipt(receiptlist);
            ViewBag.ReceiptList = model;
            return View();

        }
        //public ActionResult formPayment()
        //{
        //   // ViewBag.ReceiptList = ReceiptListCount(); //Today's Sales - Product Wise

        //    return View();

        //}
        #region SalesReceipt Normal
        public ActionResult SalesReceipt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _salesReceipt = service.GetSalesReceipt(id);

            
            if (_salesReceipt == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                
                return View(_salesReceipt);
            }

        }
        #endregion

        #region SalesReceipt Design Print
        public ActionResult SalesReceipt1(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _salesReceipt = service.GetSalesReceipt(id);


            if (_salesReceipt == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {

                return View(_salesReceipt);
            }

        }
        #endregion
        public JsonResult PopulateBank()
        {
            //queries all the Bank in the database
            var bankList = (from i in db.Banks.OrderBy(b => b.BankName)
                            select new { i.ID, i.BankCode,i.BankName }).ToList();
            //holds the list of bank
            List<Bank> _bank = new List<Bank>();

            //save the list of Banks to the _item.
            foreach (var item in bankList)
            {
                _bank.Add(new Bank
                {
                    ID = item.ID,
                    BankCode =item.BankCode,
                    BankName = item.BankName
                });
            }
            //returns the list of Banks in Json form 
            return Json(_bank, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetformPayment(string SerachBy,string SerachValue)
        //{
        //    List<PSIMS.Models.Sales> Saleslist = new List<PSIMS.Models.Sales>();
        //    if (SerachBy == "InvoiceNo")
        //    {
        //        try
        //        {
        //            int id = Convert.ToInt32(SerachValue);
        //            Saleslist = db.Sales.Where(x => x.ID == id || SerachValue == null).ToList();
        //        }
        //        catch (FormatException)
        //        {
        //            Console.WriteLine("{0} IS not a ID ", SerachValue);
        //        }
        //        return Json(Saleslist, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return null;
 
        //    }

       
            
        //}
    }
}
