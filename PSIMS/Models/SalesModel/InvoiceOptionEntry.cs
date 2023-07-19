using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.Models.SalesModel
{
    [Table("InvoiceOptionEntry")]
    public class InvoiceOptionEntry
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InvOptID { get; set; }

        [Required]
        [Display(Name = "Invoice Option")]
        [StringLength(100, ErrorMessage = "Cannot accept more than 100 characters")]
        public string InvoiceName { get; set; }

        public virtual ICollection<Sales> Sales { get; set; }
    }
}