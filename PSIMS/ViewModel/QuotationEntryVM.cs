using IdentitySample.Models;
using PSIMS.Models.QuotationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PSIMS.ViewModel
{
    public class QuotationEntryVM
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public string QuotationCode { get; set; }
        public int QuotationCategoryID { get; set; }
        public DateTime QuoteDate { get; set; }
        public DateTime ExpiryQuote { get; set; }
        public int CustomerID { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Vat { get; set; }
        public decimal GrandTotal { get; set; }
        public string Remarks { get; set; }
        public bool bActive { get; set; }
        public string YourRef { get; set; }
        public string Header_Note_Line01 { get; set; }
        public string Header_Note_Line02 { get; set; }
        public string Speical_Offers { get; set; }
        public string Terms_Conditions { get; set; }



        public List<QuotationItem> QuotationItems { get; set; }
    }
}