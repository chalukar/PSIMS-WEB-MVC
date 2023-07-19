using PSIMS.Models;
using PSIMS.Models.Finance;
using PSIMS.Models.Locations;
using PSIMS.Models.SalesModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel
{
    public class PaymentSettementVM
    {
        public int ID { get; set; }
        public int SalesID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }
        public decimal NetAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal ReceiptAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; }
        public decimal? Balance { get; set; }
        public bool Status { get; set; }
        public int PaymentType { get; set; }
        public string CompanyName { get; set; }
        public int? BankID { get; set; }
        public string InvoiceLocationName { get; set; }
        public int LocationID { get; set; }
        public int CustomerID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
        public int Countreocrd { get; set; }

        public virtual Sales Sales { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }

    }
}