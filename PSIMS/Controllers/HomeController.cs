using IdentitySample.Models;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using PSIMS.Models.InventoryModel;
using System;
using PSIMS.ViewModel;
using PSIMS.Models.PurchaseModel;
using System.Data.Entity;
using DotNet.Highcharts;
using DotNet.Highcharts.Options;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using PSIMS.Controllers;
using PSIMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using PSIMS.Models.Account;

namespace IdentitySample.Controllers
{
   [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> manager = null;
        private RoleManager<IdentityRole> roleManager = null;
        IFormatProvider theCultureInfo = new System.Globalization.CultureInfo("en-GB", true);
       
        public HomeController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
           
        }

        public ActionResult Index()
        {
            ViewBag.todaySales = TodaySales();
            ViewBag.yesterdaySales = YesterdaySales();
            ViewBag.todayProductCount = TodaySalesCount(); //Today's Sales - Product Wise
            ViewBag.todaySalesSubTot = TodaySalesSubTot(); //Today's Sales - Product Wise
            ViewBag.todaySalesDis = TodaySalesDis(); //Today's Sales - Product Wise
            ViewBag.todaySalesVat = TodaySalesVat(); //Today's Sales - Product Wise

            ViewBag.yesterdayProductCount = YesterdaySalesCount(); //Yesterday's Sales - Product Wise
            ViewBag.yesterdaySalesSubTot = YesterdaySalesSubTot(); //Yesterday's Sales - Product Wise
            ViewBag.yesterdaySalesDis = YesterdaySalesDis(); //Yesterday's Sales - Product Wise
            ViewBag.yesterdaySalesVat = YesterdaySalesVat(); //Yesterday's Sales - Product Wise
            return View();
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

//*********************************************
        //try using raw sql for efficiency


        /// <summary>
        /// Counts low stock level stocks 
        /// </summary>
        /// <returns>json data for total low stock</returns>
        public ActionResult StockLevel()
        {
            
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();

                if(role == null)
                {
                    return RedirectToAction("Login");

                }
                else
                {
                    RoleAccessType accType = db.RoleAccessTypes.FirstOrDefault(rat => rat.RoleID == role.RoleId);

                    if (accType.AccessTypeCode.Equals("ALLW"))
                    {
                        var list = db.Stocks.SqlQuery("select * from dbo.Stock s, dbo.Item i where i.ID = s.ItemID and s.Qty<i.AlertQty and s.Qty>0").ToList();
                        return Json(list.Count(), JsonRequestBehavior.AllowGet);
                        //return View(stockDistributions.ToList());
                    }
                    else
                    {
                        int UserLocationID = Convert.ToInt32(Session["LocationID"]);
                       // var list = db.LocationStocks.SqlQuery("select * from dbo.LocationStock s,  dbo.Stock i where i.ID = s.ItemID and s.Qty<i.AlertQty and s.Qty>0 and s.LocationID = UserLocationID").ToList();
                        var list = db.Stocks.SqlQuery("select * from dbo.Stock s, dbo.Item i where i.ID = s.ItemID and s.Qty<i.AlertQty and s.Qty>0").ToList();
                                     



                       //var list = dc.Orders.
                       //             Join(dc.Order_Details,
                       //             o => o.OrderID, od => od.OrderID,
                       //             (o, od) => new
                       //             {
                       //                 OrderID = o.OrderID,
                       //                 OrderDate = o.OrderDate,
                       //                 ShipName = o.ShipName,
                       //                 Quantity = od.Quantity,
                       //                 UnitPrice = od.UnitPrice,
                       //                 ProductID = od.ProductID
                       //             }).Join(dc.Products,
                       //                     a => a.ProductID, p => p.ProductID,
                       //                     (a, p) => new
                       //                     {
                       //                         OrderID = a.OrderID,
                       //                         OrderDate = a.OrderDate,
                       //                         ShipName = a.ShipName,
                       //                         Quantity = a.Quantity,
                       //                         UnitPrice = a.UnitPrice,
                       //                         ProductName = p.ProductName
                       //                     });

                        // var list = db.Stocks.SqlQuery("select * from dbo.LocationStock s,  dbo.Item i where i.ID = s.ItemID and s.Qty<i.AlertQty and s.Qty>0").ToList();
                        return Json(list.Count(), JsonRequestBehavior.AllowGet);

                    }
                }
               

                
            }
        }

