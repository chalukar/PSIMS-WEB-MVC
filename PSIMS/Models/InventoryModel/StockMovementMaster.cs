using PSIMS.Models.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.Models.InventoryModel
{
    public class StockMovementMaster
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Trnsfer ID")]
        public string ID { get; set; }

        [Display(Name = "SupplierName")]
        public int locationID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public string UserID { get; set; }

        public virtual ICollection<StockMovementDetals> StockMomentDetals { get; set; }

        public virtual Location Location { get; set; }
    }
}