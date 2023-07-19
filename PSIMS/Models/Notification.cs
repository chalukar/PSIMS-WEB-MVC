using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PSIMS.Models
{
    [Table("Notification")]
    public class Notification
    {
        public Notification()
        {
            LowStock = 0;
            ToExpire = 0;
        }
        public int ID { get; set; }
        public int LowStock { get; set; }
        public int ToExpire { get; set; }

     

    }
}