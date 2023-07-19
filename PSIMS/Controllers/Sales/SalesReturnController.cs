using PSIMS.Models;
using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PSIMS.Repository;
using PSIMS.Service;

namespace PSIMS.Controllers.Sales
{
    [Authorize]
    public class SalesReturnController : Controller
    {
        private IdentitySample.Models.ApplicationDbContext db = new IdentitySample.Models.ApplicationDbContext();
        SalesReturnService _salesReturnService = new SalesReturnService();
      

        //GET: SalesReturn
        public ActionResult Index()
        {
            return View(db.SalesReturns.OrderByDescending(s =>s.ID).ToList());
        }

        //POST: SalesReturn/ReturnDetails/5
        public ActionResult ReturnDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model  = db.SalesReturnDetails.Where(s=>s.SalesReturnID == id).ToList();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }
        

        // POST: SalesReturn/5
        public ActionResult Returns(int id)
        {
            PSIMS.Models.Sales model = db.Sales.Find(id);
            //null check
            if (model == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }
           
            return View(model);
        }

        /// <summary>
        /// Save all the returned items 
        /// </summary>
        /// <param name="coll"></param>
        /// <returns></returns>
        //POST : 
        public ActionResult ReturnItems(FormCollection coll)
        {
            
           List<SalesReturnDetail> details = new List<SalesReturnDetail>();

            var counter = Convert.ToInt32(coll["counter"]);

            //attributes required for SalesReturn
            decimal total = Convert.ToDecimal(coll["SubTotal"]);
            decimal discount = Convert.ToDecimal(coll["Discount"]);
            decimal netTotal = Convert.ToDecimal(coll["NetTotal"]);
            int salesID = Convert.ToInt32(coll["SalesID"]);

            //populating through each of the occurance of the ReturnedItems
            for (int i = 1; i <= counter; i++)
            {                 
                var value = coll["Qty_"+i];
                if (!string.IsNullOrEmpty(value) && value != "0")
                    {
                        SalesReturnDetail srd = new SalesReturnDetail {                            
                                                            StockID = Convert.ToInt32(coll["StockID_"+i]),                            
                                                            BatchNo = coll["BatchNo_"+i],
                                                            Qty = Convert.ToDecimal(coll["Qty_" + i]),
                                                            Rate = Convert.ToDecimal(coll["Rate_"+i]),
                                                            Amount = Convert.ToDecimal(coll["Amount_"+i])       
                                                      };
                        details.Add(srd); 
                    }
            }

            //populating Sales Return
            SalesReturn _SalesReturn = new SalesReturn
            {
                SalesID = salesID,
                Subtotal = total,
                Discount =discount,
                NetTotal = netTotal,
                SalesReturnDetails = details,
                Description = "n/a",
                ReturnedDate = DateTime.Today
            };

           int _salesID = _salesReturnService.insertsalesReturn(_SalesReturn);
            _salesReturnService.updatesales(_salesID);
            return RedirectToAction("Index");
        }

        //2018-02-20
        public ActionResult BackToStockOrDiscard(int? id,string actinName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = db.SalesReturnDetails.Find(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            ViewBag.Action = actinName;
            return View(model);
        }

        [HttpPost]
        public ActionResult BackToStockOrDiscard(string actinName,SalesReturnDetail model)
        {
            SalesReturnDetail srd;
            PSIMS.Models.Sales SStatus;
            ViewBag.Action = actinName;
            if (ModelState.IsValid)
            {
                if(ViewBag.Action == "ToStock")
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        srd = db.SalesReturnDetails.Find(model.ID);

                        srd.QtyBackToStock = model.QtyBackToStock;
                        srd.SalesRetunStatus = model.SalesRetunStatus;
                        db.SaveChanges();

                        _salesReturnService.UpdateReturnBackToStock(srd.StockID, srd.QtyBackToStock);
                        _salesReturnService.UpdateSalesForreturnDetails(srd.ID, actinName);
                        _salesReturnService.UpdateSalesForReturnTOStock(srd.SalesReturnID);

                    }
                }
                else if(ViewBag.Action == "Discard")
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        srd = db.SalesReturnDetails.FirstOrDefault(x=>x.ID==model.ID);
                        srd.DiscartQty = model.QtyBackToStock;
                        srd.SalesRetunStatus = model.SalesRetunStatus;
                        db.SaveChanges();

                        _salesReturnService.UpdateReturnToDiscard(model.ID);
                        _salesReturnService.UpdateSalesForReturnTODiscard(srd.SalesReturn.SalesID);

                    }
                }

                return RedirectToAction("Index"); 
            }
            return View(model);
        }

    }
}