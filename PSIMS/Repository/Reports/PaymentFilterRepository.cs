using IdentitySample.Models;
using PSIMS.Models.Finance;
using PSIMS.ViewModel.ForReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Security.Claims;
using IdentitySample;
using PSIMS.ViewModel;
using PSIMS.Models;
using PSIMS.Models.SalesModel;
using PSIMS.Models.Locations;

namespace PSIMS.Repository.Reports
{
    public class PaymentFilterRepository
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager = null;
        private RoleManager<IdentityRole> roleManager = null;


        public PaymentFilterRepository()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
        }
        public IQueryable<PaymentSettlmentViewModel> FilterPayments(PaymentSerachVM vm)
        {
            // var role = manager.FindById(User.Identity.GetUserId()).Roles.FirstOrDefault();

            ////var result = db.PaymentSettelmentMasters.Include("Customer").Include("Bank").AsQueryable();

            List<Customer> cutomers = db.Customers.ToList();
            List<Bank> banks = db.Banks.ToList();
            List<Location> locations = db.Locations.ToList();
            List<PaymentSettelmentMaster> paymentmasters = db.PaymentSettelmentMasters.ToList();
            List<PaymentSettelmentDetails> paymentdetailss = db.PaymentSettelmentDetails.ToList();

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

            var result = payments.AsQueryable();

            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;


            if (vm != null)
            {
                if (vm.InvoiceID != 0)
                {
                    var value = vm.InvoiceID;
                    //query here
                    result = result.Where(p => p.paymentdetails.InvoiceID == value);
                }


                if (!string.IsNullOrEmpty(vm.option))
                {
                    if (vm.option == "today")
                    {
                        result = from p in result
                                 where p.paymentmaster.PaymentDate.Day == DateTime.Today.Day
                                 where p.paymentmaster.PaymentDate.Month == DateTime.Today.Month
                                 where p.paymentmaster.PaymentDate.Year == DateTime.Today.Year
                                 select p;

                        //.Where(p => p.Date.Day == DateTime.Today.Day );

                    }
                    else if (vm.option == "yesterday")
                    {
                        result = from p in result
                                 where p.paymentmaster.PaymentDate.Day == DateTime.Today.Day - 1
                                 where p.paymentmaster.PaymentDate.Month == DateTime.Today.Month
                                 where p.paymentmaster.PaymentDate.Year == DateTime.Today.Year
                                 select p;
                        //result = result.Where(p => p.Date.Date == DateTime.Now.Date.AddDays(-1));
                    }
                    else if (vm.option == "thisWeek")
                    {
                        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                        result = result.Where(x => x.paymentmaster.PaymentDate >= startDayOfWeek && x.paymentmaster.PaymentDate <= endDayOfWeek);
                        // result = result.Where(p=> p.Date.);
                    }
                    else if (vm.option == "thisMonth")
                    {

                        result = result.Where(p => p.paymentmaster.PaymentDate.Month == month);
                    }
                    else if (vm.option == "lastMonth")
                    {
                        result = result.Where(p => p.paymentmaster.PaymentDate.Month == month - 1);
                    }
                    else if (vm.option == "thisYear")
                    {
                        result = result.Where(p => p.paymentmaster.PaymentDate.Year == year);

                    }
                    else if (vm.option == "lastYear")
                    {
                        result = result.Where(p => p.paymentmaster.PaymentDate.Year == year - 1);
                    }
                }

                if (!string.IsNullOrEmpty(vm.customer))
                {
                    var value = Convert.ToInt32(vm.customer);
                    //query here
                    result = result.Where(p => p.paymentmaster.CustomerID == value);
                }

                if (vm.fromDate != null || vm.toDate != null)
                {
                    if (vm.fromDate != null && vm.toDate == null)
                    {
                        //query here
                        result = result.Where(p => p.paymentmaster.PaymentDate >= vm.fromDate);
                    }
                    else if (vm.fromDate == null && vm.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.paymentmaster.PaymentDate <= vm.fromDate);
                    }
                    else if (vm.fromDate != null && vm.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.paymentmaster.PaymentDate >= vm.fromDate && p.paymentmaster.PaymentDate <= vm.toDate);
                    }
                }

               
                    //query here
                    //if (vm.IsActive == true)
                    //{
                    //    result = result.Where(p => p.paymentmaster.IsActive == true);
                    //}
                    //else
                    //{
                    //    result = result.Where(p => p.paymentmaster.IsActive == false);
                    //}

                
            }
            return result;
        }

        public IQueryable<PaymentSettelmentMaster> FilterPaymentHistoryDetails(PaymentSerachVM vm)
        {
            //var _result = db.Sales.Include("Customer").Include("invoiceOptionEntry").AsQueryable();
            var result = db.PaymentSettelmentMasters.Include("Customer").Include("Bank").AsQueryable();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (vm != null)
            {
               

                if (!string.IsNullOrEmpty(vm.option))
                {
                    if (vm.option == "today")
                    {
                        result = from p in result
                                 where p.PaymentDate.Day == DateTime.Today.Day
                                 where p.PaymentDate.Month == DateTime.Today.Month
                                 where p.PaymentDate.Year == DateTime.Today.Year
                                 select p;

                        //.Where(p => p.Date.Day == DateTime.Today.Day );

                    }
                    else if (vm.option == "yesterday")
                    {
                        result = from p in result
                                 where p.PaymentDate.Day == DateTime.Today.Day - 1
                                 where p.PaymentDate.Month == DateTime.Today.Month
                                 where p.PaymentDate.Year == DateTime.Today.Year
                                 select p;
                        //result = result.Where(p => p.Date.Date == DateTime.Now.Date.AddDays(-1));
                    }
                    else if (vm.option == "thisWeek")
                    {
                        DateTime startDayOfWeek = DateTime.Today.AddDays(-1 * (int)(DateTime.Today.DayOfWeek));
                        DateTime endDayOfWeek = DateTime.Today.AddDays(6 - (int)DateTime.Today.DayOfWeek);

                        result = result.Where(x => x.PaymentDate >= startDayOfWeek && x.PaymentDate <= endDayOfWeek);
                        // result = result.Where(p=> p.Date.);
                    }
                    else if (vm.option == "thisMonth")
                    {

                        result = result.Where(p => p.PaymentDate.Month == month);
                    }
                    else if (vm.option == "lastMonth")
                    {
                        result = result.Where(p => p.PaymentDate.Month == month - 1);
                    }
                    else if (vm.option == "thisYear")
                    {
                        result = result.Where(p => p.PaymentDate.Year == year);

                    }
                    else if (vm.option == "lastYear")
                    {
                        result = result.Where(p => p.PaymentDate.Year == year - 1);
                    }
                }
                if (!string.IsNullOrEmpty(vm.customer))
                //if (vm.customer !="0")
                {
                    var value = Convert.ToInt32(vm.customer);
                    //query here
                    result = result.Where(p => p.CustomerID == value);
                }

                if (vm.fromDate != null || vm.toDate != null)
                {
                    if (vm.fromDate != null && vm.toDate == null)
                    {
                        //query here
                        result = result.Where(p => p.PaymentDate >= vm.fromDate);
                    }
                    else if (vm.fromDate == null && vm.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.PaymentDate <= vm.fromDate);
                    }
                    else if (vm.fromDate != null && vm.toDate != null)
                    {
                        //query here
                        result = result.Where(p => p.PaymentDate >= vm.fromDate && p.PaymentDate <= vm.toDate);
                    }
                }

                //if (!string.IsNullOrEmpty(vm.IsPaid))
                //{
                //    //query here
                //    if (vm.IsPaid == "paid")
                //    {
                //        result = result.Where(p => p.IsPaid == true);
                //    }
                //    else
                //    {
                //        result = result.Where(p => p.IsPaid == false);
                //    }

                //}
            }

            return result;
        }





        public IQueryable<Sales> FiltercustomerSales(SalesSearchVM PSVM)
        {
            //var _result = db.Sales.Include("Customer").Include("invoiceOptionEntry").AsQueryable();
            var _result = (from s in db.Sales.Include(c => c.Customer).Include(i => i.invoiceOptionEntry)
                           where s.IsActive == 1
                           where (s.PaymentType == "0" && s.unitbalance == 0 && s.LastReceiptAmt == 0) || (s.PaymentType == "PP" && s.unitbalance != 0)
                           select s).AsQueryable();
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (PSVM != null)
            {
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
                if (PSVM.Company != "0")
                {
                    var value = Convert.ToInt32(PSVM.Company);
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