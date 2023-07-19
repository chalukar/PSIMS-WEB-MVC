using PSIMS.Models.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel
{
    public class ReceiptListVM
    {
        public List<Payment> Payment { get; set; }

        public string searchVal { get; set; }
    }
}