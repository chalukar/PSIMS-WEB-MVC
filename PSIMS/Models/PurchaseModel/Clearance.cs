using PSIMS.Models.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSIMS.Models.PurchaseModel
{
    public class Clearance
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Shipping Invoice No")]
        public string InvoiceNo { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayName("Invoice Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime InvoiceDate { get; set; }

        [Required]
        [DisplayName("Total Qty")]
        public int Qty { get; set; }

        [Required]
        [Range(0.00, 9999999999999999.99)]
        [DisplayName("Total Flight/Shipping Cost")]
        public decimal ShippingCost { get; set; }

        [Required]
        [DisplayName("Exchange Rate(LKR)")]
        public decimal DollerPrice { get; set; }

        [Required]
        [Range(0.00, 9999999999999999.99)]
        [DisplayName("Clearance Amount(LKR)")]
        public decimal ClearanceAmt { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CretaedDate { get; set; }

        public string UserID { get; set; }

        public int LocationID { get; set; }

        public virtual Location Location { get; set; }
    }
}