        /// <summary>
        /// Counts the total stocks that has been ecpired
        /// </summary>
        /// <returns>json result for total expiry data</returns>
        public ActionResult CheckExpiry()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var list = db.Stocks.SqlQuery("select * from Stock s where s.ExpiryDate - GETDATE() < 75").ToList();
                return Json(list.Count(), JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Shows notification page
        /// </summary>
        /// <returns></returns>
        public ActionResult Notifications() {
            return View();
        }
        
        /// <summary>
        /// Displays Partial view for Low Stocks 
        /// </summary>
        /// <returns>Partial View</returns>
        public PartialViewResult LowStock()
        {          
            List<Stock> list = (from s in db.Stocks
                                where s.Qty < s.Item.AlertQty && s.Qty > 0
                                select s).ToList();
            return PartialView(list);           
        }

        /// <summary>
        /// Displays Partial view for Stock expiry
        /// </summary>
        /// <returns></returns>
        public PartialViewResult  StockExpiry()
        {  
            List<Stock> list = db.Stocks.SqlQuery("select * from Stock s where s.ExpiryDate - GETDATE() < 75").ToList();
            return PartialView(list);
        }

        /// <summary>
        /// Deletes selected expired product
        /// </summary>
        /// <param name="stocksID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSelectedExpiry(IEnumerable<int> stocksID)
        {
            foreach(var id in stocksID)
            {
                var _stock = db.Stocks.Single(s => s.ID == id);
                db.Stocks.Remove(_stock);
            }
            db.SaveChanges();
            return RedirectToAction("Notifications");
        }
        
        /// <summary>
        /// Generates the expiry return list
        /// </summary>
        /// <param name="stocksID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateExpiryReturns(IEnumerable<int> stocksID)
        {
            List<ExpiryReturnsViewModel> returnList = new List<ExpiryReturnsViewModel>();
            foreach (var id in stocksID)
            {
                Purchase p = new Purchase();

                var stk = (from s in db.Stocks
                            where s.ID == id
                            select new { s.Item.Name, s.BatchNo, s.Qty, s.Item.ID }).SingleOrDefault();

                //get purchaseID using StockID
                var purchaseID = (from s in db.Stocks
                                 where s.ID == id
                                 select s.PurchaseID).SingleOrDefault();

                //Query Purchase Infos using purchaseID
                p = db.Purchases.SqlQuery("Select * from dbo.Purchase where ID='"+purchaseID+"'").SingleOrDefault();
                               
                ExpiryReturnsViewModel vm = new ExpiryReturnsViewModel() { 
                    StockID = id,
                    ItemName = stk.Name,
                    Batch = stk.BatchNo,
                    PurchasedDate = p.Date,
                    QtyExpired = stk.Qty,
                    SupplierName = p.Supplier.SupplierName,
                };
                returnList.Add(vm);
            }
            return View(returnList);
        }

        /// <summary>
        /// Restocks expired items with new attributes. Wont have this time. In future
        /// </summary>
        /// <param name="_Stock"></param>
        /// <returns></returns>
        public ActionResult RestockExpiry([Bind(Include = "ID, ExpiryDate, CostPrice, SellingPrice, BatchNo, Qty")] Stock _Stock)
        {
            Stock stk = (from s in db.Stocks
                      where s.ID == _Stock.ID
                      select s).Single();
            //update with the new inputs                               
            stk.ExpiryDate = _Stock.ExpiryDate;
            stk.Qty = _Stock.Qty;
            stk.CostPrice = _Stock.CostPrice;
            stk.SellingPrice = _Stock.SellingPrice;
            stk.BatchNo = _Stock.BatchNo;
            db.SaveChanges();
            
            return RedirectToAction("Notifications");
        }
        
        /// <summary>
        /// Generates purchase order. Yet to be done.
        /// </summary>
        /// <returns></returns>
        public ActionResult GeneratePurchaseOrder()
        {
            List<Stock> list = (from s in db.Stocks
                                where s.Qty < s.Item.AlertQty && s.Qty > 0
                                select s).ToList();
            return View(list);
        }
        
        //*******************************************

        //****************Charts ********************

        /// <summary>
        /// Returns Json data for yearly sales.
        /// </summary>
        /// <returns></returns>
        public JsonResult ForMorris()
        {
            string year = Convert.ToString(DateTime.Today.Year);

            ReportsController rc = new ReportsController();
            var model = rc.YearlySalesByMonth_forCharts(year).ToList();
            
            var count = model.Select(i => new Object[] {i.GrandTotal}).ToList();
            return Json(count, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Json data for Monthly sales for this month</returns>
        public JsonResult ForMorris2()
        {
            ReportsController rc = new ReportsController();
            var model = rc.MonthlySalesByDate_forCharts(DateTime.Today.Year, DateTime.Today.Month);

            var value = model.Select(i => new Object[] { i.Day.ToString(), i.Total }).ToArray();
            return Json(value, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Displays Partial View for Yearly Sales
        /// </summary>
        /// <returns></returns>
        public PartialViewResult MyChart()
        {
            string year = Convert.ToString(DateTime.Today.Year);

            ReportsController rc = new ReportsController();
            var model = rc.YearlySalesByMonth_forCharts(year).ToList();

            var name = model.Select(i => i.Month.ToString()).ToArray();

            var count = model.Select(i => new Object[] { i.GrandTotal }).ToArray();

            String[] arr = {"Jan"," Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
            var charts = new Highcharts("charts").InitChart(new Chart { DefaultSeriesType = ChartTypes.Line })
                .SetTitle(new Title { Text = "Graph of Yearly sales" })
                .SetSubtitle(new Subtitle { Text = "For year 2015" })
                // .SetXAxis(new XAxis { Categories = name, Title = new XAxisTitle{Text = "Months"} })      
                .SetXAxis(new XAxis { Categories = arr, Title = new XAxisTitle { Text = "Months"} })
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Total Sales/Month" } })
                .SetPlotOptions(new PlotOptions
                {
                    Line = new PlotOptionsLine
                    {
                        DataLabels = new PlotOptionsLineDataLabels
                        {
                            Enabled = true
                        },
                        EnableMouseTracking = false
                    }
                })
                .SetSeries(new[]
                {
                    new Series{Name= "Total sales", Data = new Data(count)}
                });
            
            return PartialView("MyChartPartialView",charts);
        }

        /// <summary>
        /// Displays Prtial View for Monthly sales
        /// </summary>
        /// <returns></returns>
        public ActionResult MyChartForSalesOfMonth()
        {
            //string year = Convert.ToString(DateTime.Today.Year);

            ReportsController rc = new ReportsController();
            var model = rc.MonthlySalesByDate_forCharts(DateTime.Today.Year, DateTime.Today.Month);

            var name = model.Select(i => i.Day.ToString()).ToArray();
            var count = model.Select(i => new Object[] { i.Total }).ToArray();

            var charts2 = new Highcharts("charts").InitChart(new Chart { DefaultSeriesType = ChartTypes.Column })
                .SetTitle(new Title { Text = "Graph of Sales in a month" })
                .SetSubtitle(new Subtitle { Text = "For year: " + DateTime.Today.Year + ", month: "+DateTime.Today.Month })
                .SetXAxis(new XAxis { Categories = name, Title = new XAxisTitle { Text = "Months" } })
                .SetYAxis(new YAxis { Title = new YAxisTitle { Text = "Total Sales per Day" } })
                .SetPlotOptions(new PlotOptions
                {
                    Line = new PlotOptionsLine
                    {
                        DataLabels = new PlotOptionsLineDataLabels
                        {
                            Enabled = true
                        },
                        EnableMouseTracking = false
                    }
                })
                .SetSeries(new[]
                {
                    new Series{Name= "Total sales", Data = new Data(count)}
                });

            return PartialView("MyChartForSalesOfMonthPartialView", charts2);
        }


        /// <summary>
        /// Calculates teh TotalSales Today .
        /// </summary>
        /// <returns>returns total sales made today.</returns>
        public decimal TodaySales() //Today's Sales - Product Wise
        {
            string LocalDT = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            DateTime _DateTime = Convert.ToDateTime(LocalDT);
            decimal sales = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, _DateTime) == 0).Sum(x => (decimal?)(x.GrandTotal)) ?? 0;
            
            return sales;
        }
        public decimal TodaySalesSubTot() //Today's Sales - Product Wise
        {
            string LocalDT = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            DateTime _DateTime = Convert.ToDateTime(LocalDT);
            decimal sales = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, _DateTime) == 0).Sum(x => (decimal?)(x.Amount)) ?? 0;

            return sales;
        }
        public decimal TodaySalesDis() //Today's Sales - Product Wise
        {
            string LocalDT = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            DateTime _DateTime = Convert.ToDateTime(LocalDT);
            decimal sales = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, _DateTime) == 0).Sum(x => (decimal?)(x.Discount)) ?? 0;

            return sales;
        }
        public decimal TodaySalesVat() //Today's Sales - Product Wise
        {
            string LocalDT = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            DateTime _DateTime = Convert.ToDateTime(LocalDT);
            decimal sales = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, _DateTime) == 0).Sum(x => (decimal?)(x.Tax)) ?? 0;

            return sales;
        }

        public List<SalesCountVM> TodaySalesCount() //Today's Sales - Product Wise
        {
            string LocalDT = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            DateTime _DateTime = Convert.ToDateTime(LocalDT);
            List<SalesCountVM> list = db.SalesItems
               .Where(x => DbFunctions.DiffDays(x.Sales.Date, _DateTime) == 0)
                .GroupBy(s => s.LocationStock.Item.Name)
                .Select(g =>
                    new SalesCountVM
                    {
                        ItemName = g.Key,
                        Description = g.Key,
                        Count = g.Count(),
                        Qty = g.Sum(p => p.Qty),
                        Amount = g.Sum(p => p.Amount),

                    }).ToList();


            return list;

        }





        /// <summary>
        /// Calculates the Total Sales on Yesterday.
        /// </summary>
        /// <returns>returns total yesterday's sales</returns>
        public decimal YesterdaySales()
        {
            string LocalDT = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            DateTime _DateTime = Convert.ToDateTime(LocalDT);
            decimal sales = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, _DateTime) == 1).Sum(x => (decimal?) (x.GrandTotal))??0;
            return sales;
        }
        public decimal YesterdaySalesSubTot() //Yesterday's Sales - Product Wise
        {
            string LocalDT = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            DateTime _DateTime = Convert.ToDateTime(LocalDT);
            decimal sales = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, _DateTime) == 1).Sum(x => (decimal?)(x.Amount)) ?? 0;

            return sales;
        }
        public decimal YesterdaySalesDis() //Yesterday's Sales - Product Wise
        {
            string LocalDT = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            DateTime _DateTime = Convert.ToDateTime(LocalDT);
            decimal sales = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, _DateTime) == 1).Sum(x => (decimal?)(x.Discount)) ?? 0;

            return sales;
        }
        public decimal YesterdaySalesVat() //Yesterday's Sales - Product Wise
        {
            string LocalDT = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            DateTime _DateTime = Convert.ToDateTime(LocalDT);
            decimal sales = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, _DateTime) == 1).Sum(x => (decimal?)(x.Tax)) ?? 0;

            return sales;
        }
        public List<SalesCountVM> YesterdaySalesCount() //Yesterday's Sales - Product Wise
        {


            string LocalDT = Convert.ToDateTime(DateTime.Now, theCultureInfo).ToString("yyyy-MM-dd hh:mm tt");
            DateTime _DateTime = Convert.ToDateTime(LocalDT);
            List<SalesCountVM> list = db.SalesItems
               .Where(x => DbFunctions.DiffDays(x.Sales.Date, _DateTime) == 1)
                .GroupBy(s => s.LocationStock.Item.Name)
                .Select(g =>
                    new SalesCountVM
                    {
                        ItemName = g.Key,
                        Description = g.Key,
                        Count = g.Count(),
                        Qty = g.Sum(p => p.Qty),
                        Amount = g.Sum(p => p.Amount),

                    }).ToList();


            return list;

        }



        /// <summary>
        /// Disposes any unclosed or opened db connection.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
