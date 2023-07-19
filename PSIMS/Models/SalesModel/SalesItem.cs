using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSIMS.Models.InventoryModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIMS.Models
{
    [Table("SalesItem")]
    public class SalesItem
    {
        public  SalesItem()
        {
            IsActive = 1;
        }
        [Key]
        public int ID { get; set; }
        public int StockID { get; set; }
        public int SalesID { get; set; }
        [Required]
        public decimal Qty { get; set; }
        [Required]
        public decimal Rate { get; set; }
        public decimal UnitDisCountAmt { get; set; }
        public decimal Amount { get; set; }
        public string PackSize { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        public int LocationStockID { get; set; }
        public int iDiscount_type { get; set; }
        public int IsActive { get; set; }

        //public virtual Stock Stock { get; set; }
        public virtual LocationStock LocationStock { get; set; }
        public virtual Sales Sales { get; set; }

 
    }
}