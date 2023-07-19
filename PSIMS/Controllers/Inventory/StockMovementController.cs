using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using PSIMS.Models.Locations;
using PSIMS.Service;
using Microsoft.AspNet.Identity;
using PSIMS.Repository;
using System.Security.Principal;
using PSIMS.Models.Account;
using Microsoft.AspNet.Identity.EntityFramework;
using PSIMS.ViewModel;
using System.Web.Script.Serialization;
using PSIMS.ViewModel.ForReports;

namespace PSIMS.Controllers.Inventory
{
    [Authorize]
    public class StockMovementController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private StockMovementRepository repo = new StockMovementRepository();
        private StockMovementService sms = new StockMovementService();
        private UserManager<ApplicationUser> manager = null;
        private RoleManager<IdentityRole> roleManager = null;

        public StockMovementController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        }

        // GET: StockDistributions
        public ActionResult TransferIndex()
        {

            //var today = DateTime.Today;
            //int year = DateTime.Now.Year;
            //int month = DateTime.Now.Month;

            var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();
            var stockDistributions = db.StockDistributions
                                       .Include(s => s.Stock)
                                       .Include(s => s.FromLocation)
                                       .Include(s => s.ToLocation);

            RoleAccessType accType = db.RoleAccessTypes
                                       .FirstOrDefault(rat => rat.RoleID == role.RoleId);

            if (accType.AccessTypeCode.Equals("ALLW"))
            {
                return View(stockDistributions.Take(100).ToList());
            }
            else
            {
                int UserLocationID = Convert.ToInt32(Session["LocationID"]);
                stockDistributions = stockDistributions
                                     .Where(x => x.LocationID == UserLocationID);
                return View(stockDistributions.Take(50).ToList());
            }

        }

        [HttpPost]
        public ActionResult TransferIndex(StockTransferNoteVM St)
        {
            var filter = new StocksFilterRepository();
            var model = filter.FilterTransferIndex(St);

            return View(model);
        }


        public ActionResult TransferStock()
        {

            return View();
        }

        public ActionResult ReceivedIndex()
        {
            var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();
            var ReceivedStock = db.StockDistributions
                                  .Include(s => s.Stock)
                                  .Include(s => s.FromLocation)
                                  .Include(s => s.ToLocation);

            RoleAccessType accType = db.RoleAccessTypes
                                       .FirstOrDefault(rat => rat.RoleID == role.RoleId);

            if (accType.AccessTypeCode.Equals("ALLW"))
            {
                return View(ReceivedStock.OrderByDescending(r => r.ID).Take(100).ToList());
            }
            else
            {
                int UserLocationID = Convert.ToInt32(Session["LocationID"]);
                ReceivedStock = ReceivedStock
                                .Where(s => s.ToLocationID == UserLocationID);
                return View(ReceivedStock.OrderByDescending(r => r.ID).Take(100).ToList());
            }


        }

        [HttpPost]
        public ActionResult ReceivedIndex(StockTransferNoteVM St)
        {
            var filter = new StocksFilterRepository();
            var model = filter.FilterReceivedIndex(St);

            return View(model);
        }


        public ActionResult ReceiveStock()
        {
            var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();
            var ReceivedStock = db.StockDistributions
                                  .Include(s => s.Stock)
                                  .Include(s => s.FromLocation)
                                  .Include(s => s.ToLocation)
                                  .Where(s => s.ReceivedQty == null);

            RoleAccessType accType = db.RoleAccessTypes
                                       .FirstOrDefault(rat => rat.RoleID == role.RoleId);
            if (accType.AccessTypeCode.Equals("ALLW"))
            {
                return View(ReceivedStock.OrderByDescending(r => r.ID).Take(100).ToList());
            }
            else
            {
                int UserLocationID = Convert.ToInt32(Session["LocationID"]);
                ReceivedStock = ReceivedStock.Where(s => s.ToLocationID == UserLocationID)
                                             .Where(s => s.ReceivedQty == null)
                                             .OrderByDescending(x => x.ID);
                return View(ReceivedStock.OrderByDescending(r => r.ID).Take(100).ToList());

            }


        }

        [HttpPost]
        public ActionResult ReceiveStock(StockTransferNoteVM St)
        {
            var filter = new StocksFilterRepository();
            var model = filter.FilterReceiveStock(St);

            return View(model);
        }

        //Daily Stock Index
        public ActionResult DaliyStockIndex()
        {
            var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();
            var DaliyStock = db.DaliyStocks;
            RoleAccessType accType = db.RoleAccessTypes.FirstOrDefault(rat => rat.RoleID == role.RoleId);
            if (accType.AccessTypeCode.Equals("ALLW"))
            {
                return View(DaliyStock.ToList());
            }
            else
            {
                int UserLocationID = Convert.ToInt32(Session["LocationID"]);
                return View(db.DaliyStocks.Where(p => p.LocationID == UserLocationID).ToList());
            }

        }

        //Location Stock
        public ActionResult LocationStockIndex()
        {
            var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();

            var _locationStock = db.LocationStocks.Include(s => s.Stock).Include(s => s.Location);
            RoleAccessType accType = db.RoleAccessTypes.FirstOrDefault(rat => rat.RoleID == role.RoleId);
            if (accType.AccessTypeCode.Equals("ALLW"))
            {
                return View(_locationStock.ToList());
            }
            else
            {
                int UserLocationID = Convert.ToInt32(Session["LocationID"]);
                _locationStock = _locationStock.Where(s => s.LocationID == UserLocationID);
                return View(_locationStock.ToList());
            }

        }
        // GET: StockDistributions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMovement stockDistribution = db.StockDistributions.Find(id);
            if (stockDistribution == null)
            {
                return HttpNotFound();
            }
            return View(stockDistribution);
        }


        [HttpPost]
        public ActionResult UpdateReceiveStock(StockMovement _stockMovement)
        {
            int UserLocationID = Convert.ToInt32(Session["LocationID"]);


            StockMovement updatedtransferStock = (from c in db.StockDistributions
                                                  where c.ID == _stockMovement.ID
                                                  select c).FirstOrDefault();


            updatedtransferStock.ReceivedQty = _stockMovement.ReceivedQty;
            updatedtransferStock.Status = _stockMovement.Status;
            updatedtransferStock.Remarks = _stockMovement.Remarks;
            updatedtransferStock.ReceivedBy = User.Identity.GetUserId();
            updatedtransferStock.ReceivedOn = DateTime.Now;


            StockMovement InsertToDaliyStock = (from c in db.StockDistributions
                                                where c.ID == _stockMovement.ID
                                                select c).FirstOrDefault();


            DaliyStock _daliyStock = new DaliyStock();

            //_daliyStock.TrxID = InsertToDaliyStock.ID;
            _daliyStock.StockID = InsertToDaliyStock.StockID;
            _daliyStock.TrxDate = DateTime.Now;
            _daliyStock.LocationID = InsertToDaliyStock.ToLocationID;
            _daliyStock.InQty = (decimal)InsertToDaliyStock.ReceivedQty;
            _daliyStock.OutQty = 0;


            StockMovement InsertToLocationStock = (from c in db.StockDistributions
                                                   where c.ID == _stockMovement.ID
                                                   select c).FirstOrDefault();

            //Get list of location stock
            LocationStock _locationStock = new LocationStock();

            LocationStock _setlocationstock = new LocationStock();

            _locationStock = (from c in db.LocationStocks
                              where c.Loc_BatchNo == InsertToLocationStock.Stock.BatchNo &&
                              c.LocationID == InsertToLocationStock.ToLocationID &&
                              c.Status ==1
                              orderby c.TrxDate descending
                              select c).Take(1).ToList().FirstOrDefault();

            if (_locationStock != null)
            {
                if (InsertToLocationStock.Stock.ExpiryDate == null && _locationStock.Loc_ExpiryDate == null)
                {
                    if (_locationStock.StockID == InsertToLocationStock.StockID &&
                       _locationStock.ItemID == InsertToLocationStock.Stock.ItemID &&
                       _locationStock.Loc_BatchNo == InsertToLocationStock.Stock.BatchNo &&
                       _locationStock.Loc_PackSize == InsertToLocationStock.Stock.PackSize)
                    {
                        _locationStock.StockID = InsertToLocationStock.StockID;
                        _locationStock.TrxDate = DateTime.Now;
                        _locationStock.FinalQty += (decimal)InsertToLocationStock.ReceivedQty;
                        _locationStock.InQty = _locationStock.FinalQty;
                        _locationStock.OutQty = 0;
                        _locationStock.Loc_PackSize = InsertToLocationStock.Stock.PackSize;


                        //db.LocationStocks.Add(_locationStock);
                        db.SaveChanges();
                    }
                    else if (_locationStock.StockID != InsertToLocationStock.StockID &&
                      _locationStock.ItemID == InsertToLocationStock.Stock.ItemID &&
                      _locationStock.Loc_BatchNo == InsertToLocationStock.Stock.BatchNo &&
                      _locationStock.Loc_PackSize == InsertToLocationStock.Stock.PackSize)
                    {
                        _locationStock.StockID = InsertToLocationStock.StockID;
                        _locationStock.TrxDate = DateTime.Now;
                        _locationStock.FinalQty += (decimal)InsertToLocationStock.ReceivedQty;
                        _locationStock.InQty = _locationStock.FinalQty;
                        _locationStock.OutQty = 0;
                        _locationStock.Loc_PackSize = InsertToLocationStock.Stock.PackSize;


                        db.LocationStocks.Add(_locationStock);
                        db.SaveChanges();
                    }
                }
                else
                {
                    if (_locationStock.StockID == InsertToLocationStock.StockID &&
                        _locationStock.ItemID == InsertToLocationStock.Stock.ItemID &&
                        _locationStock.Loc_BatchNo == InsertToLocationStock.Stock.BatchNo &&
                        _locationStock.Loc_PackSize == InsertToLocationStock.Stock.PackSize &&
                        _locationStock.Loc_ExpiryDate == InsertToLocationStock.Stock.ExpiryDate)
                    {
                        _locationStock.StockID = InsertToLocationStock.StockID;
                        _locationStock.TrxDate = DateTime.Now;
                        _locationStock.FinalQty += (decimal)InsertToLocationStock.ReceivedQty;
                        _locationStock.InQty = _locationStock.FinalQty;
                        _locationStock.OutQty = 0;
                        _locationStock.Loc_PackSize = InsertToLocationStock.Stock.PackSize;
                        

                        // db.LocationStocks.Add(_locationStock);
                        db.SaveChanges();

                    }
                    else if (_locationStock.StockID != InsertToLocationStock.StockID &&
                        _locationStock.ItemID == InsertToLocationStock.Stock.ItemID &&
                        _locationStock.Loc_BatchNo == InsertToLocationStock.Stock.BatchNo &&
                        _locationStock.Loc_PackSize == InsertToLocationStock.Stock.PackSize &&
                        _locationStock.Loc_ExpiryDate == InsertToLocationStock.Stock.ExpiryDate)
                    {
                        _locationStock.StockID = InsertToLocationStock.StockID;
                        _locationStock.TrxDate = DateTime.Now;
                        _locationStock.FinalQty += (decimal)InsertToLocationStock.ReceivedQty;
                        _locationStock.InQty = _locationStock.FinalQty;
                        _locationStock.OutQty = 0;
                        _locationStock.Loc_PackSize = InsertToLocationStock.Stock.PackSize;


                         db.LocationStocks.Add(_locationStock);
                        db.SaveChanges();

                    }
                }
                
            }
            else
            {
                if (InsertToLocationStock.Stock.ExpiryDate == null)
                {
                    _setlocationstock.StockID = InsertToLocationStock.StockID;
                    _setlocationstock.TrxID = InsertToLocationStock.transferID;
                    _setlocationstock.TrxDate = DateTime.Now;
                    _setlocationstock.ItemID = InsertToLocationStock.ItemID;
                    _setlocationstock.Loc_BatchNo = InsertToLocationStock.Stock.BatchNo;
                    _setlocationstock.Loc_PackSize = InsertToLocationStock.Stock.PackSize;
                    _setlocationstock.Loc_ExpiryDate = null;
                    _setlocationstock.UserID = User.Identity.GetUserId();
                    _setlocationstock.LocationID = UserLocationID;
                    _setlocationstock.InQty = (decimal)InsertToLocationStock.ReceivedQty;
                    _setlocationstock.OutQty = 0;
                    _setlocationstock.FinalQty = (decimal)InsertToLocationStock.ReceivedQty;
                    _setlocationstock.Status = 1;

                    db.LocationStocks.Add(_setlocationstock);
                    db.SaveChanges();
                }
                else
                {
                    _setlocationstock.StockID = InsertToLocationStock.StockID;
                    _setlocationstock.TrxID = InsertToLocationStock.transferID;
                    _setlocationstock.TrxDate = DateTime.Now;
                    _setlocationstock.ItemID = InsertToLocationStock.ItemID;
                    _setlocationstock.Loc_BatchNo = InsertToLocationStock.Stock.BatchNo;
                    _setlocationstock.Loc_PackSize = InsertToLocationStock.Stock.PackSize;
                    _setlocationstock.Loc_ExpiryDate = InsertToLocationStock.Stock.ExpiryDate;
                    _setlocationstock.UserID = User.Identity.GetUserId();
                    _setlocationstock.LocationID = UserLocationID;
                    _setlocationstock.InQty = (decimal)InsertToLocationStock.ReceivedQty;
                    _setlocationstock.OutQty = 0;
                    _setlocationstock.FinalQty = (decimal)InsertToLocationStock.ReceivedQty;
                    _setlocationstock.Status = 1;

                    db.LocationStocks.Add(_setlocationstock);
                    db.SaveChanges();
                }
            }



            db.DaliyStocks.Add(_daliyStock);
            db.SaveChanges();

           
            return new EmptyResult();
        }

        // GET: StockDistributions/Edit/5
        public ActionResult Edit(int? id)
        {
            var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMovement stockDistribution = db.StockDistributions.Find(id);
            if (stockDistribution == null)
            {
                return HttpNotFound();
            }

            var ReceivedStock = db.StockDistributions
                                  .Include(s => s.Stock)
                                  .Include(s => s.FromLocation)
                                  .Include(s => s.ToLocation);


            RoleAccessType accType = db.RoleAccessTypes
                                       .FirstOrDefault(rat => rat.RoleID == role.RoleId);



            if (accType.AccessTypeCode.Equals("ALLW"))
            {
                return View(ReceivedStock.ToList());
            }
            else
            {
                int UserLocationID = Convert.ToInt32(Session["LocationID"]);

                ReceivedStock = db.StockDistributions
                                .Where(s => s.ToLocationID == UserLocationID)
                                .OrderByDescending(s => s.ID);



                return View(ReceivedStock.ToList());

            }

        }

        // POST: StockDistributions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ItemID,StockID,FromLocationID,ToLocationID,DistributedQty,Remarks,CreateBy,CreatedOn")] StockMovement stockDistribution)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stockDistribution).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StockID = new SelectList(db.Stocks, "ID", "BatchNo", stockDistribution.StockID);
            return View(stockDistribution);
        }

        // GET: StockDistributions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StockMovement stockDistribution = db.StockDistributions.Find(id);




            if (stockDistribution == null)
            {
                return HttpNotFound();
            }
            return View(stockDistribution);
        }


        // POST: StockDistributions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StockMovement stockDistribution = db.StockDistributions.Find(id);
            db.StockDistributions.Remove(stockDistribution);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //Transder Stock Delete
        public ActionResult TransferStockDelete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  StockMovement transferStockDel = db.StockDistributions.Find(id);
            StockMovement transferStockDel = db.StockDistributions.Find(id);

            if (transferStockDel == null)
            {
                return HttpNotFound();
            }
            return View(transferStockDel);
        }
        [HttpPost, ActionName("TransferStockDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult TransferStockDeleteConfirmed(int id)
        {
            decimal _qty;
            int rtnID;

            StockMovement stockDistribution = db.StockDistributions.Find(id);
            //Stock stock = db.Stocks.Find(batchNo);
            if (stockDistribution.Status == 0)
            {
                rtnID = stockDistribution.StockID;
                _qty = stockDistribution.DistributedQty;
                //stock.Qty = stockDistribution.InitQty ;
                db.StockDistributions.Remove(stockDistribution);
                // db.Entry(stock).State = EntityState.Modified;
                sms.returnStock(rtnID, _qty);
                db.SaveChanges();

                return RedirectToAction("TransferIndex");
            }
            else
            {
                return RedirectToAction("TransferIndex");

            }

        }

        public JsonResult PopulateItemId()
        {
            List<TransferStovkVM> _itemList = new List<TransferStovkVM>();

            //queries all the Item for its ID and Name property.
            var itemList = (from i in db.Stocks.Include(i => i.Item)
                            select new { i.Item.ID, i.Item.Name })
                            .OrderByDescending(i => i.Name)
                            .Distinct().ToList();

            foreach (var i in itemList)
            {
                _itemList.Add(new TransferStovkVM
                {
                    ID = i.ID,
                    ItemCode = i.Name
                });
            }
            return Json(_itemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PopulateLocationId()
        {
            //holds list of Location
            List<Location> _locationList = new List<Location>();
            //queries all the Location for its ID and Name property.
            var locationList = (from l in db.Locations
                                where l.IsDefault == true
                                select new { l.ID, l.LocationName }).ToList();

            //save list of Location to the _locationList
            foreach (var l in locationList)
            {
                _locationList.Add(new Location
                {
                    ID = l.ID,
                    LocationName = l.LocationName
                });
            }
            //returns the Json result of _locationList
            return Json(_locationList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PopulateDestLocationId()
        {
            //holds list of Location
            List<Location> _locationList = new List<Location>();
            //queries all the Location for its ID and Name property.
            var locationList = (from l in db.Locations
                                where l.IsDefault == false
                                select new { l.ID, l.LocationName }).ToList();

            //save list of Location to the _locationList
            foreach (var l in locationList)
            {
                _locationList.Add(new Location
                {
                    ID = l.ID,
                    LocationName = l.LocationName
                });
            }
            //returns the Json result of _locationList
            return Json(_locationList, JsonRequestBehavior.AllowGet);
        }


        public JsonResult PopulateBatchNo(int search, int ItemId)
        {
            //holds list of stock
            List<Stock> _stockList = new List<Stock>();
            //queries all the BatchNo for its BatchNo and Qty property.
            var stockList = (from s in db.Stocks
                             where (s.ItemID == ItemId)
                             select new { s.ID, s.BatchNo, s.Qty }).ToList();

            //save list of stock to the _stockList
            foreach (var stock in stockList)
            {
                _stockList.Add(new Stock
                {
                    BatchNo = stock.BatchNo,
                    Qty = stock.Qty
                });
            }
            //returns the Json result of _stockList
            return Json(_stockList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvailQty(int _itemId, int _locationId, String _batchNo)
        {
            //holds list of stock
            List<Stock> _stockList = new List<Stock>();
            //queries all the BatchNo for its BatchNo and Qty property.
            var stockList = (from s in db.Stocks
                             where (s.ItemID == _itemId) && (s.BatchNo == _batchNo)
                             select new { s.ID, s.MovingQty }).ToList();

            //save list of stock to the _stockList
            foreach (var stock in stockList)
            {
                _stockList.Add(new Stock
                {
                    ID = stock.ID,
                    MovingQty = stock.MovingQty
                });
            }
            //returns the Json result of _stockList
            return Json(_stockList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveStockMovement(PSIMS.ViewModel.StockDistributionVM vm)
        {
            string[] _stockID, _qty;
            bool status = false;


            _stockID = vm.StockID.ToString().Split(',');
            _qty = vm.DistQty.ToString().Split(',');

            //new StockDistribution object using the data from the viewmodel : StockDistributionVM
            PSIMS.Models.InventoryModel.StockMovement stockdist = new PSIMS.Models.InventoryModel.StockMovement
            {
                transferID = vm.transferID,
                StockID = vm.StockID,
                ItemID = vm.ItemID,
                FromLocationID = vm.FromLocationID,
                ToLocationID = vm.ToLocationID,
                InitQty = vm.InitQty,
                DistributedQty = vm.DistQty,
                Remarks = vm.Remarks,
                TransferdBy = User.Identity.GetUserName(),
                TransferdOn = DateTime.Now,
                Status = 0



            };



            sms.UpdateStockMovement(stockdist);
            sms.UpdateStock(_stockID, _qty);
            sms.Updatepurchase(_stockID);

            // return the status in form of Json
            return new JsonResult { Data = new { status = status } };
        }


        //--------------------New Controll for Stock Moments------------------------------

        // Open Stock Transfer Entry
        public ActionResult StockTransferEntry()
        {

            return View();
        }

        public JsonResult PopulateLocation()
        {
            //holds list of suppliers
            List<Location> _locationList = new List<Location>();
            //queries all the suppliers for its ID and Name property.
            var locationList = (from s in db.Locations
                                select new { s.ID, s.LocationName, s.LocationCode }).ToList();

            //save list of suppliers to the _supplierList
            foreach (var item in locationList)
            {
                _locationList.Add(new Location
                {
                    ID = item.ID,
                    LocationCode = item.LocationCode,
                    LocationName = item.LocationName
                });
            }
            //returns the Json result of _supplierList
            return Json(_locationList, JsonRequestBehavior.AllowGet);
        }

        //GetTransfer ID
        [HttpGet]
        public JsonResult PopulateTransferID()
        {
            List<StockMovement> _listStockID = new List<StockMovement>();


            //int id = Convert.ToInt32(SearchVal);


            _listStockID = db.StockDistributions.OrderByDescending(x => x.ID).Take(1).ToList();

            if (_listStockID.Count == 0)
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
                    ID = _listStockID[0].ID,

                }, JsonRequestBehavior.AllowGet);
            }




        }

        //Source
        public JsonResult PopulateSource()
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

        //Source
        public JsonResult PopulateDestionation()
        {
            //holds list of suppliers
            List<Location> _locationList = new List<Location>();
            //queries all the suppliers for its ID and Name property.
            var locationList = (from s in db.Locations
                                where s.IsDefault == false
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

        //Item

        public JsonResult PopulateItem()
        {

            List<TransferStovkVM> _itemList = new List<TransferStovkVM>();

            //queries all the Item for its ID and Name property.
            var itemList = (from i in db.Stocks.Include(i => i.Item)
                            select new { i.Item.ID, i.Item.Name }).Distinct().ToList();

            foreach (var i in itemList)
            {
                _itemList.Add(new TransferStovkVM
                {
                    ID = i.ID,
                    ItemCode = i.Name
                });
            }
            return Json(_itemList, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult PopulateBatchNo(string stateID)
        {

            List<Stock> _listBatchNo = new List<Stock>();
            int stateiD = Convert.ToInt32(stateID);

            //queries all the BatchNo for its BatchNo and Qty property.
            var listBatchNo = (from s in db.Stocks
                               where (s.ItemID == stateiD)
                               select new { s.ID, s.BatchNo }).ToList();

            //save list of stock to the _stockList
            foreach (var i in listBatchNo)
            {
                _listBatchNo.Add(new Stock
                {
                    ID = i.ID,
                    BatchNo = i.BatchNo

                });
            }
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(_listBatchNo);
            return Json(result, JsonRequestBehavior.AllowGet);

        }


        //Availbility Qty
        [HttpGet]
        public JsonResult PopulateAvilQty(string SearchVal)
        {
            List<Stock> _listBatchNo = new List<Stock>();


            int id = Convert.ToInt32(SearchVal);

            _listBatchNo = db.Stocks.Where(x => x.ID == id).ToList();

            //_listBatchNo = (from s in db.Stocks
            //                where (s.ID == id)
            //                   select new { s.ID, s.MovingQty }).List();
            return Json(new
            {
                ID = _listBatchNo[0].ID,
                MovingQty = _listBatchNo[0].MovingQty,




            }, JsonRequestBehavior.AllowGet);


        }

        [HttpPost]
        public JsonResult SaveTransferStock(PSIMS.ViewModel.StockMomentEntryVM vm)
        {
            string[] _stockID, _qty;
            bool status = false;

            if (vm != null)
            {
                StockMovementMaster stocktransferMaster = new StockMovementMaster
                {
                    ID = vm.ID,
                    locationID = vm.locationID,
                    Date = vm.Date,
                    UserID = User.Identity.GetUserId()
                };

                stocktransferMaster.StockMomentDetals = new List<StockMovementDetals>();

                foreach (var i in vm.StockMomentDetals)
                {
                    stocktransferMaster.StockMomentDetals.Add(i);
                }

                sms.AddStockTtxMaster(stocktransferMaster);
                sms.AddStockTrxDetails(vm.StockMomentDetals);
                //sms.UpdateStock(_stockID)

                //sms.UpdateStockMovement(stockdist);
                //sms.UpdateStock(_stockID, _qty);
                //sms.Updatepurchase(_stockID);
            }



            //_stockID = vm.StockID.ToString().Split(',');
            //_qty = vm.DistQty.ToString().Split(',');

            //if (vm != null)
            //{
            //    //new StockDistribution object using the data from the viewmodel : StockDistributionVM
            //    PSIMS.Models.InventoryModel.StockMovement stockdist = new PSIMS.Models.InventoryModel.StockMovement
            //    {

            //        StockID = vm.StockID,
            //        ItemID = vm.ItemID,
            //        FromLocationID = vm.FromLocationID,
            //        ToLocationID = vm.ToLocationID,
            //        InitQty = vm.InitQty,
            //        DistributedQty = vm.DistQty,
            //        Remarks = vm.Remarks,
            //        TransferdBy = User.Identity.GetUserName(),
            //        TransferdOn = DateTime.Now,
            //        Status = 0
            //    };



            //    sms.UpdateStockMovement(stockdist);
            //    sms.UpdateStock(_stockID, _qty);
            //    sms.Updatepurchase(_stockID);
            //}
            // return the status in form of Json
            return new JsonResult { Data = new { status = status } };
        }

        public JsonResult PopulateReportTransferID()
        {
            //holds list of suppliers
            List<StockMovement> _TransferID = new List<StockMovement>();
            //queries all the suppliers for its ID and Name property.
            var TransferIDList = (from s in db.StockDistributions
                                  select new { s.ID, s.transferID }).Distinct().ToList();

            //save list of suppliers to the _supplierList
            foreach (var item in TransferIDList)
            {
                _TransferID.Add(new StockMovement
                {
                    ID = item.ID,
                    transferID = item.transferID
                });
            }
            //returns the Json result of _supplierList
            return Json(_TransferID, JsonRequestBehavior.AllowGet);
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
