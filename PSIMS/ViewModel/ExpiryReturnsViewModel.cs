using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel
{
    public class ExpiryReturnsViewModel
    {
        public int StockID { get; set; }
        public string ItemName { get; set; }
        public string Batch { get; set; }
        public DateTime PurchasedDate { get; set; }
        public decimal QtyExpired { get; set; }
        public string SupplierName { get; set; }
        public decimal CostPrice { get; set; }
    }
}

