using IdentitySample.Models;
using PSIMS.Models.SalesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PSIMS.Repository
{
    public class InvoiceTypeRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public int InvoiceTypeDuplicationCheck(InvoiceOptionEntry invoiceOptionEntry)
        {
            //check if the input supplier name already exists
            List<InvoiceOptionEntry> _invoiceOptionEntry = (from b in db.InvoiceOptionEntries where (b.InvoiceName == invoiceOptionEntry.InvoiceName) select b).ToList();

            return _invoiceOptionEntry.Count;
        }
    }
}