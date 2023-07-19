using PSIMS.Models;
using PSIMS.Models.SalesModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PSIMS.ViewModel
{
    public class SalesCollectionVM
    {
        public SalesCollectionVM()
        {

        }
        public SalesCollectionVM(Sales sales)
        {
            ID = sales.ID;

            Date = sales.Date;
            Amount = sales.Amount;
            GrandTotal = sales.GrandTotal;
            //PayMode = sales.PayMode;
            //ChequeNo = sales.ChequeNo;
            //ChequeDate = sales.ChequeDate;
            //ChequeDepositDate = sales.ChequeDepositDate;
            //ConfirmdSales = sales.ConfirmdSales;
            //ConfiredSalesDate = sales.ConfiredSalesDate;
            Customer = sales.Customer;

        }

        public int ID { get; set; }

        public int CustomerID { get; set; }

        [DisplayFormat(DataFormatString="{0:dd-MM-yyyy}")]
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public decimal GrandTotal { get; set; }

        public string PayMode { get; set; }

        public string ChequeNo { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ChequeDepositDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ChequeDate { get; set; }
        
        public int ConfirmdSales { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}",ApplyFormatInEditMode=true)]
        public DateTime? ConfiredSalesDate { get; set; }

        public virtual Customer Customer { get; set; }

       
    }
}