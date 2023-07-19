using PSIMS.Models.InventoryModel;
using PSIMS.Models.QuotationModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel
{
    public class QuotationItemForSalesVM
    {
        public int ID { get; set; }

        [ForeignKey("Quotation")]
        public int QuotationID { get; set; }

        public string QuotationCode { get; set; }

        public int locationStockID { get; set; }

        public decimal locationStockQty { get; set; }

        public string ItemModel { get; set; }

        public string ItemDesciption { get; set; }

        public string ItemName { get; set; }

        public string PackSize { get; set; }

        public decimal Unitprice { get; set; }

        public decimal QuotationQty { get; set; }

        public decimal QuotationSalesprice { get; set; }

        public decimal QuotationPackRate { get; set; }

        public decimal QuotationTotal { get; set; }

        public string Warranty { get; set; }

        public string SpecialOffer { get; set; }

        public string Loc_BatchNo { get; set; }

        public DateTime? Loc_ExpiryDate { get; set; }

        //public virtual Item Item { get; set; }
        public virtual Quotation Quotation { get; set; }
        public virtual LocationStock LocationStock { get; set; }
    }
}