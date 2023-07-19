using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSIMS.Models.InventoryModel;
using PSIMS.Models.PurchaseModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PSIMS.Models.Locations;

namespace PSIMS.Models.PurchaseModel
{
    [Table("PurchaseItem")]
    public class PurchaseItem
    {
        [Key]
        [Column(Order = 0)]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Purchase ID")]
        [ForeignKey("Purchase")]
        public string PurchaseID { get; set; }
        //private string _PurchaseID;
        //public string PurchaseID
        //{
        //    get
        //    {
        //        if (string.IsNullOrEmpty(_PurchaseID))
        //        {
        //            return _PurchaseID;
        //        }
        //        return _PurchaseID.ToUpper();
        //    }
        //    set
        //    {
        //        _PurchaseID = value;
        //    }
        //}

        public int ItemID { get; set; }

        [Display(Name = "Batch No")]
        private string _BatchNo;
        public string BatchNo
        {
            get
            {
                if (string.IsNullOrEmpty(_BatchNo))
                {
                    return _BatchNo;
                }
                return _BatchNo.ToUpper();
            }
            set
            {
                _BatchNo = value;
            }


        }

        [Display(Name = "Pack Size")]
        public string PackSize_Qty { get; set; }

        public string PackSize_Code { get; set; }

        [Range(0.00, 99999999.99, ErrorMessage = "Not an acceptable Quantity!")]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal Qty { get; set; }

        public decimal Qty_Total { get; set; }

        [Range(0, 99999999, ErrorMessage = "Not an acceptable Quantity!")]
        public int? BonusIncluded { get; set; }

        [Range(0.00, 99999999.99)]
        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal CostPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}")]
        public decimal SellingPrice { get; set; }
        public decimal UnitPrice { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Expiry { get; set; }

        public string Is_this_pack_ML { get; set; }

        public virtual Item Item { get; set; }
        public virtual Purchase Purchase { get; set; }


    }
}