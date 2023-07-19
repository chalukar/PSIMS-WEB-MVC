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
    [Table("PaymentSettelmentMaster")]
    public class PaymentSettelmentMaster
    {
        [Display(Name = "Receipt No")]
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Receipt Date")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Receipt Amount")]
        public decimal ReceiptAmount { get; set; }

        [Display(Name = "Customer Amount")]
        public decimal CustomerAmount { get; set; }

        [Display(Name = "Balance")]
        public decimal? Balance { get; set; }

        [Display(Name = "Invoice Nos")]
        public string InvoiceNos { get; set; }

        [Display(Name = "Payment Mode")]
        public int PaymentMode { get; set; }

        [Display(Name = "Bank Name")]
        public int? BankID { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Deposit Cash Date")]
        //public DateTime? DepositCashDate { get; set; }

        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[Display(Name = "Deposit Cheque Date")]
        //public DateTime? ChequeDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Payment Deposit Date")]
        public DateTime PaymentDepositDate { get; set; }

        [Display(Name = "Cheque No")]
        public int? ChequeNo { get; set; }

        [Display(Name = "Customer Name")]
        public int CustomerID { get; set; }

        [Display(Name = "Location")]
        public int LocationID { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? LastUpdatedOn { get; set; }

        public string LastUpdatedBy { get; set; }

        public int Countreocrd { get; set; }

        public int isprint { get; set; }

        public bool IsActive { get; set; }

        public int? Audit_tray_recipt_masterID { get; set; }

        [NotMapped]
        public string NumberToWords { get; set; }


        public virtual ICollection<PaymentSettelmentDetails> paymentSettelmentDetails { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Location Location { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual Audit_tray_recipt_master Audit_tray_recipt_master { get; set; }

    }
}