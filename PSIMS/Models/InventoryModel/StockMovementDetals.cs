using PSIMS.Models.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace PSIMS.Models.InventoryModel
{
    public class StockMovementDetals
    {
        public int ID { get; set; }

        public string TransferID { get; set; }

        public int ItemID { get; set; }

        public int StockID { get; set; }


        public int FromLocationID { get; set; }

        public int ToLocationID { get; set; }

        [Display(Name = "Initial Qty")]
        public int InitQty { get; set; }

        public int DistributedQty { get; set; }

        public string Remarks { get; set; }

        public string TransferdBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TransferdOn { get; set; }

        public string ReceivedBy { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ReceivedOn { get; set; }

        public int? ReceivedQty { get; set; }

        public int? Status { get; set; }

        public int? LocationID { get; set; }

        public virtual Item Item { get; set; }

        public virtual Location FromLocation { get; set; }

        public virtual Location ToLocation { get; set; }


        public virtual Stock Stock { get; set; }

        public virtual StockMovementMaster StockMomentMaster { get; set; }
    }
}