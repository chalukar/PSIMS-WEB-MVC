using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSIMS.Models.SalesModel;
using IdentitySample.Models;

namespace PSIMS.Repository
{
    public class CustomerRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public int CustomerDuplicationCheck(Customer customer)
        {
            //check if the input supplier name already exists
            List<Customer> _customer = (from c in db.Customers where c.CustomerName == customer.CustomerName select c).ToList();

            return _customer.Count;
        }
    }
}