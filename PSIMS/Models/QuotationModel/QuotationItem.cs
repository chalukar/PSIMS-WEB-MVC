using PSIMS.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.Models.QuotationModel
{
    [Table("QuotationItem")]

    public class QuotationItem
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Quotation ID")]
        [ForeignKey("Quotation")]
        public int QuotationID { get; set; }

        [Display(Name = "Quotation order No")]
        public string QuotationCode { get; set; }

        [Display(Name = "Location Stock ID")]
        public int LocationStockID { get; set; }

        [Display(Name = "Item Model")]
        public string ItemModel { get; set; }

        [Display(Name = "Item Description")]
        public string ItemDesciption { get; set; }

        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Pack Size")]
        public string PackSize { get; set; }

        [Display(Name = "Quotation Qty")]
        [Range(0.00, 99999999.99, ErrorMessage = "Not an acceptable Quantity!")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal QuotationQty { get; set; }

        [Display(Name = "Sales Unit Price")]
        [Range(0.00, 99999999.99)]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Unitprice { get; set; }

        [Display(Name = "Quotation Sales Price")]
        [Range(0.00, 99999999.99)]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal QuotationSalesprice { get; set; }

        [Display(Name = "Quotation Pack Rate")]
        [Range(0.00, 99999999.99)]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal QuotationPackRate { get; set; }

        [Display(Name = "Quotation Total")]
        [Range(0.00, 99999999.99)]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal QuotationTotal { get; set; }

        [Display(Name ="Warranty")]
        public string Warranty { get; set; }

        [Display(Name = "Special Offer")]
        public string SpecialOffer { get; set; }

        //public virtual Item Item { get; set; }
        public virtual Quotation Quotation { get; set; }


    }
}