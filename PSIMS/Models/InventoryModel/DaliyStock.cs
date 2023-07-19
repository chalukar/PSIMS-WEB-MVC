using PSIMS.Models.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSIMS.Models.InventoryModel
{
    public class DaliyStock
    {
        public int ID { get; set; }

        public int StockID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime TrxDate { get; set; }

        public int LocationID { get; set; }

        public decimal InQty { get; set; }

        public decimal OutQty { get; set; }

        public string UserID { get; set; }

        public virtual Location Location { get; set; }

        public virtual Stock Stock { get; set; }
    }
}