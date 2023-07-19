using PSIMS.Models.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel
{
    public class StockDistributionVM
    {
        public int ID { get; set; }

        public string transferID { get; set; }

        public string BatchNo { get; set; }

        public int ItemID { get; set; }

        public int StockID { get; set; }

        public int FromLocationID { get; set; }


        public int ToLocationID { get; set; }

        public decimal DistQty { get; set; }
        public decimal InitQty { get; set; } 

        public string Remarks { get; set; }

        public string CreateBy { get; set; }

        private DateTime _CreatedOn = DateTime.Now;
        public DateTime CreatedOn { get { return _CreatedOn; } set { _CreatedOn = value; } }

        [ForeignKey("FromLocationID")]
        public virtual Location FromLocation { get; set; }

        [ForeignKey("ToLocationID")]
        public virtual Location ToLocation { get; set; }
    }
}