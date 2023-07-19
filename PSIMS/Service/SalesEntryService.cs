using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using PSIMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSIMS.Models;
using PSIMS.Models.Finance;
using PSIMS.Models.QuotationModel;

namespace PSIMS.Service
{
    public class SalesEntryService
    {
        SalesEntryRepository repo;

        public SalesEntryService()
        {
            repo = new SalesEntryRepository();
        }

        public int InsertSales(Sales _sales)
        {
            return repo.InsertSales(_sales);
        }

        public void InsertSalesItem(int _salesID, string[] _stockID, string[] _qty, string[] _rate,string[] _unitDisAmt, string[] _amt, string[] _packSize,string[] _discount_type)
        {
            int count = _stockID.Count();            
                for (int i = 0; i < count; i++)
                {
                    SalesItem _salesItem = new SalesItem();
                    _salesItem.SalesID = _salesID;

                    _salesItem.StockID = Convert.ToInt32(_stockID[i]);
                    _salesItem.LocationStockID = Convert.ToInt32(_stockID[i]);
                    _salesItem.Rate = Convert.ToDecimal(_rate[i]);
                    _salesItem.Qty = Convert.ToDecimal(_qty[i]);
                    _salesItem.UnitDisCountAmt = Convert.ToDecimal(_unitDisAmt[i]);
                    _salesItem.Amount = Convert.ToDecimal(_amt[i]);
                    _salesItem.PackSize = Convert.ToString(_packSize[i]);
                    _salesItem.Date = DateTime.Now;
                    _salesItem.iDiscount_type = Convert.ToInt16(_discount_type[i]);
                    repo.InsertSalesItem(_salesItem);
                }                
            }

        public void UpdateStock(string[] _stockID, string[] _qty)
        {            
            for (int i = 0, y = _stockID.Count(); i < y; i++)
            {
                int getStockID = Convert.ToInt32(_stockID[i]);
                decimal getQty = Convert.ToDecimal(_qty[i]);
                LocationStock stock = new LocationStock();
                repo.UpdateStock(getStockID, getQty);                
            }        
        }

        public List<Sales> GetAllSalesInfo()
        {
            return repo.GetAllSalesInfo();
        }

        public Sales GetSales(int? salesId)
        {
            return repo.GetSales(salesId);
        }

        public Payment GetSalesReceipt(int? salesReceiptId)
        {
            return repo.GetSalesReceipt(salesReceiptId);
        }

        public int UpdateSales(Sales _sales)
        {
            return repo.UpdateSales(_sales);
        }

        //Quotation
        public Quotation GetquotationForm(int? quoteid)
        {
            return repo.GetquotationForm(quoteid);
        }


        //public int InsertPaymentSales(string[] _InvoiceID,string[] _InvoiceAmt,string[] _ReceiptAmt,string[] _PayMode,DateTime? _PaymentDate)
        //{
        //     Payment _payment = new Payment();

        //    _payment.SalesID = Convert.ToInt32(_InvoiceID);
        //    _payment.GrandTotal = Convert.ToDecimal(_InvoiceAmt);
        //    _payment.PaidAmount = Convert.ToDecimal(_ReceiptAmt);
        //    _payment.PayMode = Convert.ToString(_PayMode);
        //    _payment.PaymentDate = Convert.ToDateTime(_PaymentDate);

        //    repo.InsertPaymentSales(_payment);
        //}





        public int InsertPaymentSales(Payment _payment)
        {
            return repo.InsertPaymentSales(_payment);
        }

        
    }

}