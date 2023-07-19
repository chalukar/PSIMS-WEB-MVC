using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSIMS.Models.Finance
{
    public class Audit_tray_recipt_master
    {
        public int ID { get; set; }

        public int ReceiptMasterID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Customer Amount")]
        public decimal CustomerAmount { get; set; }

        [Display(Name = "Receipt Amount")]
        public decimal ReceiptAmount { get; set; }

        public string audit_Mode { get; set; } // insert - Candel - Edit

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public virtual ICollection<Audit_tray_recipt_details> Audit_tray_recipt_details { get; set; }
    }
}