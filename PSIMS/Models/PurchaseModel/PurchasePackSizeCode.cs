using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSIMS.Models.InventoryModel;
using PSIMS.Models.PurchaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PSIMS.Models.Locations;

namespace PSIMS.Models.PurchaseModel
{
    [Table("PurchasePacSizekCode")]
    public class PurchasePackSizeCode
    {
        public int ID { get; set; }

        [Display(Name = "Pack Size Code")]
        public string PackSize_Code { get; set; }
        public string UserID { get; set; }
        public int LocationID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedOn { get; set; }
        public virtual Location Location { get; set; }
    }
}