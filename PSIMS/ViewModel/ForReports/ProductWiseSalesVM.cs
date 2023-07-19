using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel.ForReports
{
    public class ProductWiseSalesVM
    {
        public List<SalesCountVM> salesCountVM { get; set; }
        //addded
        public string customer { get; set; }
        public string option { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
    }
}