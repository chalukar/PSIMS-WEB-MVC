using IdentitySample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PSIMS.Models.QuotationModel;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace PSIMS.Repository
{
    public class QuotationRepository
    {
        

        public int QuotationCatDuplicationCheck(QuotationCategory quotacat)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //check if the input supplier name already exists
                List<QuotationCategory> _quotacat = (from q in db.QuotationCategories
                                                     where (q.CategoryName == quotacat.CategoryName)
                                                     select q).ToList();

                return _quotacat.Count;
            }
                
        }

        public Quotation GetquotationForm(int? quoteid)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var _quotationFrom = from q in db.Quotations
                                     .Include(c =>c.Customer)
                                     .Include(qi =>qi.QuotationItems)
                                     where q.ID == quoteid
                                     select q;

                return _quotationFrom.FirstOrDefault();

                //var _quotationFrom = from q in db.Quotations
                //                     join qi in db.QuotationItems on q.ID equals qi.ID
                //                     join c in db.Customers on q.CustomerID equals c.ID
                //                     where q.ID == quoteid
                //                     select new { q = q, qi = qi, c = c };

                //return _quotationFrom.Select();
            }
        }

        public void AddQuotationAndQuotationItems(Quotation quotation)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                try
                {
                    db.Quotations.Add(quotation);
                    db.SaveChanges();
                    // return quotation.ID;
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