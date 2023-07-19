using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PSIMS.Models.PurchaseModel;
using System.Globalization;

namespace PSIMS.Repository
{
    public class StockMovementRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void UpdateStockMovement(StockMovement stockdist)
        {
            db.StockDistributions.Add(stockdist);
            db.SaveChanges();
        }       
               
     
        public void UpdateStock(int getStockID, decimal getDisQty)
        {
            Stock stock = new Stock();
            stock = db.Stocks.Find(getStockID);

            decimal movqty = Convert.ToInt32(stock.PackSize_Qty);
            string q = getDisQty.ToString("0.00", CultureInfo.InvariantCulture);
            string[] parts = q.Split('.');

            decimal i1 = decimal.Parse(parts[0]);  // y -1
            string i2 = parts[1];  // y -2

            decimal val = Convert.ToDecimal('.' + i2);
            decimal cal = Math.Round(10 / movqty, 2);
            decimal cal_1 = ((cal * val) * 10);

            string ConvGetQty = Convert.ToString(cal_1);
            decimal finalGetQty = Convert.ToDecimal(ConvGetQty);
            decimal val1 = Math.Round(finalGetQty, 2);

            decimal val2 = Math.Round(i1, 2);
            decimal val_f = (val2 + val1);

            stock.MovingQty = stock.MovingQty - val_f;
            db.SaveChanges();

            //stock = db.Stocks.Find(getStockID);
            //stock.MovingQty = stock.MovingQty - getDisQty;
            //db.SaveChanges();
        }

        public void Updatepurchase(int getStockID)
        {
            Stock stock = new Stock();
            stock = db.Stocks.Find(getStockID);

            
            Purchase purchase = new Purchase();
            purchase = db.Purchases.Find(stock.PurchaseID);
                        //.Where(p => p.ID == stock.PurchaseID).SingleOrDefault();

            purchase.isStockTransferred = true;
            db.SaveChanges();
                       
             
        }
        //Developing  code-----------------------
        public void AddStockTtxMaster(StockMovementMaster sm)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {

                db.StockMovementMasters.Add(sm);
                db.SaveChanges();
            }
        }


        public void AddStockTrxDetails(StockMovementDetals smd)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                StockMovementDetals _stockTrxDetails = new StockMovementDetals();

                //Initialize new stock with vm inserted values
                _stockTrxDetails.TransferID = smd.TransferID;
                _stockTrxDetails.StockID = smd.StockID;
                _stockTrxDetails.ItemID = smd.ItemID;
                _stockTrxDetails.FromLocationID = smd.FromLocationID;
                _stockTrxDetails.ToLocationID = smd.ToLocationID;
                _stockTrxDetails.InitQty = smd.InitQty;
                _stockTrxDetails.DistributedQty = smd.DistributedQty;
                _stockTrxDetails.Remarks = smd.Remarks;
                //_stockTrxDetails.TransferdBy = User.Identity.GetUserName();
                _stockTrxDetails.TransferdOn = DateTime.Now;
                _stockTrxDetails.Status = 0;


                db.StockMovementDetals.Add(_stockTrxDetails);
                db.SaveChanges();

            }
            
        }

        public void returnStock(int rtnID, decimal qty)
        {
            Stock stock = new Stock();
            stock = db.Stocks.Find(rtnID);
            stock.MovingQty = stock.MovingQty + qty;
            db.SaveChanges();
        }

        //Developing  code-----------------------
    }
}