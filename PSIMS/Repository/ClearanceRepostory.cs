using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentitySample.Models;
using PSIMS.Models.PurchaseModel;

namespace PSIMS.Repository
{
    public class ClearanceRepostory
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public int ClearanceDuplicationCheck(Clearance clearance)
        {
            //check if the input Location name already exists
            List<Clearance> _clearance = (from c in db.Clearances where (c.InvoiceNo == clearance.InvoiceNo) select c).ToList();
            return _clearance.Count;
        }
    }
}