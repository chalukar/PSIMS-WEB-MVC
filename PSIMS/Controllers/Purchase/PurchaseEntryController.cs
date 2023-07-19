using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using PSIMS.Models.Locations;
using PSIMS.Models.PurchaseModel;
using PSIMS.Service;
using PSIMS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using PSIMS.Repository;

namespace PSIMS.Controllers
{
     [Authorize]
    public class PurchaseEntryController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        private PurchaseEntryService service = new PurchaseEntryService();
        
        // GET: PurchaseEntry
        /// <summary>
        /// Main page for entering new purchase records.
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public ActionResult Index()
        {
            var vm = new PurchaseEntryVM();
            return View(vm);

        }

         //Testing For Data Entry
        public ActionResult IndexForFirst()
        {
            var vm = new PurchaseEntryVM();
            return View(vm);
        }

        public JsonResult Populatepacksizecode()
        {
            //holds list of suppliers
            List<PurchasePackSizeCode> _packsizecode = new List<PurchasePackSizeCode>();
            //queries all the suppliers for its ID and Name property.
            var packsizecodeList = (from s in db.PurchasePackSizeCodes
                                select new { s.ID, s.PackSize_Code }).ToList();

            //save list of suppliers to the _supplierList
            foreach (var item in packsizecodeList)
            {
                _packsizecode.Add(new PurchasePackSizeCode
                {
                    ID = item.ID,
                    PackSize_Code = item.PackSize_Code
                });
            }
            //returns the Json result of _supplierList
            return Json(_packsizecode, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Populate Supplier DropDown List
        /// </summary>
        /// <returns>Json data of supplier's list</returns>
        /// <remarks>The value is cached for 3 minutes so that it doesn't have to query to database again.</remarks>

        public JsonResult PopulateSupplier()
        {
            //holds list of suppliers
            List<Supplier> _supplierList = new List<Supplier>();
            //queries all the suppliers for its ID and Name property.
            var supplierList = (from s in db.Suppliers
                               select new { s.ID, s.SupplierName }).ToList();
            
                //save list of suppliers to the _supplierList
                foreach (var item in supplierList)
                {                   
                    _supplierList.Add(new Supplier { 
                        ID = item.ID ,
                        SupplierName = item.SupplierName
                    });          
                }
            //returns the Json result of _supplierList
            return Json(_supplierList, JsonRequestBehavior.AllowGet); 
        }


        /// <summary>
        /// Populate Items DropDownList
        /// </summary>
        /// <returns>Json data of Item's list</returns>
        /// <remarks>The value is cached for 3 minutes so that it doesn't have to query to database again.</remarks>
       
        public JsonResult PopulateItem()
        {
            //queries all the items in the database
            var itemList = (from i in db.Items.OrderBy(m =>m.Name)
                            select new { i.ID, i.Name }).ToList();
            //holds the list of item
            List<Item> _item = new List<Item>();

            //save the list of items to the _item.
            foreach (var item in itemList)
            {
                _item.Add(new Item { 
                    ID=item.ID,
                    Name =item.Name
                });
            }
            //returns the list of item in Json form 
            return Json(_item, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Populate Supplier Form-Country List_Phurchase Entry
        /// </summary>
        /// <returns>Json data of supplier's list</returns>
        /// <remarks>The value is cached for 3 minutes so that it doesn't have to query to database again.</remarks>

        public JsonResult PopulateCountry()
        {
            //holds list of suppliers
            List<Country> _countryList = new List<Country>();
            //queries all the suppliers for its ID and Name property.
            var countryList = (from s in db.Countries
                                select new { s.CountryID, s.CountryName }).ToList();

            //save list of suppliers to the _supplierList
            foreach (var item in countryList)
            {
                _countryList.Add(new Country
                {
                    CountryID = item.CountryID,
                    CountryName = item.CountryName
                });
            }
            //returns the Json result of _supplierList
            return Json(_countryList, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Populate Supplier Form-Country List_Phurchase Entry
        /// </summary>
        /// <returns>Json data of supplier's list</returns>
        /// <remarks>The value is cached for 3 minutes so that it doesn't have to query to database again.</remarks>

        public JsonResult PopulateLocation()
        {
            //holds list of suppliers
            List<Location> _locationList = new List<Location>();
            //queries all the suppliers for its ID and Name property.
            var locationList = (from s in db.Locations
                                where s.IsDefault == true
                               select new { s.ID, s.LocationName }).ToList();

            //save list of suppliers to the _supplierList
            foreach (var item in locationList)
            {
                _locationList.Add(new Location
                {
                    ID = item.ID,
                    LocationName = item.LocationName
                });
            }
            //returns the Json result of _supplierList
            return Json(_locationList, JsonRequestBehavior.AllowGet);
        }
        


        /// <summary>
        /// Post action for Saving data to database
        /// </summary>
        /// <param name="p">View model holding the entered data.</param>
        /// <returns>Returns value indicating if the data has been saved or failed to save.</returns>
        /// <remarks> Gets data as ViewModel rather than FormCollection.</remarks>
        [HttpPost]
        public JsonResult SavePurchase(PurchaseEntryVM p)
        {
            PurchaseEntryRepository repo = new PurchaseEntryRepository();
            bool status = false;
            int count = 0;
            if (p != null)
            {
                List<PSIMS.Models.PurchaseModel.Purchase> purchasecount = new List<PSIMS.Models.PurchaseModel.Purchase>();

                purchasecount = (from b in db.Purchases where (b.ID == p.ID) select b).ToList();
                int countpurchaseID = purchasecount.Count;

                if (countpurchaseID > 0)
                {
                   
                    status = false;
                    count = countpurchaseID;
                }
                else
                {
                    //new purchase object using the data from the viewmodel : PurchaseEntryVM
                    PSIMS.Models.PurchaseModel.Purchase purchase = new PSIMS.Models.PurchaseModel.Purchase
                    {
                        ID = p.ID,
                        Date = p.Date,
                        SupplierID = p.SupplierID,
                        Amount = p.Amount,
                        Discount = p.Discount,
                        Tax = p.Tax,
                        GrandTotal = p.GrandTotal,
                        IsPaid = p.IsPaid,
                        Description = p.Description,
                        LastUpdated = DateTime.Now,
                        UserID = User.Identity.GetUserId(),
                        LocationID = Convert.ToInt32(Session["LocationID"]),
                    };

                    purchase.PurchaseItems = new List<PurchaseItem>();
                    //populating the PurchaseItems from the PurchaseItems within ViewModel : PurchaseEntryVM
                    foreach (var i in p.PurchaseItems)
                    {
                        purchase.PurchaseItems.Add(i);
                    }

                    //add purchase 
                    // finally save changes.
                    service.AddPurchaseAndPurchseItems(purchase);
                    service.InsertOrUpdateInventory(p.PurchaseItems);

                    //if everything is sucessful, set status to true.
                    status = true;
                    count = countpurchaseID;
                }

                 
            }     
            // return the status in form of Json
            return new JsonResult { Data = new { status = status, count = count } };
        }

        [HttpGet]
        public JsonResult GetformShippClear(string SearchVal)
        {
            // List<PSIMS.Models.Sales> Saleslist = new List<PSIMS.Models.Sales>();
            List<PSIMS.Models.PurchaseModel.Clearance> _shippcl = new List<PSIMS.Models.PurchaseModel.Clearance>();

            string _purinvoiceNo = SearchVal;

            _shippcl = db.Clearances.Where(x => x.InvoiceNo == _purinvoiceNo).ToList();
            return Json(new
            {
                _Qty = _shippcl[0].Qty,
                _ShippingCost = _shippcl[0].ShippingCost,
                _ClearanceAmt = _shippcl[0].ClearanceAmt,
                _DollerPrice = _shippcl[0].DollerPrice


            }, JsonRequestBehavior.AllowGet);

        }

        //public ActionResult AutoComplete()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult GetAutoComplete(string term)
        //{
        //    var result = db.Items.Where(x => x.Name.Contains(term))
        //        .Select(s => new ItemsVM { ItemID = s.ID, Name = s.Name }).ToList();

        //    //var itemList = (from i in db.Items.OrderBy(m => m.Name)
        //    //                select new { i.ID, i.Name }).ToList();

        //    return Json("", JsonRequestBehavior.AllowGet);
        //}
    }
}