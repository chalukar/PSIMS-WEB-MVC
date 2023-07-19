using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIMS.Models.InventoryModel
{
    public class DiscardStock
    {
        
        [Key]
        public int ID { get; set; }

        [Required]
        public string BatchNo { get; set; }

        [Required]
        public int StockID { get; set; }

        [Required]
        public decimal Qty { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        [Required]
        [ForeignKey("SalesReturnDetail")]
        public int SalesReturnDetailID { get; set; }
        //public int LocationStockID { get; set; }


        public virtual SalesReturnDetail SalesReturnDetail { get; set; }
        //public virtual LocationStock LocationStock { get; set; }

    }
}