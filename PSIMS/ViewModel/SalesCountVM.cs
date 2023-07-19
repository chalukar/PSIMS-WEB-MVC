using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel
{
    public class SalesCountVM
    {
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int Count { get; set; }
        public decimal Qty { get; set; }
        public decimal Amount { get; set; }

        public DateTime Date { get; set; } 



    }
}