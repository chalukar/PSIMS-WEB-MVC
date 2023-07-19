using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel
{
    public class DayTotalVM
    {
        public int Day { get; set; }
        public decimal Total { get; set; }
    }
    public class SalesDetails
    {
        public DateTime Date { get; set; }
        public List<DayTotalVM> Days { get; set; }
    }
}
