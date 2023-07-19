using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using PSIMS.Repository;
using PSIMS.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Transactions;
using System.Data.Entity.Core;
using System.Net;
using IdentitySample;
using Microsoft.AspNet.Identity;
using PSIMS;
using PSIMS.Models.SalesModel;
using PSIMS.Models.QuotationModel;
using PSIMS.ViewModel;

namespace PSIMS.Controllers
{
    [Authorize]
    public class SalesEntryController : Controller
    {
        private SalesEntryRepository repo = new SalesEntryRepository();
        private SalesEntryService service = new SalesEntryService();
        ApplicationDbContext db = new ApplicationDbContext();
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);

        //Sales Entry Option
        public ActionResult Sales_Option()
        {

            return View();
        }
        // GET: SalesEntry
        public ActionResult Index()
        {
            int UserLocationID = Convert.ToInt32(Session["LocationID"]);
            //var locStickID = db.LocationStocks
            //                .Where(x => x.Stock.ExpiryDate > DateTime.Now && x.FinalQty != 0 && x.LocationID == UserLocationID);

            var locStickID = (from x in db.LocationStocks
                              where x.Stock.ExpiryDate > DateTime.Now || x.Stock.ExpiryDate == null
                              where x.FinalQty != 0
                              where x.LocationID == UserLocationID
                              select x);
                              

            return View(locStickID.ToList());              
        }
        //Quotation Based Sales Entry
        public ActionResult Quote_Index()
        {
            //int UserLocationID = Convert.ToInt32(Session["LocationID"]);
            //var locStickID = db.LocationStocks
            //                .Where(x => x.Stock.ExpiryDate > DateTime.Now || x.Stock.ExpiryDate == null && x.FinalQty != 0 && x.LocationID == UserLocationID);

            //return View(locStickID.ToList());
            return View();
        }

        //Get Data from Sales Table for Receipt
        [HttpGet]
        public JsonResult GetQuotationItemList(string SearchVal)
        {
           // List<PSIMS.Models.QuotationModel.QuotationItem> _quotelist = new List<PSIMS.Models.QuotationModel.QuotationItem>();
            List<PSIMS.ViewModel.QuotationItemForSalesVM> _quotelist = new List<PSIMS.ViewModel.QuotationItemForSalesVM>();
            var QuoteCode = SearchVal;


            var quotelist = (from q in db.Quotations
                             join ql in db.QuotationItems on q.ID equals ql.QuotationID
                             join ls in db.LocationStocks on ql.LocationStockID equals ls.ID
                             where q.QuotationCode == QuoteCode
                             where q.bActive == true
                             select new {ql.ID,ql.LocationStockID, ql.ItemName,ql.ItemDesciption,ql.PackSize,ql.Unitprice,ql.QuotationPackRate,ql.QuotationTotal,ql.QuotationQty,ls.FinalQty,ls.Loc_BatchNo,ls.Loc_ExpiryDate}
                             ).ToList();
                 

            foreach (var item in quotelist)
            {
                if(item.Loc_ExpiryDate == null)
                {
                    _quotelist.Add(new QuotationItemForSalesVM
                    {


                        ID = item.ID,
                        locationStockID = item.LocationStockID,
                        ItemName = item.ItemName,
                        ItemDesciption = item.ItemDesciption,
                        PackSize = item.PackSize,
                        QuotationPackRate = item.QuotationPackRate,
                        QuotationTotal = item.QuotationTotal,
                        Unitprice = item.Unitprice,
                        QuotationQty = item.QuotationQty,
                        locationStockQty = item.FinalQty,
                        Loc_BatchNo = item.Loc_BatchNo,
                        Loc_ExpiryDate = item.Loc_ExpiryDate,

                    });

                }
                else
                {
                    _quotelist.Add(new QuotationItemForSalesVM
                    {


                        ID = item.ID,
                        locationStockID = item.LocationStockID,
                        ItemName = item.ItemName,
                        ItemDesciption = item.ItemDesciption,
                        PackSize = item.PackSize,
                        QuotationPackRate = item.QuotationPackRate,
                        QuotationTotal = item.QuotationTotal,
                        Unitprice = item.Unitprice,
                        QuotationQty = item.QuotationQty,
                        locationStockQty = item.FinalQty,
                        Loc_BatchNo = item.Loc_BatchNo,
                        Loc_ExpiryDate = item.Loc_ExpiryDate,



                    });
                }
              
            }
            //returns the Json result of _supplierList
            return Json(quotelist, JsonRequestBehavior.AllowGet);

            
        }


        //has fully functional UI 
        //public ActionResult IndexTest()
        //{
        //    return View(repo.GetAllStock());
        //}                

        ////Another test attempt
        //public ActionResult IndexTest2()
        //{

        //    return View(repo.GetAllStock());
        //}

