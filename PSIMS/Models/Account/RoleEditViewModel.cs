using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PSIMS.Models.Account
{
    public class RoleEditViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        
        public string AccessTypeCode { get; set; }

    }
}