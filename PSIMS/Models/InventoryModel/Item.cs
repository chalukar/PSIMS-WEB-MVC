using PSIMS.Models.PurchaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;


namespace PSIMS.Models.InventoryModel
{
    [Table("Item")]
    public class Item
    {
        public int ID { get; set; }

        [Required]
        [Display(Name="Item Code")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Manufacturer")]
        public int? ManufacturerID { get; set; }

        [Required]
        [Display(Name = "Categeory")]
        public int? ProductCategoryID { get; set; }

        [Required]
        [Display(Name = "Alert Qty")]
        public int AlertQty { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Last Update")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastUpdated { get; set; }

        public string UserID { get; set; }

        public int LocationID { get; set; }

        private DateTime _CreatedOn = DateTime.Now;
        public DateTime CreatedOn { get { return _CreatedOn; } set { _CreatedOn = value; } }


        //reference entity
        public virtual Manufacturer Manufacturer { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<PurchaseItem> PurchaseItems { get; set; }
        public virtual ICollection<StockMovement> StockDistribution { get; set; }
        
    }

    public enum Measurement
    {
        ml, mg, gm, kg, others

    }

    public enum UnitType
    {
        pkg, file, pcs, other
    }
}


