using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSIMS.Models;
using System.Data.Entity;
using PSIMS.Models.Finance;
using System.Web.Mvc;
using PSIMS.Models.QuotationModel;
using System.Globalization;

namespace PSIMS.Repository
{
    public class SalesEntryRepository 
    {
        private IdentitySample.Models.ApplicationDbContext db;

        public SalesEntryRepository()
        {
            db = new IdentitySample.Models.ApplicationDbContext();
        } 
       
        //returns all the stock items 
        //public List<Stock> GetAllStock()
        //{
        //    return db.Stocks.ToList();
            
        //}

        public List<Sales> GetAllSalesInfo()
        {
            return db.Sales.ToList();
        }
        //Inserts in sales 
        public int InsertSales(Sales _sales)
        {           
            db.Sales.Add(_sales);
            db.SaveChanges();            
            return _sales.ID;
        }

        /*
        public void InsertSalesItem(int _salesID, string[] _stockID, string[] _qty, string[] _rate, string[] _amt)
        {
            int count = _stockID.Count();

            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                for (int i = 0; i < count; i++)
                {
                    SalesItem _salesItem = new SalesItem();
                    _salesItem.SalesID = _salesID;

                    _salesItem.StockID = Convert.ToInt32(_stockID[i]);
                    _salesItem.Rate = Convert.ToDecimal(_rate[i]);
                    _salesItem.Qty = Convert.ToInt32(_qty[i]);
                    _salesItem.Amount = Convert.ToDecimal(_amt[i]);

                    db.SalesItems.Add(_salesItem);                    
                }
                db.SaveChanges();
            }

        }
        */

        public void InsertSalesItem(SalesItem _salesItem)
        {
            try
            {
                db.SalesItems.Add(_salesItem);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                
            }
            
        }
        /*
        public bool UpdateStock(string[] _stockID, string[] _qty)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                for (int i = 0, y = _stockID.Count(); i < y; i++)
                {
                    int getStockID = Convert.ToInt32(_stockID[i]);
                    int getQty = Convert.ToInt32(_qty[i]);
                    Stock stock = new Stock();
                    stock = db.Stocks.Find(getStockID);
                   
                    stock.Qty = stock.Qty - getQty;
                   // db.Stocks.Add(stock);
                    db.SaveChanges();
                }                
            }
            return true;
        }*/

        public void UpdateStock(int getStockID, decimal getQty)
        {

            Stock getstockid = new Stock();
            LocationStock stock = new LocationStock();


            getstockid = (from s in db.Stocks 
                                join ls in db.LocationStocks
                                on s.ID equals ls.StockID 
                                where ls.ID == getStockID
                                select s ).FirstOrDefault();


            stock = db.LocationStocks.Find(getStockID); // get stock details  from location Stock

            decimal getpacksize_qty = Convert.ToInt32(getstockid.PackSize_Qty);  //get packsize_qty from stock selected row

            string q = getQty.ToString("0.00", CultureInfo.InvariantCulture);
            string[] parts = q.Split('.');

            decimal i1 = decimal.Parse(parts[0]);  // y -1
            string i2 =parts[1];  // y -2

            decimal val = Convert.ToDecimal('.' + i2);
            decimal cal = Math.Round(10 / getpacksize_qty, 2);
            decimal cal_1 = ((cal * val) * 10);

            string ConvGetQty = Convert.ToString(cal_1);
            decimal finalGetQty = Convert.ToDecimal(ConvGetQty);
            decimal val1 = Math.Round(finalGetQty, 2);

            decimal val2 = Math.Round(i1, 2);
            decimal val_f = (val2 + val1);

            stock.OutQty = stock.OutQty + val_f; // getQty;
            stock.FinalQty = stock.InQty - stock.OutQty;
            db.SaveChanges();

        }

/*      public List<SalesItem> GetSales(int? salesId)
        {
            var _salesItems = from s in db.SalesItems
                              where s.SalesItem == salesId
                              select s;
                              
            return _salesItems.ToList();
        }*/

        public Sales GetSales(int? salesId)
        {
            var _sales = from s in db.Sales
                              where s.ID == salesId
                              select s;

            return _sales.FirstOrDefault();
        }

        public Payment GetSalesReceipt(int? salesReceiptId)
        {
            
            Payment payment = null;
            List<Payment> _salesReceipt = (from p in db.Payments.Include(b=>b.Bank)
                                where p.ID == salesReceiptId
                         select p).ToList();

            foreach (var p in _salesReceipt) 
            {
             
                payment = new Payment()
                {

                    ID = p.ID,
                    SalesID = p.SalesID,
                    GrandTotal = p.GrandTotal,
                    CompanyName = p.CompanyName,
                    PaymentDate = p.PaymentDate,
                    PayMode = p.PayMode,
                    PaidAmount = p.PaidAmount,
                    ChequeNo = p.ChequeNo,
                    ChequeDate=p.ChequeDate,
                    BankID = p.BankID,
                    Bank = p.Bank,
                    NumberToWords = new Repository.NumberToEnglish().changeToWords(Convert.ToDecimal(p.PaidAmount).ToString())
                };
            }

            return payment;
        }

        public int UpdateSales(Sales _sales)
        {
            db.Sales.Add(_sales);
            db.SaveChanges();
            return _sales.ID;
        }

        public int InsertPaymentSales(Payment _payment)
        {
            db.Payments.Add(_payment);
            db.SaveChanges();
            return _payment.ID;
        }

        public Quotation GetquotationForm(int? quoteid)
        {
            var _quotationFrom = from q in db.Quotations.Include(c => c.Customer).Include(qi => qi.QuotationItems)
                                 where q.ID == quoteid
                                 select q;

            return _quotationFrom.Single();


   

        }
    }
}
