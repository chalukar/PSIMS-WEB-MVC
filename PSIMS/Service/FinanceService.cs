using IdentitySample.Models;
using PSIMS.Models;
using PSIMS.Models.Finance;
using PSIMS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace PSIMS.Service
{
    public class FinanceService
    {
        FinanceRepository repo = new FinanceRepository();
        ApplicationDbContext db = new ApplicationDbContext();


        //Inert Receipt Master
        public int InsertReceiptMaster(PaymentSettelmentMaster rtpPay)
        {
            return repo.InsertReceiptMaster(rtpPay);
        }

        //insert Receipt Payment Details
        public void InsertReceiptDetails(int receiptID, string[] invID, string[] invDate, string[] loc, string[] custID, string[] paytype, string[] invtot, string[] recptAmnt,DateTime? CreatedOn,string CreatedBy, int audittrayMasterID,string[] balanceAmt)
        {
            PaymentSettelmentDetails _PaymentDetails = new PaymentSettelmentDetails();
            int count = invID.Count();
            for (int i = 0; i < count; i++)
            {
                decimal? getbalanceamt = 0;
                //Get Invoice sales ID
                int _salesID = Convert.ToInt32(invID[i]);
                decimal? _receiptAmtval = Convert.ToDecimal(recptAmnt[i]);
                decimal? _InvAmtval = Convert.ToDecimal(invtot[i]);

                decimal? _unitbalance = (from s in db.Sales
                                         where s.ID == _salesID
                                         select s.unitbalance).SingleOrDefault();

                if (_unitbalance == 0)
                {
                    getbalanceamt = _InvAmtval - _receiptAmtval;
                }
                else
                {
                    getbalanceamt = _unitbalance - _receiptAmtval;
                }

                
                _PaymentDetails.PaymentSettelmentMasterID = receiptID;

                _PaymentDetails.InvoiceID = Convert.ToInt32(invID[i]);
                _PaymentDetails.InvoiceDate = Convert.ToDateTime(invDate[i]);
                _PaymentDetails.InvLocation = Convert.ToString(loc[i]);
                _PaymentDetails.CustomerID = Convert.ToInt32(custID[i]);
                _PaymentDetails.PaymentType = paytype[i];
                _PaymentDetails.InvGrandTot = Convert.ToDecimal(invtot[i]);
                _PaymentDetails.ReceiptAmount = Convert.ToDecimal(recptAmnt[i]);
                _PaymentDetails.UnitBalance = getbalanceamt;
                _PaymentDetails.Status = true;
                _PaymentDetails.Audit_tray_recipt_MasterID = audittrayMasterID;
                _PaymentDetails.CreatedBy = CreatedBy;
                _PaymentDetails.CreatedOn = CreatedOn;
                //_PaymentDetails.UnitBalance = Convert.ToDecimal(balanceAmt[i]);

                repo.InsertReceiptDetails(_PaymentDetails);

            }
            
        }

      
        //Receipt Print Option
        public PaymentSettelmentMaster GetPaymentReceipt(int? receiptid)
        {
            return repo.GetPaymentReceipt(receiptid);
        }

      
       // Update Receipt 
        public int UpdateReceiptMaster(PaymentSettelmentMaster rtpPay)
        {
            return repo.UpdateReceiptMaster(rtpPay);
        }

        public int UpdateReceiptDetails(PaymentSettelmentDetails rtpPay)
        {
            return repo.UpdateReceiptDetails(rtpPay);
        }


        //Inert Autit Tray
        public int auditTrayReceiptMaster(int receiptID, DateTime paymentDate, decimal customerAmount, decimal receiptAmount, string user)
        {
            Audit_tray_recipt_master atrm = new Audit_tray_recipt_master();

            atrm.ReceiptMasterID = receiptID;
            atrm.PaymentDate = paymentDate;
            atrm.CustomerAmount = customerAmount;
            atrm.ReceiptAmount = receiptAmount;
            atrm.audit_Mode = "I";
            atrm.CreatedOn = DateTime.Now;
            atrm.CreatedBy = user;
            return repo.auditTrayReceiptMaster(atrm);
        }

        public int auditTrayReceiptMaster_Update(int receiptID, DateTime paymentDate, decimal customerAmount, decimal receiptAmount, string user)
        {
            Audit_tray_recipt_master atrm = new Audit_tray_recipt_master();

            atrm.ReceiptMasterID = receiptID;
            atrm.PaymentDate = paymentDate;
            atrm.CustomerAmount = customerAmount;
            atrm.ReceiptAmount = receiptAmount;
            atrm.audit_Mode = "U";
            atrm.CreatedOn = DateTime.Now;
            atrm.CreatedBy = user;
            return repo.auditTrayReceiptMaster_Update(atrm);
        }

        public int auditTrayReceiptDetails_Insert(int audit_tray_recipt_MasterID, int paymentSettelmentMasterID, int invoiceID, decimal receiptAmount, int receiptDetailsID, DateTime? createdOn, string createdBy, decimal invGrandTot, decimal? unitBalance)
        {
            Audit_tray_recipt_details atrd = new Audit_tray_recipt_details();
            atrd.Audit_tray_recipt_masterID = audit_tray_recipt_MasterID;
            atrd.ReceiptMasterID = paymentSettelmentMasterID;
            atrd.InvoiceNo = invoiceID;
            atrd.ReceiptAmount = receiptAmount;
            atrd.audit_Mode = "U";
            atrd.ReciptDetaisID = receiptDetailsID;
            atrd.CreatedOn = createdOn;
            atrd.CreatedBy = createdBy;
            atrd.InvGrandTot = invGrandTot;
            atrd.UnitBalance = unitBalance;
            return repo.auditTrayReceiptDetails_Insert(atrd);
        }


        //Update Sales Models -Create
        public void UpdateSalesPendingPayment(string[] invID, string[] paytype, string[] invAmt, string[] RecptAmnt)
        {
            int count = invID.Count();
            for (int y = 0; y < count; y++)
            {
                decimal? getbalanceamt = 0;
                //Get Invoice sales ID
                int _salesID = Convert.ToInt32(invID[y]);
                decimal? _receiptAmtval = Convert.ToDecimal(RecptAmnt[y]);
                decimal? _InvAmtval = Convert.ToDecimal(invAmt[y]);

                decimal? _unitbalance = (from s in db.Sales
                                         where s.ID == _salesID
                                         select s.unitbalance).SingleOrDefault();

                if (_unitbalance == 0)
                {
                    getbalanceamt = _InvAmtval - _receiptAmtval;
                }
                else
                {
                    getbalanceamt = _unitbalance - _receiptAmtval;
                }

                int getinvoiceID = Convert.ToInt32(invID[y]);
                string getPaytype = paytype[y];
                decimal getinvamt = Convert.ToDecimal(invAmt[y]);
                decimal getRecptAmnt = Convert.ToDecimal(RecptAmnt[y]);
                decimal? getunitbalance = getbalanceamt;
                repo.UpdateSalesPendingPayment(getinvoiceID, getPaytype, getunitbalance, getRecptAmnt);
            }



        }

        //Update Sales Models -Update
        public void UpdateSalesPendingPaymentForChangereciptDetails(int invoiceID, string paymentType, decimal receiptAmount, decimal? unitBalance)
        {

            Sales _updateSales = new Sales();
            _updateSales.ID = invoiceID;
            _updateSales.PaymentType = paymentType;
            _updateSales.LastReceiptAmt = receiptAmount;
            _updateSales.unitbalance = unitBalance;
            repo.UpdateSalesPendingPaymentForChangereciptDetails(_updateSales);
        }
    }
}