using PSIMS.Models.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel.ForReports
{
    public class PaymentSerachVM
    {
        public List<Payment> payment { get; set; }
        //addded
        public string option { get; set; }
        public string customer { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string IsPaid { get; set; }
        public bool IsActive { get; set; }
        public int InvoiceID { get; set; }
    }
}