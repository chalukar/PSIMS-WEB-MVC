using IdentitySample.Models;
using PSIMS.Models;
using PSIMS.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PSIMS.Repository.Reports
{
    public class ProductWiseSalesFilterRepository
    {
        private ApplicationDbContext db;
        public ProductWiseSalesFilterRepository()
        {
            db = new ApplicationDbContext();
        }



        public List<SalesCountVM> Filterproduct(ViewModel.ForReports.ProductWiseSalesVM pws)
        {
            List<SalesCountVM> result  = null;

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (pws != null)
            {
                if (!string.IsNullOrEmpty(pws.option))
                {
                    if (pws.option == "today")
                    {
                        result = db.SalesItems
                                .Where(p => p.Date.Day == DateTime.Today.Day)
                                .Where(p => p.Date.Month == DateTime.Today.Month)
                                .Where(p => p.Date.Year == DateTime.Today.Year)
                                .GroupBy(s => s.LocationStock.Item.Name)
                                .Select(g =>
                                    new SalesCountVM
                                    {
                                        ItemName = g.Key,
                                        Count = g.Count(),
                                        Qty = g.Sum(p => p.Qty),
                                        Amount = g.Sum(p => p.Amount),

                                    }).ToList();


                        //.Where(p => p.Date.Day == DateTime.Today.Day );

                    }
                    else if (pws.option == "yesterday")
                    {
                        result = db.SalesItems
                                 .Where(p => p.Date.Day == DateTime.Today.Day -1)
                                 .Where(p => p.Date.Month == DateTime.Today.Month)
                                 .Where(p => p.Date.Year == DateTime.Today.Year)
                                 .GroupBy(s => s.LocationStock.Item.Name)
                                 .Select(g =>
                                     new SalesCountVM
                                     {
                                         ItemName = g.Key,
                                         Count = g.Count(),
                                         Qty = g.Sum(p => p.Qty),
                                         Amount = g.Sum(p => p.Amount),

                                     }).ToList();
                        //result = result.Where(p => p.Date.Date == DateTime.Now.Date.AddDays(-1));
                    }
                    else if (pws.option == "thisWeek")
                    {
                        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                        result = db.SalesItems
                                .Where(x => x.Date >= startDayOfWeek && x.Date <= endDayOfWeek)
                                .GroupBy(s => s.LocationStock.Item.Name)
                                .Select(g =>
                                    new SalesCountVM
                                    {
                                        ItemName = g.Key,
                                        Count = g.Count(),
                                        Qty = g.Sum(p => p.Qty),
                                        Amount = g.Sum(p => p.Amount),

                                    }).ToList();
                                       // result = result.Where(p=> p.Date.);
                    }
                    else if (pws.option == "thisMonth")
                    {
                        result = db.SalesItems
                                .Where(p => p.Date.Month == month)
                                .GroupBy(s => s.LocationStock.Item.Name)
                                .Select(g =>
                                    new SalesCountVM
                                    {
                                        ItemName = g.Key,
                                        Count = g.Count(),
                                        Qty = g.Sum(p => p.Qty),
                                        Amount = g.Sum(p => p.Amount),

                                    }).ToList();
                       
                    }
                    else if (pws.option == "lastMonth")
                    {
                        result = db.SalesItems
                               .Where(p => p.Date.Month == month - 1)
                               .GroupBy(s => s.LocationStock.Item.Name)
                               .Select(g =>
                                   new SalesCountVM
                                   {
                                       ItemName = g.Key,
                                       Count = g.Count(),
                                       Qty = g.Sum(p => p.Qty),
                                       Amount = g.Sum(p => p.Amount),

                                   }).ToList();
                        
                    }
                    else if (pws.option == "thisYear")
                    {
                        result = db.SalesItems
                              .Where(p => p.Date.Year == year)
                              .GroupBy(s => s.LocationStock.Item.Name)
                              .Select(g =>
                                  new SalesCountVM
                                  {
                                      ItemName = g.Key,
                                      Count = g.Count(),
                                      Qty = g.Sum(p => p.Qty),
                                      Amount = g.Sum(p => p.Amount),

                                  }).ToList();
                  

                    }
                    else if (pws.option == "lastYear")
                    {
                        result = db.SalesItems
                              .Where(p => p.Date.Year == year - 1)
                              .GroupBy(s => s.LocationStock.Item.Name)
                              .Select(g =>
                                  new SalesCountVM
                                  {
                                      ItemName = g.Key,
                                      Count = g.Count(),
                                      Qty = g.Sum(p => p.Qty),
                                      Amount = g.Sum(p => p.Amount),

                                  }).ToList();
                    
                    }
                    if (!string.IsNullOrEmpty(pws.customer))
                    {
                        var value = Convert.ToInt32(pws.customer);
                        if(value !=0)
                        {
                            //query here
                            result = db.SalesItems
                           .Where(x => x.Sales.CustomerID == value)
                            .GroupBy(s => s.LocationStock.Item.Name)
                            .Select(g =>
                                new SalesCountVM
                                {
                                    ItemName = g.Key,
                                    Count = g.Count(),
                                    Qty = g.Sum(p => p.Qty),
                                    Amount = g.Sum(p => p.Amount),

                                }).ToList();
                            //result = result.Where(p => p.SupplierID == value);
                        }
                        
                       
                    }
                    if (pws.fromDate != null || pws.toDate != null)
                    {
                        
                        if (pws.fromDate != null && pws.toDate == null)
                        {
                            //query here
                            //result = result.Where(p => p.Date >= pws.fromDate);
                             result  = db.SalesItems
                            .Where(p => p.Date >= pws.fromDate)
                            .GroupBy(s => s.LocationStock.Item.Name)
                            .Select(g =>
                                new SalesCountVM
                                {
                                    ItemName = g.Key,
                                    Count = g.Count(),
                                    Qty = g.Sum(p => p.Qty),
                                    Amount = g.Sum(p => p.Amount),

                                }).ToList();
                                
                        }
                        else if (pws.fromDate == null && pws.toDate != null)
                        {
                            //query here
                            //result = result.Where(p => p.Date >= pws.fromDate);
                            result = db.SalesItems
                                    .Where(p => p.Date >= pws.fromDate)
                                    .GroupBy(s => s.LocationStock.Item.Name)
                                    .Select(g =>
                                        new SalesCountVM
                                        {
                                            ItemName = g.Key,
                                            Count = g.Count(),
                                            Qty = g.Sum(p => p.Qty),
                                            Amount = g.Sum(p => p.Amount),

                                        }).ToList();
                        }
                        else if (pws.fromDate != null && pws.toDate != null)
                        {
                            //query here
                            // result = result.Where(p => p.Date >= pws.fromDate && p.Date <= pws.toDate);
                            result = db.SalesItems
                                    .Where(p => p.Date >= pws.fromDate && p.Date <= pws.toDate)
                                    .GroupBy(s => s.LocationStock.Item.Name)
                                    .Select(g =>
                                        new SalesCountVM
                                        {
                                            ItemName = g.Key,
                                            Count = g.Count(),
                                            Qty = g.Sum(p => p.Qty),
                                            Amount = g.Sum(p => p.Amount),

                                        }).ToList();
                        }
                    }
                }
            }

            return result;

        }
    }
}