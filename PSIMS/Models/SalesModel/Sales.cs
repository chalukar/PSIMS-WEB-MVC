using PSIMS.Models.Finance;
using PSIMS.Models.Locations;
using PSIMS.Models.SalesModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace PSIMS.Models
{
    [Table("Sales")]
    public class Sales
    {
        public Sales()
        {
            IsActive = 1;
        }
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}" , ApplyFormatInEditMode= true)]
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal GrandTotal { get; set; }
        public string UserID { get; set; }
        public int LocationID { get; set; }
        public string Remarks { get; set; }
        public string referance { get; set; }

        public int CustomerID { get; set; }

         //Foreign key
        public int InvOptID { get; set; }
        public int IsActive { get; set; } // 1 :Active , 2: Cancel , 3:Return To Stock 4: return to Discard

        public bool IsPayment { get; set; }
        public string PaymentType { get; set; } // defulat:0, partion payment : 1, full payment :2
        public decimal? unitbalance { get; set; }
        public decimal? LastReceiptAmt { get; set; }


        public virtual ICollection<SalesItem> SalesItems { get; set; }
        public virtual ICollection<SalesReturn> SalesReturns { get; set; }

        public virtual Customer Customer { get; set; }


        public virtual InvoiceOptionEntry invoiceOptionEntry { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

    }
   
}