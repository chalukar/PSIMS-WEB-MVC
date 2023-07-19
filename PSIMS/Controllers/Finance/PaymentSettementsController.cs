using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PSIMS.Models.Finance;
using PSIMS.Models.SalesModel;
using PSIMS.Repository;
using PSIMS.Repository.Reports;
using PSIMS.Service;
using PSIMS.ViewModel;
using PSIMS.ViewModel.ForReports;

namespace PSIMS.Controllers.Finance
{
    public class PaymentSettementsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        FinanceRepository repo = new FinanceRepository();
        FinanceService service = new FinanceService();

        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        private UserManager<ApplicationUser> manager = null;
        private RoleManager<IdentityRole> roleManager = null;


        public PaymentSettementsController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        }
        public ActionResult PaymentReceiptForSales()
        {
            var today = DateTime.Today;
            int _year = DateTime.Now.Year;
            int _month = DateTime.Now.Month;

            //var paymentSettements = db.Sales
            //                        .Where(s => s.Date.Month == _month)
            //                        .Where(s => s.Date.Year == _year)
            //                        .Where(s => s.IsActive == 1)
            //                        .where(s => s.PaymentType == 0 && s.unitbalance == 0 && s.LastReceiptAmt == 0) || (s.PaymentType == 1 && s.unitbalance != 0)).ToList();
            ////.Where((s => s.PaymentType == 0 || s.PaymentType == 1 && s.unitbalance == 0 || s.unitbalance != 0)).ToList();

            var paymentSettements = (from s in db.Sales.Include(c => c.Customer)
                                     where s.IsActive == 1
                                     where s.Date.Year == _year && s.Date.Month == _month
                                     where (s.PaymentType == "0" && s.unitbalance == 0 && s.LastReceiptAmt == 0) || (s.PaymentType == "PP" && s.unitbalance != 0)
                                     select s).OrderByDescending(s =>s.ID);

            return View(paymentSettements.Where(s =>s.Date.Year == _year).Where(s => s.Date.Month == _month));

        }

        [HttpPost]
        public JsonResult PendingPaymentSerializeFormData(FormCollection _collection)
        {
            bool status = false;
            if (_collection != null)
            {
                //Get local Date Time
                string dt = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
                string[] _invID, _invDate, _loc, _custID, _Paytype, _invAmt, _invbal, _RecptAmnt;
                int _passCustID = 0;

                //bool _statusdetl = true;
                int? _bankcode = 0; int _cheq = 0; DateTime? _chqueDate = null; DateTime _DepositDate = DateTime.Now;

                _invID = _collection["InvoiceID"].Split(',');
                _invDate = _collection["InvoiceDate"].Split(',');
                _loc = _collection["location"].Split(',');
                _custID = _collection["CustID"].Split(',');
                _Paytype = _collection["pmttype"].Split(',');
                _invAmt = _collection["grandTot"].Split(',');
                _invbal = _collection["InvBalance"].Split(',');
                _RecptAmnt = _collection["totRecpPyt"].Split(',');

                decimal _totalReceipt = Convert.ToDecimal(_collection["Total"]);
                decimal _CustAmount = Convert.ToDecimal(_collection["custAmount"]);
                string _invNos = _collection["setInvNos"];
                int _paymode = Convert.ToInt32(_collection["PaymentMode"]);

                if (_paymode.Equals(2)) // Chequs
                {
                    _bankcode = Convert.ToInt32(_collection["selectBank"]);
                    _cheq = Convert.ToInt32(_collection["chqNo"]);
                    _DepositDate = Convert.ToDateTime(_collection["ChqDate"]);
                }
                else if (_paymode.Equals(1)) // Cash
                {
                    //_bankcode = null;
                    _bankcode = Convert.ToInt32(_collection["selectBank_cashbnk"]);

                    _DepositDate = Convert.ToDateTime(_collection["depositDate_cashbank"]);
                    _cheq = 0;
                    _chqueDate = null;
                }
                DateTime _Receiptdate = Convert.ToDateTime(dt);
                int _CustomerID = Convert.ToInt32(_collection["toAddress"]);
                int _ReceiptlocationID = Convert.ToInt32(Session["LocationID"]);
                DateTime? _CreatedOn = DateTime.Now;
                string _CreatedBy = User.Identity.GetUserName();

                PaymentSettelmentMaster _RtpPay = new PaymentSettelmentMaster()
                {
                    PaymentDate = _Receiptdate,
                    ReceiptAmount = _totalReceipt,
                    CustomerAmount = _CustAmount,
                    //Balance = (_CustAmount - _totalReceipt),
                    CustomerID = _CustomerID,
                    InvoiceNos = _invNos,
                    PaymentMode = _paymode,
                    BankID = _bankcode,
                    PaymentDepositDate = _DepositDate,
                    ChequeNo = _cheq,
                    //ChequeDate = _chqueDate,
                    LocationID = Convert.ToInt32(Session["LocationID"]),
                    CreatedOn = DateTime.Now,
                    CreatedBy = User.Identity.GetUserName(),
                    Countreocrd = 1,
                    isprint = 0,
                    IsActive = true,
                };
                //int passCustID = Convert.ToInt32(_custID);

                int count = _custID.Count();
                for (int i = 0; i < count; i++)
                {
                    _passCustID = Convert.ToInt32(_custID[i]);
                }

                if (_RtpPay.CustomerID == _passCustID)
                {
                    //Master Receipt Data
                    int ReceiptID = service.InsertReceiptMaster(_RtpPay);
                    // Master Audit Tray Data
                    int audittrayMasterID = service.auditTrayReceiptMaster(ReceiptID, _RtpPay.PaymentDate, _RtpPay.CustomerAmount, _RtpPay.ReceiptAmount, _RtpPay.CreatedBy);
                    //Details Receipt Date
                    service.InsertReceiptDetails(ReceiptID, _invID, _invDate, _loc, _custID, _Paytype, _invAmt, _RecptAmnt, _CreatedOn, _CreatedBy, audittrayMasterID, _invbal);
                    //Master Sales Table For Pending Payment
                    service.UpdateSalesPendingPayment(_invID, _Paytype, _invAmt, _RecptAmnt);
                    // Details Audit Tray Data
                    //service.auditTrayReceiptDetails(audittrayMasterID, ReceiptID, _invID, _RecptAmnt);

                    return Json(ReceiptID);

                }
                else
                {
                    return Json("2");
                }
            }
            return Json("2");

        }

        //Generates Payment Receipt
        public ActionResult PaymentReceipt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _PayReceipt = service.GetPaymentReceipt(id);
            //check if id supplied is present or not.
            if (_PayReceipt == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                return View(_PayReceipt);
            }

        }
        // Populate to Customer Name
        public JsonResult PopulateToAddress()
        {
            //holds list of suppliers
            List<Customer> _customerList = new List<Customer>();
            PSIMS.Models.SalesModel.Customer.CstStatus _status = 0;

            //queries all the suppliers for its ID and Name property.
            var customerList = (from s in db.Customers
                                where s.Status == _status
                                select new { s.ID, s.CustomerName }).ToList();

            //save list of suppliers to the _supplierList
            foreach (var item in customerList)
            {
                _customerList.Add(new Customer
                {
                    ID = item.ID,
                    CustomerName = item.CustomerName
                });
            }
            //returns the Json result of _supplierList
            return Json(_customerList, JsonRequestBehavior.AllowGet);
        }

        //Populate To Bank Name
        public JsonResult PopulateBank()
        {
            //queries all the Bank in the database
            var bankList = (from i in db.Banks.OrderBy(b => b.BankName)
                            select new { i.ID, i.BankCode, i.BankName }).ToList();
            //holds the list of bank
            List<Bank> _bank = new List<Bank>();

            //save the list of Banks to the _item.
            foreach (var item in bankList)
            {
                _bank.Add(new Bank
                {
                    ID = item.ID,
                    BankCode = item.BankCode,
                    BankName = item.BankName
                });
            }
            //returns the list of Banks in Json form 
            return Json(_bank, JsonRequestBehavior.AllowGet);
        }

        public List<SalesVM> PendingSalesAmount()
        {
            var today = DateTime.Today;
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            List<SalesVM> list = db.Sales
               .Where(s => s.Date.Month == month)
               .Where(s => s.Date.Year == year)
               .Where(s => s.PaymentType == "0" || s.PaymentType == "PP")
                .GroupBy(s => s.CustomerID)
                .Select(g =>
                    new SalesVM
                    {
                        CustomerID = g.Key,
                        GrandTotal = g.Sum(p => p.GrandTotal),

                    }).ToList();


            return list;

        }

        //Sales Invoice Search View
        [HttpPost]
        public ActionResult PaymentReceiptForSales(SalesSearchVM PSVM)
        {
            var filter = new PaymentFilterRepository();
            var model = filter.FiltercustomerSales(PSVM);

            return View(model);
        }

        // GET: PaymentSettements Receipt History
        public ActionResult ReceiptHistory()
        {
            var today = DateTime.Today;
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            var paymentview = db.PaymentSettelmentMasters
                             .Include(p => p.Bank)
                             .Include(p => p.Customer)
                             .OrderByDescending(r => r.ID);
            return View(paymentview.Where(p =>p.PaymentDate.Year ==year).Where(p => p.PaymentDate.Month == month));
            // return View(paymentview.Take(100));
        }


        [HttpPost]
        public ActionResult ReceiptHistory(PaymentSerachVM PSVM)
        {
            var filter = new PaymentFilterRepository();
            var model = filter.FilterPaymentHistoryDetails(PSVM);

            return View(model);
        }

        // GET: PaymentSettements Receipt History Details
        public ActionResult ReceiptHistoryDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var _payRecDet = (from si in db.PaymentSettelmentDetails
                              where si.PaymentSettelmentMasterID == id
                              select si).ToList();


            if (_payRecDet == null)
            {
                return HttpNotFound();
            }
            return View(_payRecDet.ToList());
        }

        // Payment Receipt Summary - Edit oage Load
        public ActionResult Edit_PaymentReceiptMaster(int? id)
        {
            PaymentSettelmentMaster paymst = new PaymentSettelmentMaster();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int _isprint = (from si in db.PaymentSettelmentMasters
                            where si.ID == id
                            select si.isprint).FirstOrDefault();
            if (_isprint == 0)
            {
                paymst = db.PaymentSettelmentMasters.Find(id);
                return View(paymst);
            }
            else
            {
                ViewBag.ErrorMessage = "1";

            }

            return View(paymst);
        }

        // Payment Receipt Summary - Edit Option
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit_PaymentReceiptMaster(PaymentSettelmentMaster pytm)
        {
            if (ModelState.IsValid && pytm != null)
            {

                PaymentSettelmentMaster _RtpPay = new PaymentSettelmentMaster()
                {
                    ID = pytm.ID,
                    PaymentDate = pytm.PaymentDate,
                    ReceiptAmount = pytm.ReceiptAmount,
                    CustomerAmount = pytm.CustomerAmount,
                    //CustomerID = pytm.CustomerID,
                    InvoiceNos = pytm.InvoiceNos,
                    PaymentMode = pytm.PaymentMode,
                    BankID = pytm.BankID,
                    ChequeNo = pytm.ChequeNo,
                    PaymentDepositDate = pytm.PaymentDepositDate,
                    LocationID = Convert.ToInt32(Session["LocationID"]),
                    LastUpdatedOn = DateTime.Now,
                    LastUpdatedBy = User.Identity.GetUserName(),
                };

                int ReceiptID = service.UpdateReceiptMaster(_RtpPay);
                int audittrayMasterID = service.auditTrayReceiptMaster_Update(ReceiptID, _RtpPay.PaymentDate, _RtpPay.CustomerAmount, _RtpPay.ReceiptAmount, User.Identity.GetUserId());

                ViewBag.ErrorMessage_Update = "Successfully Updated";
            }

            return View(pytm);
        }

        // Payment Receipt Details - Edit oage Load
        public ActionResult EditPaymentReceiptDetails(int? id)
        {
            PaymentSettelmentDetails paydet = new PaymentSettelmentDetails();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int _isprint = (from ptm in db.PaymentSettelmentDetails
                            join ptd in db.PaymentSettelmentMasters on ptm.PaymentSettelmentMasterID equals ptd.ID
                            where ptm.ID == id
                            select ptd.isprint).FirstOrDefault();

            if (_isprint == 0)
            {
                paydet = db.PaymentSettelmentDetails.Find(id);
                return View(paydet);
            }
            else
            {
                ViewBag.ErrorMessage = "1";

            }
            return View();
        }

        // Payment Receipt Details - Edit Option
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPaymentReceiptDetails(PaymentSettelmentDetails pytdt)
        {
            if (ModelState.IsValid && pytdt != null)
            {
                PaymentSettelmentDetails _RtpPay = new PaymentSettelmentDetails()
                {
                    ID = pytdt.ID, // payment Details ID
                    PaymentSettelmentMasterID = pytdt.PaymentSettelmentMasterID, // Payment Summary ID
                    InvoiceID = pytdt.InvoiceID,
                    InvoiceDate = pytdt.InvoiceDate,
                    InvGrandTot = pytdt.InvGrandTot,
                    ReceiptAmount = pytdt.ReceiptAmount,
                    UnitBalance = pytdt.UnitBalance,
                    Audit_tray_recipt_MasterID = pytdt.Audit_tray_recipt_MasterID,
                    PaymentType = pytdt.PaymentType, //Full Payment =FP , Part Payment = PP,Balance Payment = BP
                    CreatedOn = DateTime.Now,
                    CreatedBy = User.Identity.GetUserName(),


                };
                int ReceiptDetailsID = service.UpdateReceiptDetails(_RtpPay);
                int audittrayDetailsID = service.auditTrayReceiptDetails_Insert(_RtpPay.Audit_tray_recipt_MasterID, _RtpPay.PaymentSettelmentMasterID, _RtpPay.InvoiceID, _RtpPay.ReceiptAmount, ReceiptDetailsID, _RtpPay.CreatedOn, _RtpPay.CreatedBy, _RtpPay.InvGrandTot, _RtpPay.UnitBalance);
                service.UpdateSalesPendingPaymentForChangereciptDetails(pytdt.InvoiceID, pytdt.PaymentType, pytdt.ReceiptAmount, pytdt.UnitBalance);

                ViewBag.ErrorMessage_Update = "Successfully Updated";
            }

            return View(pytdt);
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
