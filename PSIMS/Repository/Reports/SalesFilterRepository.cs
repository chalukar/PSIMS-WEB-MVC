using IdentitySample.Models;
using PSIMS.Models;
using PSIMS.Models.SalesModel;
using PSIMS.ViewModel;
using PSIMS.ViewModel.ForReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PSIMS.Repository
{
    public class SalesFilterRepository
    {
        private ApplicationDbContext db;
        public SalesFilterRepository()
        {
            db = new ApplicationDbContext();
        }



        public IQueryable<SalesItem> FilterSales(SalesSearchVM svm)
        {
            var _result = db.SalesItems.Include("Sales").Include("LocationStock").AsQueryable();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (svm != null)
            {
                if (!string.IsNullOrEmpty(svm.option))
                {
                    if (svm.option == "today")
                    {
                        _result = from p in _result
                                  where p.Date.Day == DateTime.Today.Day
                                  where p.Date.Month == DateTime.Today.Month
                                  where p.Date.Year == DateTime.Today.Year
                                  select p;


                    }
                    else if (svm.option == "yesterday")
                    {
                        _result = from p in _result
                                  where p.Date.Day == DateTime.Today.Day - 1
                                  where p.Date.Month == DateTime.Today.Month
                                  where p.Date.Year == DateTime.Today.Year
                                  select p;
                        //_result = _result.Where(p => p.Date.Date == DateTime.Now.Date.AddDays(-1));
                    }
                    else if (svm.option == "thisWeek")
                    {
                        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                        _result = _result.Where(x => x.Date >= startDayOfWeek && x.Date <= endDayOfWeek);
                        // _result = _result.Where(p=> p.Date.);
                    }
                    else if (svm.option == "thisMonth")
                    {

                        _result = _result.Where(p => p.Date.Month == month);
                    }
                    else if (svm.option == "lastMonth")
                    {
                        _result = _result.Where(p => p.Date.Month == month - 1);
                    }
                    else if (svm.option == "thisYear")
                    {
                        _result = _result.Where(p => p.Date.Year == year);

                    }
                    else if (svm.option == "lastYear")
                    {
                        _result = _result.Where(p => p.Date.Year == year - 1);
                    }
                }

                if (!string.IsNullOrEmpty(svm.customer))
                {
                    var value = Convert.ToInt32(svm.customer);
                    //query here
                    _result = _result.Where(p => p.Sales.CustomerID == value);
                }

                if (svm.fromDate != null || svm.toDate != null)
                {
                    if (svm.fromDate != null && svm.toDate == null)
                    {
                        //query here
                        _result = _result.Where(p => p.Date >= svm.fromDate);
                    }
                    else if (svm.fromDate == null && svm.toDate != null)
                    {
                        //query here
                        _result = _result.Where(p => p.Date <= svm.fromDate);
                    }
                    else if (svm.fromDate != null && svm.toDate != null)
                    {
                        //query here
                        _result = _result.Where(p => p.Date >= svm.fromDate && p.Date <= svm.toDate);
                    }

                }

                if (svm.IsActive != null)
                {
                    //query here
                    if (svm.IsActive == 1)
                    {
                        _result = _result.Where(p => p.IsActive == 1);
                    }
                    else
                    {
                        _result = _result.Where(p => p.IsActive == 2);
                    }

                }


            }
            return _result;
        }

        public IQueryable<Sales> FiltercustomerSales(SalesSearchVM PSVM)
        {
            //var _result = db.Sales.Include("Customer").Include("invoiceOptionEntry").AsQueryable();
            var _result = (from s in db.Sales.Include(c => c.Customer).Include(i => i.invoiceOptionEntry)
                           where s.IsActive == 1
                           //where (s.PaymentType == "0" && s.unitbalance == 0 && s.LastReceiptAmt == 0) || (s.PaymentType == "PP" && s.unitbalance != 0)
                           select s).AsQueryable();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (PSVM != null)
            {
                if (!string.IsNullOrEmpty(PSVM.option))
                {
                    if (PSVM.PaymentType == "0")
                    {
                        _result = from p in _result
                                  where p.PaymentType == "0"
                                  select p;
                    }
                    else if (PSVM.PaymentType == "PP")
                    {
                        _result = from p in _result
                                  where p.PaymentType == "PP"
                                  select p;
                    }
                    else if (PSVM.PaymentType == "FP")
                    {
                        _result = from p in _result
                                  where p.PaymentType == "FP"
                                  select p;
                    }
                    else if (PSVM.PaymentType == "BP")
                    {
                        _result = from p in _result
                                  where p.PaymentType == "BP"
                                  select p;
                    }

                }
                if(PSVM.invoiceNo !=0)
                {
                    var value = PSVM.invoiceNo;

                    _result = from p in _result
                              where p.ID == value
                              select p;
                }

                if (!string.IsNullOrEmpty(PSVM.option))
                {
                    if (PSVM.option == "today")
                    {
                        _result = from p in _result
                                  where p.Date.Day == DateTime.Today.Day
                                  where p.Date.Month == DateTime.Today.Month
                                  where p.Date.Year == DateTime.Today.Year
                                  select p;


                    }
                    else if (PSVM.option == "yesterday")
                    {
                        _result = from p in _result
                                  where p.Date.Day == DateTime.Today.Day - 1
                                  where p.Date.Month == DateTime.Today.Month
                                  where p.Date.Year == DateTime.Today.Year
                                  select p;
                        //_result = _result.Where(p => p.Date.Date == DateTime.Now.Date.AddDays(-1));
                    }
                    else if (PSVM.option == "thisWeek")
                    {
                        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                        _result = _result.Where(x => x.Date >= startDayOfWeek && x.Date <= endDayOfWeek);
                        // _result = _result.Where(p=> p.Date.);
                    }
                    else if (PSVM.option == "thisMonth")
                    {

                        _result = _result.Where(p => p.Date.Month == month);
                    }
                    else if (PSVM.option == "lastMonth")
                    {
                        _result = _result.Where(p => p.Date.Month == month - 1);
                    }
                    else if (PSVM.option == "thisYear")
                    {
                        _result = _result.Where(p => p.Date.Year == year);

                    }
                    else if (PSVM.option == "lastYear")
                    {
                        _result = _result.Where(p => p.Date.Year == year - 1);
                    }
                }

                //if (!string.IsNullOrEmpty(PSVM.Company))
                if (!string.IsNullOrEmpty(PSVM.customer))
                {
                    var value = Convert.ToInt32(PSVM.customer);
                    //query here
                    _result = _result.Where(p => p.CustomerID == value);
                }


                if (PSVM.fromDate != null || PSVM.toDate != null)
                {
                    if (PSVM.fromDate != null && PSVM.toDate == null)
                    {
                        //query here
                        _result = _result.Where(p => p.Date >= PSVM.fromDate);
                    }
                    else if (PSVM.fromDate == null && PSVM.toDate != null)
                    {
                        //query here
                        _result = _result.Where(p => p.Date <= PSVM.fromDate);
                    }
                    else if (PSVM.fromDate != null && PSVM.toDate != null)
                    {
                        //query here
                        _result = _result.Where(p => p.Date >= PSVM.fromDate && p.Date <= PSVM.toDate);
                    }

                }
                if (PSVM.IsActive != null)
                {
                    //query here
                    if (PSVM.IsActive == 1) // active
                    {
                        _result = _result.Where(p => p.IsActive == 1);
                    }
                    else if (PSVM.IsActive == 2) // cancelled
                    {
                        _result = _result.Where(p => p.IsActive == 0);
                    }

                }

            }

            return _result;
        }

        public IQueryable<Sales> FilterSalesHistory(SalesSearchVM PSVM)
        {
            var _result = (from s in db.Sales.Include(c => c.Customer).Include(i => i.invoiceOptionEntry)
                               //where s.IsActive == 1
                           select s).AsQueryable();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (PSVM != null)
            {
                if(PSVM.salesID != null || PSVM.salesID != 0)
                {
                    var sals_ID = PSVM.salesID;
                    _result = _result.Where(p => p.ID == sals_ID);
                }
                //if (!string.IsNullOrEmpty(PSVM.option))
                //{
                //    if (PSVM.option == "today")
                //    {
                //        _result = from p in _result
                //                  where p.Date.Day == DateTime.Today.Day
                //                  where p.Date.Month == DateTime.Today.Month
                //                  where p.Date.Year == DateTime.Today.Year
                //                  select p;


                //    }
                //    else if (PSVM.option == "yesterday")
                //    {
                //        _result = from p in _result
                //                  where p.Date.Day == DateTime.Today.Day - 1
                //                  where p.Date.Month == DateTime.Today.Month
                //                  where p.Date.Year == DateTime.Today.Year
                //                  select p;
                //        //_result = _result.Where(p => p.Date.Date == DateTime.Now.Date.AddDays(-1));
                //    }
                //    else if (PSVM.option == "thisWeek")
                //    {
                //        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                //        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                //        _result = _result.Where(x => x.Date >= startDayOfWeek && x.Date <= endDayOfWeek);
                //        // _result = _result.Where(p=> p.Date.);
                //    }
                //    else if (PSVM.option == "thisMonth")
                //    {

                //        _result = _result.Where(p => p.Date.Month == month);
                //    }
                //    else if (PSVM.option == "lastMonth")
                //    {
                //        _result = _result.Where(p => p.Date.Month == month - 1);
                //    }
                //    else if (PSVM.option == "thisYear")
                //    {
                //        _result = _result.Where(p => p.Date.Year == year);

                //    }
                //    else if (PSVM.option == "lastYear")
                //    {
                //        _result = _result.Where(p => p.Date.Year == year - 1);
                //    }
                //}

                if (!string.IsNullOrEmpty(PSVM.customer))
                {
                    var value = Convert.ToInt32(PSVM.customer);
                    //query here
                    _result = _result.Where(p => p.CustomerID == value);
                }

                if (PSVM.fromDate != null || PSVM.toDate != null)
                {
                    if (PSVM.fromDate != null && PSVM.toDate == null)
                    {
                        //query here
                        _result = _result.Where(p => p.Date >= PSVM.fromDate);
                    }
                    else if (PSVM.fromDate == null && PSVM.toDate != null)
                    {
                        //query here
                        _result = _result.Where(p => p.Date <= PSVM.fromDate);
                    }
                    else if (PSVM.fromDate != null && PSVM.toDate != null)
                    {
                        //query here
                        _result = _result.Where(p => p.Date >= PSVM.fromDate && p.Date <= PSVM.toDate);
                    }

                }
                if (PSVM.IsActive != null)
                {
                    //query here
                    if (PSVM.IsActive == 1) // active
                    {
                        _result = _result.Where(p => p.IsActive == 1);
                    }
                    else if (PSVM.IsActive == 2) // cancelled
                    {
                        _result = _result.Where(p => p.IsActive == 0);
                    }

                }

            }

            return _result;
        }
    }
}