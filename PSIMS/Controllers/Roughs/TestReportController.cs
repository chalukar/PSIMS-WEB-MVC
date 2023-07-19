using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PSIMS.ViewModel;
using IdentitySample.Models;
using PSIMS.Models.InventoryModel;

namespace PSIMS.Controllers.Roughs
{
    public class TestReportController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: TestReport
        public ActionResult Index()
        {            
            return View(db.Stocks.ToList());                   
        }

        [HttpPost]
        public ActionResult index(StockSearchVM vm)
        {
            var business = new FilterBusinessLogic();
            var model = business.GetStocks(vm);            
            return View(model);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class FilterBusinessLogic
    {
        private ApplicationDbContext db;
        public FilterBusinessLogic()
        {
            db = new ApplicationDbContext();
        }

        public IQueryable<Stock> GetStocks(StockSearchVM searchModel)
        {
            var result = db.Stocks.AsQueryable();
            if (searchModel != null)
            {
                if (!string.IsNullOrWhiteSpace(searchModel.batch))
                    result = result.Where(x => x.BatchNo == searchModel.batch);
                if (!string.IsNullOrEmpty(searchModel.name))
                    result = result.Where(x => x.Item.Name.Contains(searchModel.name));
                if (!string.IsNullOrEmpty(searchModel.option))
                {                   
                    if (searchModel.option == "BelowMin")
                    {
                        result =result.Where(x=> x.Qty < x.Item.AlertQty);
                    }
                    else if(searchModel.option == "UnSold")
                    {
                        result = result.Where(x => x.Qty == x.InitialQty);
                    }                
                 
                }
                if ((searchModel.fromDate != null )|| (searchModel.toDate != null))
                {
                    if (searchModel.fromDate != null && searchModel.toDate == null)
                    {
                        result = result.Where(x => x.ExpiryDate > searchModel.fromDate);
                    }
                    else if (searchModel.toDate != null && searchModel.fromDate == null)
                    {
                        result = result.Where(x => x.ExpiryDate < searchModel.toDate);
                    }
                    else
                    {
                        result = result.Where(x => (x.ExpiryDate > searchModel.fromDate && x.ExpiryDate < searchModel.toDate));
                    }
                }

                
                                      
            }
            return result;
        }
    }



}