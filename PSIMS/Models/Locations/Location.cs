using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using PSIMS.Models.PurchaseModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSIMS.Models.Locations
{
     [Table("Location")]
    public class Location
    {
        public int ID { get; set; }

        [Display(Name = "Location Code")]
        [Required(ErrorMessage = "Location Code is required.")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only Alphabets allowed.")]
        [StringLength(3, ErrorMessage = "The Location Code must contains 3 characters", MinimumLength = 3)]
        public string LocationCode { get; set; }

        [Display(Name = "Location Name")]
        [Required(ErrorMessage = "Location Name is required.")]
        [RegularExpression("^[a-zA-Z\\s]+$", ErrorMessage = "Only Alphabets allowed.")]
        [StringLength(150, ErrorMessage = "Cannot accept more than 150 characters")]
        public string LocationName { get; set; }

        public StatusList Status { get; set; }
        public IEnumerable<SelectListItem> TypeOfStatus { get; set; }
        public Location()
        {
            TypeOfStatus = Enum.GetNames(typeof(StatusList)).Select(name => new SelectListItem()
            {
                Text = name,
                Value = name
            });
        }

        public bool IsDefault { get; set; }

        public string UserID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreateOn { get; set; }

        public virtual ICollection<EditUserViewModel> EditUserViewModel { get; set; }
        //public virtual ICollection<StockDistribution> StockDistribution { get; set; }
        public virtual ICollection<StockMovement> FromStockDistribution { get; set; }
        public virtual ICollection<StockMovement> ToStockDistribution { get; set; }
        public virtual ICollection<Stock> Stock { get; set; }
    



    }
}