using IdentitySample.Models;
using PSIMS.Models.Finance;
using PSIMS.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.Repository
{
    public class ReceiptFilterRepository
    {
        private ApplicationDbContext db;

        public ReceiptFilterRepository()
        {
            db = new ApplicationDbContext();
        }
        public List<Payment> FilterReceipt(ReceiptListVM receiptlist)
        {
            List<Payment> result = null;
            if (receiptlist != null)
            {
                if (!string.IsNullOrEmpty(receiptlist.searchVal))
                {
                    var value = Convert.ToInt32(receiptlist.searchVal);
                    if (receiptlist.searchVal != null)
                    {
                       result= (from s in db.Payments
                                where s.ID == value
                                select s).ToList();
                    }

                } 
            }
            return result;
        }

        //public IQueryable<Payment> FilterReceipt(ReceiptListVM receiptlist)
        //{
        //    var result = db.Payments.Include("Customer").AsQueryable();
        //    if (receiptlist != null)
        //    {
        //        if (!string.IsNullOrEmpty(receiptlist.searchVal))
        //        {
        //            var value = Convert.ToInt32(receiptlist.searchVal);
        //            if (receiptlist.searchVal != null)
        //            {

        //                result = result.Where(p => p.ID == value);
        //            }
                    
        //        } 
        //    }
        //    return result;
        //}
    }
}