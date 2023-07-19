using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using PSIMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSIMS.Models;

namespace PSIMS.Service
{
    public class SalesReturnService
    {
        SalesReturnRepository repo;

        public SalesReturnService()
        {
            repo = new SalesReturnRepository();
        }

        public void UpdateReturnBackToStock(int _stockID, decimal _qty)
        {
            repo.UpdateReturnBackToStock(_stockID, _qty);
        }

        public void UpdateReturnToDiscard(int salesReturnDetailID)
        {
            repo.UpdateReturnToDiscard(salesReturnDetailID);
        }

        public int insertsalesReturn(SalesReturn salesReturn)
        {
            return repo.insertsalsReturn(salesReturn);
        }

        public void updatesales(int salesID)
        {
            //int countSalesID = salesID.Count();
            //for (int y = 0; y < countSalesID; y++)
            //{
            //    int _getsalesID = Convert.ToInt32(salesID[y]);
               
            //}
            repo.updatesales(salesID);
        }

        public void UpdateSalesForReturnTOStock(int salesReturnID)
        {
            repo.UpdateSalesForReturnTOStock(salesReturnID);
        }

        public void UpdateSalesForReturnTODiscard(int salesReturnID)
        {
            repo.UpdateSalesForReturnTODiscard(salesReturnID);
        }

        public void UpdateSalesForreturnDetails(int salesReturnID, string actinName)
        {
            repo.UpdateSalesForreturnDetails(salesReturnID, actinName);
        }

 
    }
}