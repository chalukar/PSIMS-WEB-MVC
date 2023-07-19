using PSIMS.Models.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSIMS.Models.InventoryModel
{
    public class LocationStock
    {
        public int ID { get; set; }

        [Required]
        public int StockID { get; set; }

        public string TrxID { get; set; }

        public int ItemID { get; set; }

        [StringLength(20, ErrorMessage = "Too long. Plese check again!")]
        [Required]
        public string Loc_BatchNo { get; set; }

        [Required]
        public string Loc_PackSize { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Loc_ExpiryDate { get; set; }

        public string UserID { get; set; }

        public int LocationID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TrxDate { get; set; }

        public decimal InQty { get; set; }

        public decimal OutQty { get; set; }

        public decimal FinalQty { get; set; }
        public int? Status { get; set; }
        public virtual Location Location { get; set; }

        public virtual Stock Stock { get; set; }
        public virtual Item Item { get; set; }


    }
}