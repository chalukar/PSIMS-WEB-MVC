using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSIMS.Models.PurchaseModel;
using IdentitySample.Models;

namespace PSIMS.Repository
{
    public class SupplierRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public int SupplierDuplicationCheck(Supplier supplier)
        {
            //check if the input supplier name already exists
            List<Supplier> _supplier = (from s in db.Suppliers
                                        where s.SupplierName == supplier.SupplierName
                                        select s).ToList();
            return _supplier.Count;            
        }
       
    }
}