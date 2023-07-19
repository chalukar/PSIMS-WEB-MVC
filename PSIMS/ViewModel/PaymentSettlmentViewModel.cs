using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using PSIMS.Models.Finance;
using PSIMS.Models.Locations;
using PSIMS.Models.SalesModel;


namespace PSIMS.ViewModel
{
    public class PaymentSettlmentViewModel
    {
        public PaymentSettelmentMaster paymentmaster { get; set; }
        public PaymentSettelmentDetails paymentdetails { get; set; }
        public Customer customer { get; set; }
        public Bank bank { get; set; }
        public Location location { get; set; }

      


    }
}