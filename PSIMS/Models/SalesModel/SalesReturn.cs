using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PSIMS.Models.InventoryModel;
using System.Web;

namespace PSIMS.Models
{
    [Table("SalesReturn")]
    public class SalesReturn
    {
        public int ID { get; set; }
        public int SalesID { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal NetTotal { get; set; }
        public string Description { get; set; }
        public DateTime ReturnedDate { get; set; }
        public int salesReturnStatus { get; set; }

        public string UserID { get; set; }

        public int LocationID { get; set; }

        public virtual ICollection<SalesReturnDetail> SalesReturnDetails { get; set; }
        public virtual Sales Sales { get; set; }

    }


    public class SalesReturnDetail
    {
        public int ID { get; set; }
        public int SalesReturnID { get; set; }
        public int StockID { get; set; }
        public string BatchNo { get; set; }
        public decimal Qty { get; set; }

        [DisplayName("Update Qty")]
        public decimal QtyBackToStock { get; set; }
        [DisplayName("Discard Qty")]
        public decimal DiscartQty { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public string UserID { get; set; }
        public SRStatus SalesRetunStatus { get; set; }

        public int status { get; set; }

        public int LocationID { get; set; }

        public virtual SalesReturn SalesReturn { get; set; }
    }

    public enum SRStatus
    {
        
        ToStock,
        ToDiscard,
        Cancel



    }
}