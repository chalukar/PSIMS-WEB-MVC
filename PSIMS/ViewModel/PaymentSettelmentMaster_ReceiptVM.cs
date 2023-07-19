using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using IdentitySample.Models;
using PSIMS.Models;
using PSIMS.Models.Finance;
using PSIMS.Models.SalesModel;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using PSIMS.Models.Locations;

namespace PSIMS.ViewModel
{
    public class PaymentSettelmentMaster_ReceiptVM
    {

        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; }
        public decimal ReceiptAmount { get; set; }
        public decimal CustomerAmount { get; set; }
        public decimal? Balance { get; set; }
        public string InvoiceNos { get; set; }
        public int PaymentMode { get; set; }
        public int? BankID { get; set; }
        public DateTime? ChequeDate { get; set; }
        public int? ChequeNo { get; set; }
        public int CustomerID { get; set; }
        public int LocationID { get; set; }
        public int Countreocrd { get; set; }
        public int isprint { get; set; }
        public bool IsActive { get; set; }
        public string CompanyName { get; set; }
        public string NumberToWords { get; set; }

        public virtual ICollection<PaymentSettelmentDetails> paymentSettelmentDetails { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual Bank Bank { get; set; }
    }
}