using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSIMS.Models.Finance
{
    public class Audit_tray_recipt_details
    {
        public int ID { get; set; }

        [Display(Name = "Audit Master ID")]
        public int Audit_tray_recipt_masterID { get; set; }

        public int ReceiptMasterID { get; set; }

        public int ReciptDetaisID { get; set; }

        public int InvoiceNo { get; set; }

        [Display(Name = "Receipt Amount")]
        public decimal ReceiptAmount { get; set; }

        public string audit_Mode { get; set; } // insert - Candel - Edit

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        [Display(Name = "Invoice Grand Total")]
        public decimal InvGrandTot { get; set; }

        [Display(Name = "Unit Balance")]
        public decimal? UnitBalance { get; set; }

        public virtual Audit_tray_recipt_master Audit_tray_recipt_master { get; set; }
        public virtual ICollection<PaymentSettelmentDetails> PaymentSettelmentDetails { get; set; }

    }
}