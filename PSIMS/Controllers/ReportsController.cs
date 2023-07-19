using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSIMS.Models.InventoryModel;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using Microsoft.Reporting.WebForms;
using System.IO;
using PSIMS.ViewModel;
using PSIMS.Repository;
using System.Data.Entity;
using PSIMS.ViewModel.ForReports;
using PSIMS.Repository.Reports;
using PSIMS.Models.SalesModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using System.Net;
using PSIMS.Models.Finance;
using PSIMS.Models.Locations;

namespace PSIMS.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private UserManager<ApplicationUser> manager = null;
        private RoleManager<IdentityRole> roleManager = null;

        public ReportsController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        }
        // GET: Reports
        public ActionResult Index()
        {
            return View(db.Stocks.ToList());
        }

        //********************** for low stock items : Default **************************
        public ActionResult Stocks()
        {
            // return View();           
            return View(db.Stocks.ToList());
        }

        [HttpPost]
        public ActionResult Stocks(StockSearchVM vm)
        {
            var filter = new StocksFilterRepository();
            var model = filter.FilterStocks(vm);
            return View(model);
        }

        public ActionResult TestStocks(StockSearchVM vm)
        {
            var filter = new StocksFilterRepository();
            var model = filter.FilterStocks(vm);
            return PartialView("_StocksPartialView", model);
        }
        //************************* End ***********************************
        //*****************************Purchase****************************************
        /// <summary>
        /// Purchase Report
        /// </summary>
        /// <returns></returns>
        //public ActionResult Purchase()
        //{

        //    return View(db.Purchases.ToList());
        //}
        //[HttpPost]
        //public ActionResult Purchase(PurchaseSearchVM vm)
        //{
        //    var filter = new PurchaseFilterRepository();
        //    var model = filter.FilterPurchase(vm);
        //    return View(model);
        //}


        //public ActionResult PurchaseFilter(PurchaseSearchVM vm)
        //{
        //    var filter = new PurchaseFilterRepository();
        //    var model = filter.FilterPurchase(vm);
        //    return PartialView("_PurchasePartialView", model);
        //}


        //*********************************End************************************   
        public ActionResult DailySales()
        {

            var list = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, DateTime.Now) == 0).ToList();
            return View(list);
        }
        public ActionResult DailySalesFor(DateTime getDate)
        {
            var list = db.Sales.Where(x => DbFunctions.DiffDays(x.Date, getDate) == 0).ToList();
            return PartialView("_DailySalesPartialView", list);
        }

        //Staffs
        public ActionResult EmployeesReports()
        {
            return null;
        }
        /*
         //Monthly Sales
         public ActionResult MonthlySalesByDate()
         {

             int year = 2014;
             int month = 12;
             var query = db.Sales.Where(x => x.Date.Year == year && x.Date.Month == month).GroupBy(x => x.Date).Select(g => new DayTotalVM
             {
                 Day = g.Key.Day,
                 Total = g.Sum(x => x.GrandTotal)
             });

             int daysInMonth = DateTime.DaysInMonth(year, month);
             List<DayTotalVM> days = new List<DayTotalVM>();
             for (int i = 1; i < daysInMonth + 1; i++ )
             {
                 DayTotalVM item = new DayTotalVM() { Day = i};
                 DayTotalVM ex = query.Where(x => x.Day == i).FirstOrDefault();
                 if(ex != null)
                 {
                     item.Total = ex.Total;
                 }
                 days.Add(item);
             }

             SalesVM model = new SalesVM() 
             { 
                 Date = new DateTime(year, month,1),
                 Days = days
             };

             return View(model);
         }
 */

        /// <summary>
        /// Display monthly Sales for each day in a month
        /// </summary>
        /// <returns></returns>
        public ActionResult MonthlySalesByDate()
        {
            int year = DateTime.Now.Year;
            int month = 12;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var days = Enumerable.Range(1, daysInMonth);
            var query = db.Sales.Where(x => x.Date.Year == year && x.Date.Month == month).OrderBy(x => x.Date).Select(g => new
            {
                Day = g.Date.Day,
                Total = g.GrandTotal
            });
            var model = new SalesDetails
            {
                Date = new DateTime(year, month, 1),
                Days = days.GroupJoin(query, d => d, q => q.Day, (d, q) => new DayTotalVM
                {
                    Day = d,
                    Total = q.Sum(x => x.Total)
                }).ToList()
            };
            return View(model);
        }



        //Returns Monthly sales based on a parameter
        [HttpPost]
        public ActionResult MonthlySalesByDate(string _year, string _month)
        {
            //assign incoming values to the variables
            int year = 0, month = 0;
            //check if year is null
            if (string.IsNullOrWhiteSpace(_year) && _month != null)
            {
                year = DateTime.Now.Date.Year;
                month = Convert.ToInt32(_month.Trim());
            }
            else
            {
                year = Convert.ToInt32(_year.Trim());
                month = Convert.ToInt32(_month.Trim());
            }
            //calculate ttal number of days in a particular month for a that year 
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var days = Enumerable.Range(1, daysInMonth);
            var query = db.Sales.Where(x => x.Date.Year == year && x.Date.Month == month).OrderBy(x => x.Date.Day).Select(g => new
            {
                Day = g.Date.Day,
                Total = g.GrandTotal
            });
            var model = new SalesDetails
            {
                Date = new DateTime(year, month, 1),
                Days = days.GroupJoin(query, d => d, q => q.Day, (d, q) => new DayTotalVM
                {
                    Day = d,
                    Total = q.Sum(x => x.Total)
                }).ToList()
            };
            return View(model);
        }




        public List<DayTotalVM> MonthlySalesByDate_forCharts(int yr, int mnt)
        {
            int year = yr;
            int month = mnt;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            var days = Enumerable.Range(1, daysInMonth);
            var query = db.Sales.Where(x => x.Date.Year == year && x.Date.Month == month).Select(g => new
            {
                Day = g.Date.Day,
                Total = g.GrandTotal
            });
            SalesDetails model = new SalesDetails
            {
                Date = new DateTime(year, month, 1),
                Days = days.GroupJoin(query, d => d, q => q.Day, (d, q) => new DayTotalVM
                {
                    Day = d,
                    Total = q.Sum(x => x.Total)
                }).ToList()
            };
            return model.Days.ToList();
        }
        //********************************************************************
        /// <summary>
        /// Yearly Sales
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public ActionResult YearlySalesByMonth(string year)
        {
            int _year = 0;
            int _toYear = 0;
            if (string.IsNullOrWhiteSpace(year))
            {
                _year = DateTime.Now.Year;
                _toYear = _year + 1;
            }
            else
            {
                _year = Convert.ToInt32(year);
                _toYear = _year + 1;
            }

            //    int _year = DateTime.Now.Year;
            //    int _toYear = _year + 1;
            var query = db.Sales.Where(s => (s.Date.Year >= _year && s.Date.Year < _toYear));
            // YearlyReportVM _Model = new YearlyReportVM();
            List<MonthTotalVM> _Model = new List<MonthTotalVM>();

            for (int i = 1; i < 13; i++)
            {
                decimal _grandTotal = 0;
                decimal temp = 0;
                temp = query.Where(x => x.Date.Month == i).Sum(x => (decimal?)(x.GrandTotal)) ?? 0;

                _grandTotal = temp;

                MonthTotalVM model = new MonthTotalVM()
                {
                    Year = _year,
                    Month = i,
                    GrandTotal = _grandTotal
                };
                _Model.Add(model);
            }

            return View(_Model.ToList());
        }



        public List<MonthTotalVM> YearlySalesByMonth_forCharts(string year)
        {
            int _year = 0;
            int _toYear = 0;
            if (string.IsNullOrWhiteSpace(year))
            {
                _year = DateTime.Now.Year;
                _toYear = _year + 1;
            }
            else
            {
                _year = Convert.ToInt32(year);
                _toYear = _year + 1;
            }

            var query = db.Sales.Where(s => (s.Date.Year >= _year && s.Date.Year < _toYear));
            List<MonthTotalVM> _Model = new List<MonthTotalVM>();

            for (int i = 1; i < 13; i++)
            {
                decimal _grandTotal = 0;
                decimal temp = 0;
                temp = query.Where(x => x.Date.Month == i).Sum(x => (decimal?)(x.GrandTotal)) ?? 0;

                _grandTotal = temp;

                MonthTotalVM model = new MonthTotalVM()
                {
                    Year = _year,
                    Month = i,
                    GrandTotal = _grandTotal
                };
                _Model.Add(model);
            }
            return _Model.ToList();
        }

        //*********************************************************************
        /// <summary>
        /// Payments Report
        /// </summary>
        /// <returns></returns>
        public ActionResult ReceiptPaymentsReport()
        {
            //int year = 0, month = 0;

            var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();

            //var payment = db.PaymentSettelmentMasters.Include(p => p.paymentSettelmentDetails).Include(c => c.Customer).Include(i => i.Bank);

            List<Customer> cutomers = db.Customers.ToList();
            List<Bank> banks = db.Banks.ToList();
            List<Location> locations = db.Locations.ToList();
            List<PaymentSettelmentMaster> paymentmasters = db.PaymentSettelmentMasters.ToList();
            List<PaymentSettelmentDetails> paymentdetailss = db.PaymentSettelmentDetails.ToList();



            var payment = from pm in paymentmasters
                          join pd in paymentdetailss on pm.ID equals pd.PaymentSettelmentMasterID into table1
                          from pd in table1.ToList()
                          join bn in banks on pm.BankID equals bn.ID into table2
                          from bn in table2.ToList()
                          join cu in cutomers on pm.CustomerID equals cu.ID into table3
                          from cu in table3.ToList()
                          join lo in locations on pm.LocationID equals lo.ID into table4
                          from lo in table4.ToList()
                          select new PaymentSettlmentViewModel
                          {
                              paymentmaster = pm,
                              paymentdetails = pd,
                              customer = cu,
                              bank = bn,
                              location = lo

                          };
            
        
            string roleName = (roleManager.Roles.FirstOrDefault(x => x.Id == role.RoleId)).Name;
            if (roleName.Equals("Super Admin") || roleName.Equals("Admin") || roleName.Equals("Chairman") || roleName.Equals("CEO") || roleName.Equals("GM")
                || roleName.Equals("DGM") || roleName.Equals("AGM") || roleName.Equals("Auditor"))
            {
                return View(payment.Take(100));
                //return View(payment);
            }
            else
            {
                int UserLocationID = Convert.ToInt32(Session["LocationID"]);
                //var payments = payment.Where(x => x.LocationID == UserLocationID);
                var payments = from pm in paymentmasters
                              join pd in paymentdetailss on pm.ID equals pd.PaymentSettelmentMasterID into table1
                              from pd in table1.ToList()
                              join bn in banks on pm.BankID equals bn.ID into table2
                              from bn in table2.ToList()
                              join cu in cutomers on pm.CustomerID equals cu.ID into table3
                              from cu in table3.ToList()
                              join lo in locations on pm.LocationID equals lo.ID into table4
                              from lo in table4.ToList()
                              select new PaymentSettlmentViewModel
                              {
                                  paymentmaster = pm,
                                  paymentdetails = pd,
                                  customer = cu,
                                  bank = bn,
                                  location = lo

                              };
                return View(payments.Take(100));

            }


            //return View(db.Payments.ToList());
        }


        [HttpPost]
        public ActionResult ReceiptPaymentsReport(PaymentSerachVM PSVM)
        {
            var filter = new PaymentFilterRepository();
            var model = filter.FilterPayments(PSVM);

            return View(model);
        }



        public ActionResult SalesDetails()
        {
            var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();

            var SalesItem = db.SalesItems;
            string roleName = (roleManager.Roles.FirstOrDefault(x => x.Id == role.RoleId)).Name;
            if (roleName.Equals("Super Admin") || roleName.Equals("Admin") || roleName.Equals("Chairman") || roleName.Equals("CEO") || roleName.Equals("GM")
                || roleName.Equals("DGM") || roleName.Equals("AGM") || roleName.Equals("Auditor"))
            {
                return View(SalesItem.ToList());
            }
            else
            {
                int UserLocationID = Convert.ToInt32(Session["LocationID"]);
                var _Sales = SalesItem.Include(s => s.Sales)
                            .Where(s => s.Sales.LocationID == UserLocationID);
                return View(_Sales.ToList());
            }


        }
        [HttpPost]
        public ActionResult SalesDetails(SalesSearchVM svm)
        {
            var salesfilter = new SalesFilterRepository();
            var model = salesfilter.FilterSales(svm);
            return View(model);
        }

        public ActionResult CustomerSales()
        {
            int _year = DateTime.Now.Year;
            int _month = DateTime.Now.Month;

            //var customerSales = db.Sales.Include("Customer").Include("invoiceOptionEntry").AsQueryable();
            var customerSales = (from s in db.Sales
                                 where s.IsActive == 1
                                 where s.Date.Year == _year && s.Date.Month == _month
                                 where (s.IsPayment == false && s.PaymentType == "0" && s.unitbalance == 0 && s.LastReceiptAmt == 0) 
                                 //|| (s.PaymentType == "PP" && s.unitbalance != 0)
                                 select s);

            return View(customerSales.ToList());
            //var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();

            //var customerSales = db.Sales.OrderByDescending(s=>s.ID).Take(100);
            //string roleName = (roleManager.Roles.FirstOrDefault(x => x.Id == role.RoleId)).Name;
            //if (roleName.Equals("Super Admin") || roleName.Equals("Admin") || roleName.Equals("Chairman") || roleName.Equals("CEO") || roleName.Equals("GM")
            //    || roleName.Equals("DGM") || roleName.Equals("AGM") || roleName.Equals("Auditor"))
            //{
            //    return View(customerSales.ToList());
            //}
            //else
            //{
            //    int UserLocationID = Convert.ToInt32(Session["LocationID"]);
            //    var Purchase = customerSales.Where(x => x.LocationID == UserLocationID);
            //    return View(Purchase.ToList());
            // }



        }

        public ActionResult CustomerSalesItem_Report(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Report_salesItems = (from si in db.SalesItems
                                     where si.SalesID == id
                                     select si).ToList();

            if (Report_salesItems == null)
            {
                return HttpNotFound();
            }
            return View(Report_salesItems.ToList());
        }

        [HttpPost]
        public ActionResult CustomerSales(SalesSearchVM PSVM)
        {
            var filter = new SalesFilterRepository();
            var model = filter.FiltercustomerSales(PSVM);

            return View(model);
        }


        //*********************************************************************

        //*********************************************************************
        /// <summary>
        /// Purchase Report
        /// </summary>
        /// <returns></returns>
        public ActionResult Purchase()
        {
            var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();

            var Purchases = db.Purchases;
            string roleName = (roleManager.Roles.FirstOrDefault(x => x.Id == role.RoleId)).Name;
            if (roleName.Equals("Super Admin") || roleName.Equals("Admin") || roleName.Equals("Chairman") || roleName.Equals("CEO") || roleName.Equals("GM")
                || roleName.Equals("DGM") || roleName.Equals("AGM") || roleName.Equals("Auditor"))
            {
                return View(Purchases.ToList());
            }
            else
            {
                int UserLocationID = Convert.ToInt32(Session["LocationID"]);
                var Purchase = Purchases.Where(x => x.LocationID == UserLocationID);
                return View(Purchase.ToList());
            }

            // return View(db.Purchases.ToList());
        }



        [HttpPost]
        public ActionResult Purchase(PurchaseSearchVM vm)
        {
            var filter = new PurchaseFilterRepository();
            var model = filter.FilterPurchase(vm);
            return View(model);
        }

        //*********************************************************************   

        public ActionResult ProfitAndLoss()
        {
            return View();
        }
        [HttpPost]
        public ActionResult productwisesales(ProductWiseSalesVM pws)
        {
            var filter = new ProductWiseSalesFilterRepository();
            var model = filter.Filterproduct(pws);
            ViewBag.todayProductCount = model;
            return View();
        }
        public ActionResult productwisesales()
        {
            ViewBag.todayProductCount = TodaySalesCount(); //Today's Sales - Product Wise

            return View();

        }
        public List<SalesCountVM> TodaySalesCount() //Today's Sales - Product Wise
        {

            List<SalesCountVM> list = db.SalesItems
                //.Where(x => DbFunctions.DiffDays(x.Sales.Date, DateTime.Now) == 0)
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


        public JsonResult PopulateCustomer()
        {
            //holds list of suppliers
            List<Customer> _customerList = new List<Customer>();

            //queries all the suppliers for its ID and Name property.
            var customerList = (from s in db.Customers
                                select new { s.ID, s.CustomerName }).ToList();

            //save list of suppliers to the _supplierList
            foreach (var item in customerList)
            {
                _customerList.Add(new Customer
                {
                    ID = item.ID,
                    CustomerName = item.CustomerName
                });
            }
            //returns the Json result of _supplierList
            return Json(_customerList, JsonRequestBehavior.AllowGet);
        }


        public ActionResult StockTransferNote()
        {
            return View(db.StockDistributions.ToList());
            //var filter = new StocksFilterRepository();
            //var model = filter.FilterStocks(vm);
            //return View(model);
        }

    }
}