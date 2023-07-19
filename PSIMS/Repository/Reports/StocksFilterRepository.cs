using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using PSIMS.ViewModel;
using PSIMS.ViewModel.ForReports;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace PSIMS.Repository
{
    public class StocksFilterRepository
    {
        private ApplicationDbContext db;
        public StocksFilterRepository()
        {
            db = new ApplicationDbContext();
        }

        public IQueryable<Stock> FilterStocks(StockSearchVM searchModel)
        {
            var result = db.Stocks.Include("Item").AsQueryable();
            if (searchModel != null)
            {
                if (!string.IsNullOrWhiteSpace(searchModel.batch))
                    result = result.Where(x => x.BatchNo == searchModel.batch);
                if (!string.IsNullOrEmpty(searchModel.name))
                    result = result.Where(x => x.Item.Name.Contains(searchModel.name));
                if (!string.IsNullOrEmpty(searchModel.option))
                {
                    if (searchModel.option == "BelowMin")
                    {
                        result = result.Where(x => x.Qty < x.Item.AlertQty);
                    }
                    else if (searchModel.option == "UnSold")
                    {
                        result = result.Where(x => x.Qty == x.InitialQty);
                    }
                    else if (searchModel.option == "Expired")
                    {
                        result = result.Where(x => DbFunctions.DiffDays( DateTime.Now, x.ExpiryDate) <= 0);
                    }
                        
                    else if(searchModel.option == "90")
                    {                        
                        result = result.Where(x => DbFunctions.DiffDays(DateTime.Now, x.ExpiryDate) <= 90 && DbFunctions.DiffDays(DateTime.Now, x.ExpiryDate) > 0);
                    }
                      
                    else if (searchModel.option == "60")
                    {
                        result = result.Where(x => DbFunctions.DiffDays(DateTime.Now, x.ExpiryDate) <= 60 && DbFunctions.DiffDays(DateTime.Now, x.ExpiryDate) > 0);
                    } 
                    else if (searchModel.option == "30")
                    {

                        result = result.Where(x => DbFunctions.DiffDays(DateTime.Now, x.ExpiryDate) <= 30 && DbFunctions.DiffDays(DateTime.Now, x.ExpiryDate) > 0);
                    }
                    else if (searchModel.option == "15")
                    {
                        result = result.Where(x => DbFunctions.DiffDays(DateTime.Now, x.ExpiryDate) <= 15 && DbFunctions.DiffDays(DateTime.Now, x.ExpiryDate) > 0);
                    }
                }
                if ((searchModel.fromDate != null) || (searchModel.toDate != null))
                {
                    if (searchModel.fromDate != null && searchModel.toDate == null)
                    {
                        result = result.Where(x => x.ExpiryDate > searchModel.fromDate);
                    }
                    if (searchModel.toDate != null && searchModel.fromDate == null)
                    {
                        result = result.Where(x => x.ExpiryDate < searchModel.toDate);
                    }
                    if (searchModel.toDate != null && searchModel.fromDate != null)
                    {
                        result = result.Where(x => (x.ExpiryDate > searchModel.fromDate && x.ExpiryDate < searchModel.toDate));
                    }
                }
            }
            return result;
        }

        public IQueryable<StockMovement> FilterTransferIndex(StockTransferNoteVM st)
        {
            //var result = db.PaymentSettelmentMasters.Include("Customer").Include("Bank").AsQueryable();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            var result = db.StockDistributions
                                       .Include(s => s.Stock)
                                       .Include(s => s.FromLocation)
                                       .Include(s => s.ToLocation)
                                       .Include(s => s.Item).AsQueryable();

            if (st != null)
            {


                if (!string.IsNullOrEmpty(st.option))
                {
                    if (st.option == "today")
                    {
                        result = from p in result
                                 where p.TransferdOn.Day == DateTime.Today.Day
                                 where p.TransferdOn.Month == DateTime.Today.Month
                                 where p.TransferdOn.Year == DateTime.Today.Year
                                 select p;

                        //.Where(p => p.Date.Day == DateTime.Today.Day );

                    }
                    else if (st.option == "yesterday")
                    {
                        result = from p in result
                                 where p.TransferdOn.Day == DateTime.Today.Day - 1
                                 where p.TransferdOn.Month == DateTime.Today.Month
                                 where p.TransferdOn.Year == DateTime.Today.Year
                                 select p;
                        //result = result.Where(p => p.Date.Date == DateTime.Now.Date.AddDays(-1));
                    }
                    else if (st.option == "thisWeek")
                    {
                        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                        result = result.Where(x => x.TransferdOn >= startDayOfWeek && x.TransferdOn <= endDayOfWeek);
                        // result = result.Where(p=> p.Date.);
                    }
                    else if (st.option == "thisMonth")
                    {

                        result = result.Where(p => p.TransferdOn.Month == month);
                    }
                    else if (st.option == "lastMonth")
                    {
                        result = result.Where(p => p.TransferdOn.Month == month - 1);
                    }
                    else if (st.option == "thisYear")
                    {
                        result = result.Where(p => p.TransferdOn.Year == year);

                    }
                    else if (st.option == "lastYear")
                    {
                        result = result.Where(p => p.TransferdOn.Year == year - 1);
                    }
                }


                if (st.fromDate != null || st.toDate != null)
                {
                    if (st.fromDate != null && st.toDate == null)
                    {
                        //query here
                        result = result.Where(p => p.TransferdOn >= st.fromDate);
                    }
                    else if (st.fromDate == null && st.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.TransferdOn <= st.fromDate);
                    }
                    else if (st.fromDate != null && st.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.TransferdOn >= st.fromDate && p.TransferdOn <= st.toDate);
                    }
                }


            }

            return result;
        }

     

        public IQueryable<StockMovement> FilterReceivedIndex(StockTransferNoteVM st)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            var result = db.StockDistributions
                                  .Include(s => s.Stock)
                                  .Include(s => s.FromLocation)
                                  .Include(s => s.ToLocation)
                                  .Include(s =>s.Item);

            if (st != null)
            {


                if (!string.IsNullOrEmpty(st.option))
                {
                    if (st.option == "today")
                    {
                        result = from p in result
                                 where p.TransferdOn.Day == DateTime.Today.Day
                                 where p.TransferdOn.Month == DateTime.Today.Month
                                 where p.TransferdOn.Year == DateTime.Today.Year
                                 select p;

                        //.Where(p => p.Date.Day == DateTime.Today.Day );

                    }
                    else if (st.option == "yesterday")
                    {
                        result = from p in result
                                 where p.TransferdOn.Day == DateTime.Today.Day - 1
                                 where p.TransferdOn.Month == DateTime.Today.Month
                                 where p.TransferdOn.Year == DateTime.Today.Year
                                 select p;
                        //result = result.Where(p => p.Date.Date == DateTime.Now.Date.AddDays(-1));
                    }
                    else if (st.option == "thisWeek")
                    {
                        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                        result = result.Where(x => x.TransferdOn >= startDayOfWeek && x.TransferdOn <= endDayOfWeek);
                        // result = result.Where(p=> p.Date.);
                    }
                    else if (st.option == "thisMonth")
                    {

                        result = result.Where(p => p.TransferdOn.Month == month);
                    }
                    else if (st.option == "lastMonth")
                    {
                        result = result.Where(p => p.TransferdOn.Month == month - 1);
                    }
                    else if (st.option == "thisYear")
                    {
                        result = result.Where(p => p.TransferdOn.Year == year);

                    }
                    else if (st.option == "lastYear")
                    {
                        result = result.Where(p => p.TransferdOn.Year == year - 1);
                    }
                }


                if (st.fromDate != null || st.toDate != null)
                {
                    if (st.fromDate != null && st.toDate == null)
                    {
                        //query here
                        result = result.Where(p => p.TransferdOn >= st.fromDate);
                    }
                    else if (st.fromDate == null && st.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.TransferdOn <= st.fromDate);
                    }
                    else if (st.fromDate != null && st.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.TransferdOn >= st.fromDate && p.TransferdOn <= st.toDate);
                    }
                }
            }

            return result;
        }
        public IQueryable<StockMovement> FilterReceiveStock(StockTransferNoteVM st)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            var result = db.StockDistributions
                                  .Include(s => s.Stock)
                                  .Include(s => s.FromLocation)
                                  .Include(s => s.ToLocation)
                                  .Include(s => s.Item)
                                  .Where(s => s.ReceivedQty == null);

            if (st != null)
            {
                if (!string.IsNullOrEmpty(st.option))
                {
                    if (st.option == "today")
                    {
                        result = from p in result
                                 where p.TransferdOn.Day == DateTime.Today.Day
                                 where p.TransferdOn.Month == DateTime.Today.Month
                                 where p.TransferdOn.Year == DateTime.Today.Year
                                 select p;

                        //.Where(p => p.Date.Day == DateTime.Today.Day );

                    }
                    else if (st.option == "yesterday")
                    {
                        result = from p in result
                                 where p.TransferdOn.Day == DateTime.Today.Day - 1
                                 where p.TransferdOn.Month == DateTime.Today.Month
                                 where p.TransferdOn.Year == DateTime.Today.Year
                                 select p;
                        //result = result.Where(p => p.Date.Date == DateTime.Now.Date.AddDays(-1));
                    }
                    else if (st.option == "thisWeek")
                    {
                        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                        result = result.Where(x => x.TransferdOn >= startDayOfWeek && x.TransferdOn <= endDayOfWeek);
                        // result = result.Where(p=> p.Date.);
                    }
                    else if (st.option == "thisMonth")
                    {

                        result = result.Where(p => p.TransferdOn.Month == month);
                    }
                    else if (st.option == "lastMonth")
                    {
                        result = result.Where(p => p.TransferdOn.Month == month - 1);
                    }
                    else if (st.option == "thisYear")
                    {
                        result = result.Where(p => p.TransferdOn.Year == year);

                    }
                    else if (st.option == "lastYear")
                    {
                        result = result.Where(p => p.TransferdOn.Year == year - 1);
                    }
                }


                if (st.fromDate != null || st.toDate != null)
                {
                    if (st.fromDate != null && st.toDate == null)
                    {
                        //query here
                        result = result.Where(p => p.TransferdOn >= st.fromDate);
                    }
                    else if (st.fromDate == null && st.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.TransferdOn <= st.fromDate);
                    }
                    else if (st.fromDate != null && st.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.TransferdOn >= st.fromDate && p.TransferdOn <= st.toDate);
                    }
                }
            }

            return result;
        }
    }
}