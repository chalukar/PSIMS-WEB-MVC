using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using IdentitySample.Models;
using PSIMS.Service;
using PSIMS.ViewModel;
using PSIMS.Models.SalesModel;
using PSIMS.Models.QuotationModel;
using PSIMS.Models.InventoryModel;
using System.Net;

namespace PSIMS.Controllers.Quotation
{
    [Authorize]
    public class QuotationEntryController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        QuotationEntryService service = new QuotationEntryService();
        SalesEntryService sales_service = new SalesEntryService();
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
        // GET: QuotationEntry
        public ActionResult Index()
        {
            //var quotevm = new QuotationEntryVM();
            //return View(quotevm);
            int UserLocationID = Convert.ToInt32(Session["LocationID"]);
            var locStickID = db.LocationStocks
                            .Where(x => x.Stock.ExpiryDate > DateTime.Now ||x.Stock.ExpiryDate==null)
                            .Where(x =>x.FinalQty > 0 && x.LocationID == UserLocationID);

            return View(locStickID.ToList());
        }


        //Populate Customer
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

        //Populate Qutation Category

        public JsonResult PopulateQuotCat()
        {
            //holds list of suppliers
            List<QuotationCategory> _quotCatList = new List<QuotationCategory>();


            //queries all the suppliers for its ID and Name property.
            var quotcatList = (from q in db.QuotationCategories
                               select new { q.ID, q.CategoryName }).ToList();



            //save list of suppliers to the _supplierList
            foreach (var item in quotcatList)
            {
                _quotCatList.Add(new QuotationCategory
                {
                    ID = item.ID,
                    CategoryName = item.CategoryName
                });
            }
            //returns the Json result of _supplierList
            return Json(_quotCatList, JsonRequestBehavior.AllowGet);
        }

        //Populate Item 
        public JsonResult PopulateItem()
        {
            //holds the list of item
            List<Item> _item = new List<Item>();

            //queries all the items in the database
            var itemList = (from i in db.Items.OrderByDescending(m => m.Name)
                            select new { i.ID, i.Name }).ToList();

            //save the list of items to the _item.
            foreach (var item in itemList)
            {
                _item.Add(new Item
                {
                    ID = item.ID,
                    Name = item.Name
                });
            }
            //returns the list of item in Json form 
            return Json(_item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveQuotation(QuotationEntryVM qout)
        {
            bool status = false;
            string _datetime = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            //int data = 0;
            //var data;

           

            if (qout != null)
            {
                //new purchase object using the data from the viewmodel : PurchaseEntryVM
                PSIMS.Models.QuotationModel.Quotation quotation = new PSIMS.Models.QuotationModel.Quotation
                {

                    QuotationCode = qout.QuotationCode,
                    //OurRef = null,
                    YourRef = qout.YourRef,
                    QuotationCategoryID = qout.QuotationCategoryID,
                    Amount = qout.Amount,
                    Discount = qout.Discount,
                    Vat = qout.Vat,
                    GrandTotal = qout.GrandTotal,
                    QuoteDate = qout.QuoteDate,
                    ExpiryQuote = qout.ExpiryQuote,
                    CustomerID = qout.CustomerID,
                    Remarks = qout.Remarks,
                    bActive = true,
                    CreatedOn = Convert.ToDateTime(_datetime),
                    Header_Note_Line01 = qout.Header_Note_Line01,
                    Header_Note_Line02 = qout.Header_Note_Line02,
                    Speical_Offers = qout.Speical_Offers,
                    Terms_Conditions = qout.Terms_Conditions,
                    
                    UserID = User.Identity.GetUserId(),
                    LocationID = Convert.ToInt32(Session["LocationID"]),
                };

                quotation.QuotationItems = new List<QuotationItem>();
                //populating the PurchaseItems from the PurchaseItems within ViewModel : PurchaseEntryVM
                foreach (var i in qout.QuotationItems)
                {
                    quotation.QuotationItems.Add(i);
                }

                service.AddQuotationAndQuotationItems(quotation);
                //int quotation_quote = service.AddQuotationAndQuotationItems(quotation);
                //data = quotation_quote;
                status = true;
            }
            // return the status in form of Json
            return new JsonResult { Data = new { status = status } };
            //return new JsonResult { Data = new { data = data } };
        }


        //Generates quotation - Analizer
        public ActionResult quotationForm_Analizer(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _quotationFrom = service.GetquotationForm(id);
            //check if id supplied is present or not.
            if (_quotationFrom == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                return View(_quotationFrom);
            }

        }
        //Generates quotation - Re-Agent
        public ActionResult quotationForm_ReAgent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var _quotationFrom = sales_service.GetquotationForm(id);
            //check if id supplied is present or not.
            if (_quotationFrom == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                return View(_quotationFrom);
            }

        }

        public JsonResult PopulateQuotationCode()
        {
            //holds list of suppliers
            List<PSIMS.Models.QuotationModel.Quotation> _QuoteCode = new List<PSIMS.Models.QuotationModel.Quotation>();

            //queries all the suppliers for its ID and Name property.

            _QuoteCode = db.Quotations.OrderByDescending(x => x.ID).Take(1).ToList();

            if (_QuoteCode.Count == 0)
            {

                return Json(new
                {
                    ID = 0,
                    //_listStockID[0].ID,

                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new
                {
                    ID = _QuoteCode[0].ID,

                }, JsonRequestBehavior.AllowGet);
            }


        }
    }

}