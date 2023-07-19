using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel.ForReports
{
    public class StockTransferNoteVM
    {
        public List<StockDistributionVM> StockDistributionVM{ get; set; }
        //addded
        public string TransferID { get; set; }
        public string option { get; set; }
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }

    }
}