        [HttpPost]
        public JsonResult SerializeFormData(FormCollection _collection)
        {
            if (_collection != null)
            {
                //Get local Date Time
                string dt = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
                string _UDis_Split;
                string[] discount;
                string[] _stockID, _qty, _rate, _unitDisAmt, _amt, _packSize, _discount_type;

                _UDis_Split = _collection["getDisAmt"];
                discount = _UDis_Split.Split(new string[] { "%", "," }, StringSplitOptions.RemoveEmptyEntries);
                //if (_UDis_Split.Contains(",") || _UDis_Split.Contains("%"))
                //{
                //    discount = _UDis_Split.Split(new[] { ",", "%" }, StringSplitOptions.RemoveEmptyEntries);

                //}
                //else
                //{
                //    discount = null;
                //}

                // DateTime? _ChequeDepositDate=DateTime.Today,_ChequeDate=DateTime.Today;
                //for salesItem
                _stockID = _collection["StockID"].Split(',');
                _qty = _collection["Qty"].Split(',');
                _rate = _collection["Rate"].Split(',');
                _unitDisAmt = discount;
                _amt = _collection["Amount"].Split(',');
                _packSize = _collection["PackSize"].Split(',');
                _discount_type = _collection["disc_type"].Split(',');

                //for sales
                decimal _total =  Convert.ToDecimal(_collection["Total"]);
                //decimal _discount = Convert.ToDecimal("0");
                decimal _tax = Convert.ToDecimal(_collection["salestaxAmt"]);
                string _remarks = _collection["remarks"];
                string _referance = _collection["referance"];
                decimal _grandTotal = Convert.ToDecimal(_collection["GrandTotal"]);
                DateTime _date = Convert.ToDateTime(dt);
                //For Customer Name  but its null 
                int _customerID = Convert.ToInt32(_collection["CustName"]);
                int _InvoiceOptionID = Convert.ToInt32(_collection["InvOpt"]);
                string _userID = User.Identity.GetUserId();
                int _locationID = Convert.ToInt32(Session["LocationID"]);


                // string _PaymentMode = _collection["PaymentMode"];
                // string _chequeNo = _collection["ChqNo"];
                //if (_PaymentMode.Equals("CH"))
                //{
                //    _ChequeDepositDate = Convert.ToDateTime(_collection["ChqDepDate"]);
                //    _ChequeDate = Convert.ToDateTime(_collection["ChqDate"]);
                //}
                //else if (_PaymentMode.Equals("CR"))
                //{
                //    //_ChequeDepositDate = Convert.ToDateTime(_collection["ChqDepDate"]);
                //    //_ChequeDate = Convert.ToDateTime(_collection["ChqDate"]);
                //    _ChequeDepositDate = null;
                //    _ChequeDate = null;
                //}
                //else if (_PaymentMode.Equals("CA"))
                //{
                //    _ChequeDepositDate =null;
                //    _ChequeDate = null;

                //}


                //instance of the global class
                //MvcApplication app = new MvcApplication();
                PSIMS.Models.Sales _sales = new PSIMS.Models.Sales()
                {
                    Date = _date,
                    Amount = _total,
                    Discount = 0,
                    GrandTotal = _grandTotal,
                    Tax = _tax,
                    UserID = User.Identity.GetUserId(),
                    Remarks = _remarks,
                    CustomerID = _customerID,
                    InvOptID = _InvoiceOptionID,
                    IsActive = 1,
                    LocationID = Convert.ToInt32(Session["LocationID"]),
                    unitbalance = 0,
                    LastReceiptAmt =0,
                    PaymentType = "0",

                   // PayMode =_PaymentMode,
                    
                   // ConfirmdSales = (_PaymentMode=="PA") ? 0 : 1
                    
                };



               
                //insert into sales, sales-items, stock
                int salesID = service.InsertSales(_sales);
                service.InsertSalesItem(salesID, _stockID, _qty, _rate, _unitDisAmt, _amt, _packSize, _discount_type);

                service.UpdateStock(_stockID,  _qty);

                return Json(salesID);
              
                                   
            }
            return Json("null");            
        }

        public ActionResult SalesInfo()
        {
            return View(service.GetAllSalesInfo());
        }

        //Generates Sales Invoice
        public ActionResult SalesInvoice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _sales = service.GetSales(id);
            //check if id supplied is present or not.
            if (_sales == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                return View(_sales);  
            }                   
            
        }
        public ActionResult SalesInvoice1(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _sales = service.GetSales(id);
            //check if id supplied is present or not.
            if (_sales == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                return View(_sales);
            }

        }
        /// <summary>
        /// Populate Customer DropDown List
        /// </summary>
        /// <returns>Json data of Customer's list</returns>
        /// <remarks>The value is cached for 3 minutes so that it doesn't have to query to database again.</remarks>
        /// 


        public JsonResult PopulateCustomer()
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
        /// <summary>
        /// Populate Invoice Type DropDown List
        /// </summary>
        /// <returns>Json data of Invoice Type list</returns>
        /// <remarks>The value is cached for 3 minutes so that it doesn't have to query to database again.</remarks>
        /// 

        public JsonResult PopulateInvoiceType()
        {
            //holds list of suppliers
            List<InvoiceOptionEntry> _InvoiceOptionEntry = new List<InvoiceOptionEntry>();
            //queries all the suppliers for its ID and Name property.
            var invoiceOptionEntry = (from s in db.InvoiceOptionEntries
                                       select new { ID = s.InvOptID, s.InvoiceName }).ToList();

            //save list of suppliers to the _supplierList
            foreach (var item in invoiceOptionEntry)
            {
                _InvoiceOptionEntry.Add(new InvoiceOptionEntry
                {
                    InvOptID = item.ID,
                    InvoiceName = item.InvoiceName
                });
            }
            //returns the Json result of _settingInvoiceEntry
            return Json(_InvoiceOptionEntry, JsonRequestBehavior.AllowGet);
        }
    }
}