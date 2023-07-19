using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.Models
{
    public class MenuUserRoleModel
    {
        [Key, Column(Order = 1)]
        public int MenuId { get; set; }
        [Key, Column(Order = 2)]
        public string RoleId { get; set; }
        [Required]
        public Boolean Enabled { get; set; }
    }
}