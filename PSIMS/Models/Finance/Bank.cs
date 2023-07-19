using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.Models.Finance
{
    [Table("Bank")]
    public class Bank
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Bank Code")]
        public int BankCode { get; set; }

        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        public string UserID { get; set; }

        public int LocationID { get; set; }

        public virtual ICollection<Payment> Payment { get; set; }
        public virtual ICollection<PaymentSettelmentMaster> PaymentSettelmentMaster { get; set; }


    }
}