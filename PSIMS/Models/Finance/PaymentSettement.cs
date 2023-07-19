using PSIMS.Models.Locations;
using PSIMS.Models.SalesModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace PSIMS.Models.Finance
{
    [Table("PaymentSettement")]
    public class PaymentSettement
    {
        [Display(Name = "Receipt No")]
        public int ID { get; set; }

        [Display(Name = "Invoice No")]
        public int SalesID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Amount")]
        public decimal NetAmount { get; set; }

        [Display(Name = "Grand Total")]
        public decimal GrandTotal { get; set; }

        [Display(Name = "Receipt Amount")]
        public decimal ReceiptAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Balance")]
        public decimal? Balance { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public int InvoiceLocationID { get; set; }

        [Display(Name = "Payment Type")]
        public int PaymentType { get; set; }


        public int? BankID { get; set; }

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