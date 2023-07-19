using PSIMS.Models.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.Models.InventoryModel
{
    [Table("Stock")]
    public class Stock
    {
        public Stock()
        {
            BatchNo = "--";
            Stop_Notification = true;
            ItemExpired = false;
            this.BatchNo = this.BatchNo.ToUpper();
            LocationID = 1;

        }
        public int ID { get; set; }
        [Required]
        public int ItemID { get; set; }

        [StringLength(20, ErrorMessage="Too long. Plese check again!")]

        [Required]
        public string BatchNo { get; set; }


        [Required]
        public string PackSize { get; set; }

        public string PackSize_Qty { get; set; }

        public string PacksizeCode { get; set; }

        [Range(0.00,9999999.99)]
        public decimal InitialQty { get; set; }

        [Required]
        [Range(0.00,1000000.99)]
        public decimal Qty { get; set; }

        [Required]
        [Range(0.00, 1000000.99)]
        public decimal? MovingQty { get; set; }

        [Required]
        [Range(0.00, 1000000.99,ErrorMessage="Out of range!")]
        public decimal CostPrice { get; set; }

        [Required]
        [Range(0.00, 1000000.99, ErrorMessage = "Out of range!")]
        public decimal SellingPrice { get; set; }
        public decimal unitprice { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}",ApplyFormatInEditMode=true)]
        public DateTime? ManufacturedDate { get; set; }

     
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }

        public bool ItemExpired { get; set; }

        public bool Stop_Notification { get; set; }

        public string PurchaseID { get; set; }

        public int LocationID { get; set; }

        public string Is_This_ML { get; set; }


        private DateTime _CreatedOn = DateTime.Now;
        public DateTime CreatedOn { get { return _CreatedOn; } set { _CreatedOn = value; } }
        
        //references
        public virtual Item Item { get; set; }

        public virtual Location Location { get; set; }

        public virtual ICollection<StockMovement> StockDistribution { get; set; }
        public virtual ICollection<LocationStock> LocationStock { get; set; }

    }
}