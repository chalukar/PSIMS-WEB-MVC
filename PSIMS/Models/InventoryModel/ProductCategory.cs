using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace PSIMS.Models.InventoryModel
{
    [Table("ProductCategory")]
    public class ProductCategory
    {

        public int ID { get; set; }

        [Required]
        [Display(Name="Category Name")]
        public string CategoryName { get; set; }
        public string UserID { get; set; }

        public int LocationID { get; set; }

        public virtual ICollection<Item> Items { get; set; }

    }
}