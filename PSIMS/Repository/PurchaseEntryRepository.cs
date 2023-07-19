using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using PSIMS.Models.PurchaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Validation;


/*  Steps:
    * check if item with same batch and ID already exixts
    *   if No insert new item 
    *   if yes check if the cost price matches 
    *      if yes update the quantity and initial quatity
    *      else insert new item
   */

namespace PSIMS.Repository
{
    public class PurchaseEntryRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public int PurchaseEntryDuplicationCheck(Purchase purchase)
        {
            //check if the input Location name already exists
            List<Purchase> _purchase = (from b in db.Purchases where (b.ID == purchase.ID)  select b).ToList();
            return _purchase.Count;
        }


        private Stock _stock;


        /// <summary>
        /// Add Items to PurchaseItems
        /// </summary>
        /// <param name="pi"></param>
        public void AddPurchaseAndPurchseItems(Purchase p)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    db.Purchases.Add(p);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
               
            }
        }
        
        /// <summary>
        /// Insert or update stock based on purchase Items
        /// </summary>
        /// <param name="vm"></param>
        public void InsertOrUpdateInventory(PurchaseItem vm)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                _stock = new Stock();

                decimal UQ = vm.Qty;
                decimal PS = Convert.ToInt32(vm.PackSize_Qty);
                decimal SP = vm.SellingPrice;
                decimal UP = SP / PS;
                //Get Pack Size Code Name
                int PackSizeCodeID = Convert.ToInt32(vm.PackSize_Code);
                var checkPacksizeCodename = (from ps in db.PurchasePackSizeCodes
                                            where ps.ID == PackSizeCodeID
                                            select ps.PackSize_Code).SingleOrDefault();
                //Initialize new stock with vm inserted values
                _stock.ItemID = vm.ItemID;
                _stock.BatchNo = vm.BatchNo;
                _stock.PackSize = vm.PackSize_Qty +""+ checkPacksizeCodename;
                _stock.PackSize_Qty = vm.PackSize_Qty;
                _stock.PacksizeCode = vm.PackSize_Code;
                _stock.CostPrice = vm.CostPrice;
                _stock.SellingPrice = vm.SellingPrice;
                _stock.unitprice = UP;
                _stock.ExpiryDate = vm.Expiry;
                _stock.PurchaseID = vm.PurchaseID;
                _stock.Is_This_ML = vm.Is_this_pack_ML;


                //Get list of all the inserted item in Stock table
                List<Stock> _checkItem = (from s in db.Stocks
                                          where s.ItemID == vm.ItemID && s.BatchNo == vm.BatchNo
                                          select s).ToList();

                //count the number of exixting record on inserted item
                int countStock = _checkItem.Count();

                //Add new record if record is not found
                if (countStock == 0)
                {
                    try
                    {
                        //decimal TQty = UQ * PS;
                        //Add new item with new Initial qty
                        _stock.Qty = vm.Qty;
                        _stock.InitialQty = _stock.Qty;
                        _stock.MovingQty = _stock.Qty;
                        db.Stocks.Add(_stock);
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                   
                }
                else
                {
                    //to check how many times loop executes completely 
                    int loopCount = 0;
                    //Check and Add or update
                    foreach (Stock stock in _checkItem)
                    {
                        if (stock.CostPrice == vm.CostPrice && stock.SellingPrice == vm.SellingPrice)
                        {
                            try
                            {
                                //decimal TQty = UQ * PS;
                                //Update qty and InitialQty 
                                stock.Qty += vm.Qty;
                                stock.InitialQty += vm.Qty;
                                db.SaveChanges();
                                break;
                            }
                            catch (DbEntityValidationException e)
                            {
                                foreach (var eve in e.EntityValidationErrors)
                                {
                                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                    foreach (var ve in eve.ValidationErrors)
                                    {
                                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                            ve.PropertyName, ve.ErrorMessage);
                                    }
                                }
                                throw;
                            }
                           
                        }
                        loopCount++;
                    }
                    if (loopCount == _checkItem.Count())
                    {
                        try
                        {
                            _stock.Qty = vm.Qty;
                            _stock.InitialQty = _stock.Qty;
                            _stock.MovingQty = _stock.Qty;
                            db.Stocks.Add(_stock);
                            db.SaveChanges();

                            // decimal TQty = UQ * PS;
                            //Add new record with Qty and intial Qty

                            //_stock.InitialQty += vm.Qty;
                            //db.Stocks.Add(_stock);
                            //db.SaveChanges();
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            throw;
                        }
                      
                    }
                }
            }
        }


        /// <summary>
        /// Checks if invoice number already exists in database. 
        /// If no saves the entry
        /// </summary>
        /// <param name="_purchase"></param>
        /// <returns>1 as success, 0 as failure</returns>

        public void InsertIntoPurchase(Purchase _purchase)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Purchases.Add(_purchase);
                db.SaveChanges();
            }
        }

        
    }
}