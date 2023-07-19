using IdentitySample.Models;
using PSIMS.Models;
using PSIMS.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Web;

namespace PSIMS.Repository
{
    public class SalesReturnRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public void UpdateReturnBackToStock(int getStockID, decimal getQty)
        {
            LocationStock Locstock = new LocationStock();

            Stock getstockid = new Stock();

            getstockid = (from s in db.Stocks
                          join ls in db.LocationStocks
                          on s.ID equals ls.StockID
                          where ls.ID == getStockID
                          select s).FirstOrDefault();


            Locstock = db.LocationStocks.Find(getStockID);

            decimal getpacksize_qty = Convert.ToInt32(getstockid.PackSize_Qty);

            string q = getQty.ToString("0.00", CultureInfo.InvariantCulture);
            string[] parts = q.Split('.');

            decimal i1 = decimal.Parse(parts[0]);  // y -1
            string i2 = parts[1];  // y -2

            decimal val = Convert.ToDecimal('.' + i2);
            decimal cal = Math.Round(10 / getpacksize_qty, 2);
            decimal cal_1 = ((cal * val) * 10);

            string ConvGetQty = Convert.ToString(cal_1);
            decimal finalGetQty = Convert.ToDecimal(ConvGetQty);
            decimal val1 = Math.Round(finalGetQty, 2);

            decimal val2 = Math.Round(i1, 2);
            decimal val_f = (val2 + val1);

            Locstock.OutQty = Locstock.OutQty - val_f;
            Locstock.FinalQty = Locstock.FinalQty + val_f;
            db.SaveChanges();
        }

  

        public void UpdateReturnToDiscard(int salesReturnDetailId)
        {
            DiscardStock dicrdStck = new DiscardStock();
            var salsRetrnDetal = db.SalesReturnDetails.FirstOrDefault(x => x.ID == salesReturnDetailId);
            if(salsRetrnDetal!=null)
            {
                dicrdStck.BatchNo = salsRetrnDetal.BatchNo;
                dicrdStck.StockID = salsRetrnDetal.StockID;
                dicrdStck.Qty = salsRetrnDetal.Qty;
                dicrdStck.CreatedOn = DateTime.Now;
                dicrdStck.CreatedBy = HttpContext.Current.User.Identity.Name;
                dicrdStck.SalesReturnDetailID = salesReturnDetailId;
                db.DiscardStocks.Add(dicrdStck);
                db.SaveChanges();
            }
            
        }
        public void UpdateSalesForreturnDetails(int salesReturnID,string actinName)
        {
            try
            {
                SalesReturnDetail _Returndetails = new SalesReturnDetail();

                if(actinName == "ToStock")
                {
                    _Returndetails = db.SalesReturnDetails.Find(salesReturnID);
                    _Returndetails.status = 1; // Return To Stock
                    db.SaveChanges();
                }
                else
                {
                    _Returndetails = db.SalesReturnDetails.Find(salesReturnID);
                    _Returndetails.status = 2; // Return To Discard
                    db.SaveChanges();
                }
               
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void UpdateSalesForReturnTODiscard(int salesReturnID)
        {
            try
            {
                SalesReturn _Returnsales = new SalesReturn();
                _Returnsales = db.SalesReturns.Find(salesReturnID);
                _Returnsales.salesReturnStatus = 2; // Return To Discard
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void UpdateSalesForReturnTOStock(int salesReturnID)
        {
            try
            {
                SalesReturn _Returnsales = new SalesReturn();
                _Returnsales = db.SalesReturns.Find(salesReturnID);
                _Returnsales.salesReturnStatus = 1; // Return To Stock
                db.SaveChanges();

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void updatesales(int getsalesID)
        {
            try
            {
                Sales _Returnsales = new Sales();
                _Returnsales = db.Sales.Find(getsalesID);
                _Returnsales.IsActive = 3;

                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public int insertsalsReturn(SalesReturn salesReturn)
        {
            //Add in Sales And save changes 
            db.SalesReturns.Add(salesReturn);
            db.SaveChanges();
            return salesReturn.SalesID;
        }
    }
}