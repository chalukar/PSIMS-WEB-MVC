using PSIMS.Models.Locations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.Models.PurchaseModel
{
    [Table("Purchase")]
    public class Purchase
    {
        public Purchase()
        {
            isStockTransferred = false;

        }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Display(Name = "Invoice No")]
        public string ID { get; set; }
       
        [DisplayFormat(DataFormatString="{0:yyyy-MM-dd}", ApplyFormatInEditMode= true)]
        public DateTime Date { get; set; }

        [Display(Name="SupplierName")]
        public int SupplierID { get; set; }

        public decimal Amount { get; set; }

        public decimal? Discount { get; set; }

        [Display(Name = "Vat")]
        public decimal? Tax { get; set; }

        [Required]
        public decimal GrandTotal { get; set; }

        public bool IsPaid { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastUpdated { get; set; }

        public string Description { get; set; }

        public string UserID { get; set; }

        public int LocationID { get; set; }

        public bool isStockTransferred { get; set; }


        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }

        public virtual Location Location { get; set; }

    }
}