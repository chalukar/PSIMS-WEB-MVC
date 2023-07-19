using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.Models
{
    public class MenuModels
    {  
        [Key, Column(Order = 1)]
        public int MenuId { get; set; }
        [Display(Name="Main Menu")]
        public string MainMenuName { get; set; }
        [Display(Name = "Sub Menu")]
        public string SubMenuName { get; set; }
        [Display(Name = "Url")]
        public string Url { get; set; }
        [Display(Name = "Icon")]
        public string FaIconName { get; set; }
        [NotMapped]
        public bool Enabled { get; set; }

        //public string ActionName { get; set; }
    }
}