using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSIMS.Models.InventoryModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;
namespace PSIMS.Models.Account
{
    public class RoleAccessType
    {
        [Required]
        [ForeignKey("AccessType")]
        public string AccessTypeCode { get; set; }

        [Key]
        [ForeignKey("Role")]
        public string RoleID { get; set; }

        public virtual AccessType  AccessType { get; set; }
        public virtual IdentityRole Role { get; set; }
    }
}