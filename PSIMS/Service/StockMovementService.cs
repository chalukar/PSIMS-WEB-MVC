using PSIMS.Models.InventoryModel;
using PSIMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace PSIMS.Service
{
    public class StockMovementService
    {
        StockMovementRepository repo;

        public StockMovementService()
        {
            repo = new StockMovementRepository();
        }
        public void UpdateStockMovement(StockMovement stockdist)
        {
            repo.UpdateStockMovement(stockdist);
            //Update The stock quantity from the current location
            //Insert new stock to the new location if same item, batch is not available
            //repo.InsertOrUpdateStockQty(stockdist);
        }

        //public int updatedTraxStock(StockMovement _stockMovement)
        //{
           
        //    return repo.updatedTraxStock(_stockMovement);
        //}

        //public int InsetDaliyStock(StockMovement _stockMovement)
        //{
        //    return repo.InsetDaliyStock(_stockMovement);
        //}



        //public int InsertLocationStock(LocationStock _locationStock)
        //{
        //    return repo.InsertLocationStock(_locationStock);
        //}

        public void UpdateStock(string[] _stockID, string[] _qty)
        {
            for (int i = 0, y = _stockID.Count(); i < y; i++)
            {
                int getStockID = Convert.ToInt32(_stockID[i]);
                decimal getDisQty = Convert.ToDecimal(_qty[i]);
                Stock stock = new Stock();
                repo.UpdateStock(getStockID, getDisQty);
            } 
        }


        public void Updatepurchase(string[] _stockID)
        {
            for (int i = 0, y = _stockID.Count(); i < y; i++)
            {
                int getStockID = Convert.ToInt32(_stockID[i]);
                repo.Updatepurchase(getStockID);

            } 
           
        }

        public void AddStockTtxMaster(StockMovementMaster sm)
        {
            repo.AddStockTtxMaster(sm);
        }



        public void AddStockTrxDetails(List<StockMovementDetals> Trlist)
        {
            foreach (StockMovementDetals item in Trlist)
            {
                repo.AddStockTrxDetails(item);
            }
        }

        public void returnStock(int rtnID, decimal qty)
        {
            repo.returnStock(rtnID, qty);
        }
    }
}