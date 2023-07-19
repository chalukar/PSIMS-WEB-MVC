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
    [Table("PaymentSettelmentDetails")]
    public class PaymentSettelmentDetails
    {
        [Display(Name = "Receipt Details No")]
        public int ID { get; set; }

        [Display(Name = "Receipt Summary No")]
        public int PaymentSettelmentMasterID { get; set; }

        [Display(Name = "Invoice No")]
        public int InvoiceID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Invoice Date")]
        public DateTime InvoiceDate { get; set; }

        [Display(Name = "Invoice Grand Total")]
        public decimal InvGrandTot { get; set; }

        [Display(Name = "Receipt Amount")]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Receipt Amount must be a number only")]
        public decimal ReceiptAmount { get; set; }

        [Display(Name = "Unit Balance")]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Unit Balance Amount must be a number only")]
        public decimal? UnitBalance { get; set; }

        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; } // Full Payment = FP , Part Payment = PP ,Balance Payment = BP //Full Settlement =2 , Part Settlement = 1

        public string InvLocation { get; set; }

        public int InvLocationID { get; set; }

        public int CustomerID { get; set; }

        [Display(Name = "Audit Receipt Master ID")]
        public int Audit_tray_recipt_MasterID { get; set; }

        [Display(Name = "Status")]
        public bool Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public virtual PaymentSettelmentMaster PaymentSettelmentMaster { get; set; }
        public virtual Location Location { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Audit_tray_recipt_details Audit_tray_recipt_details { get; set; }



    }
}