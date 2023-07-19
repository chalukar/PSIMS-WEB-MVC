using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSIMS.Models.PurchaseModel
{
    [Table("Country")]
    public class Country
    {
        [Key]
        public int CountryID { get; set; }

        [Display(Name="Country Name")]
        public string CountryName { get; set; }
        public string UserID { get; set; }

        public virtual ICollection<Supplier> supplier { get; set; }
    }
}