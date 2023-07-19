using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using PSIMS.Models.Account;

namespace PSIMS.Models.InventoryModel
{
    public class AccessType
    {
        [Key]
        [MaxLength(10)]
        public string Code { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<RoleAccessType> RoleAccessTypes { get; set; }


    }
}