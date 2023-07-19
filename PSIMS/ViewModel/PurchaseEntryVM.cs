using IdentitySample.Models;
using PSIMS.Models.PurchaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSIMS.ViewModel
{
    public class PurchaseEntryVM
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public string ID { get; set; }
        public DateTime Date { get; set; }
        public int SupplierID { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }

        public decimal GrandTotal { get; set; } 
        public bool IsPaid { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdated { get; set; }

        
        public List<PurchaseItem> PurchaseItems { get; set; }

    }
}