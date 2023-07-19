using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.Models
{
    public class MenuViewModal
    {
        public int MenuId { get; set; }
        public string MainMenuName { get; set; }
        public string SubMenuName { get; set; }
        public string Url { get; set; }
        public string FaIconName { get; set; }
        public bool Enabled { get; set; }
        public string RoleId { get; set; }
    }
}