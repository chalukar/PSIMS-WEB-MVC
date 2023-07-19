using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PSIMS.Repository;
using PSIMS.Models.QuotationModel;

namespace PSIMS.Service
{
    public class QuotationEntryService
    {
        private QuotationRepository repo = new QuotationRepository();

        public void AddQuotationAndQuotationItems(Quotation quotation)
        {
            repo.AddQuotationAndQuotationItems(quotation);
        }

        public Quotation GetquotationForm(int? quoteid)
        {
            return repo.GetquotationForm(quoteid);
        }

    }
}