using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PSIMS.ViewModel
{
    public class StockMomentEntryVM
    {

        ApplicationDbContext db = new ApplicationDbContext();

        public string ID { get; set; }
        public int locationID { get; set; }

        public DateTime Date { get; set; }



        public List<StockMovementDetals> StockMomentDetals { get; set; }

    }
}