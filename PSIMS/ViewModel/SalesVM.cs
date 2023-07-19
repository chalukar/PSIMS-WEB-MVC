using PSIMS.Models;
using PSIMS.Models.Finance;
using PSIMS.Models.SalesModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel
{
    public class SalesVM
    {
        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
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
        public int IsActive { get; set; }

        public bool IsPayment { get; set; }
        public string PaymentType { get; set; } // defulat:0, Part payment : PP, full payment :FP, Balance Payment = BP


        public virtual ICollection<SalesItem> SalesItems { get; set; }
        public virtual ICollection<SalesReturn> SalesReturns { get; set; }

        public virtual Customer Customer { get; set; }


        public virtual InvoiceOptionEntry invoiceOptionEntry { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
    }
}