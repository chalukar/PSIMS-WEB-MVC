using PSIMS.Models.SalesModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.Models.Finance
{
    [Table("Payment")]
    public class Payment
    {
        [Display(Name="Receipt No")]
        public int ID { get; set; }

        [Display(Name = "Invoice No")]
        public int SalesID { get; set; }

        [Display(Name = "Grand Total")]
        public decimal GrandTotal { get; set; }

        [Display(Name = "Paid Amount")]
        public decimal PaidAmount { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Balance")]
        public decimal? Balance { get; set; }

        [Display(Name = "Pay Mode")]
        public string PayMode { get; set; }

        [Display(Name = "Chq.No")]
        public string ChequeNo { get; set; }

        [Display(Name = "Chq.Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ChequeDate { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public int? BankID { get; set; }

        public string UserID { get; set; }

        public int LocationID { get; set; }

        public int CustomerID { get; set; }

        [NotMapped]
        public string NumberToWords { get; set; }

        public virtual Sales Sales { get; set; }

        public virtual Bank Bank { get; set; }

        public virtual Customer Customer { get; set; }





        


    }
}