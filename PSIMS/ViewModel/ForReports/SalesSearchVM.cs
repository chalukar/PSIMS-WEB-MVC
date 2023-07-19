using PSIMS.Models;
using PSIMS.Models.SalesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel
{
    public class SalesSearchVM
    {
        public List<SalesItem> salesItem { get; set; }
        //addded
        public string option { get; set; }
        public string customer { get; set; }
        public string Company { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public int IsActive { get; set; }
        public string PaymentType { get; set; }
        public int invoiceNo { get; set; }
        public int salesID { get; set; }
    }
}