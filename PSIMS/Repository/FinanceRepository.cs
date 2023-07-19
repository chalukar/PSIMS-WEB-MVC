using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PSIMS.Models;
using PSIMS.Models.Finance;
using PSIMS.Models.SalesModel;
using PSIMS.Repository;
using PSIMS.Service;
using PSIMS.ViewModel;



namespace PSIMS.Repository
{
    public class FinanceRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // Set Receipt Print
        public int SetPaymentReceiptprint(int? printid)
        {
            try
            {
                PaymentSettelmentMaster _printsalesid = new PaymentSettelmentMaster();
                _printsalesid = db.PaymentSettelmentMasters.Find(printid);
                _printsalesid.isprint = 1;

                db.SaveChanges();
                return _printsalesid.ID;
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
        // Receipt invoice 
        public PaymentSettelmentMaster GetPaymentReceipt(int? receiptid)
        {
            try
            {

                SetPaymentReceiptprint(receiptid);

                PaymentSettelmentMaster _Pntmst = null;
                List<PaymentSettelmentMaster> _PReceipt = (from rp in db.PaymentSettelmentMasters.Include(b => b.Bank).Include(c => c.Customer).Include(p => p.paymentSettelmentDetails)
                                                           where rp.ID == receiptid
                                                           select rp).ToList();

                foreach (var p in _PReceipt)
                {

                    _Pntmst = new PaymentSettelmentMaster()
                    {
                        ID = p.ID,
                        PaymentDate = p.PaymentDate,
                        ReceiptAmount = p.ReceiptAmount,
                        CustomerAmount = p.CustomerAmount,
                        InvoiceNos = p.InvoiceNos,
                        PaymentMode = p.PaymentMode,
                        Bank = p.Bank,
                        BankID = p.BankID,
                        ChequeNo = p.ChequeNo,
                        PaymentDepositDate = p.PaymentDepositDate,
                        Customer = p.Customer,
                        CustomerID = p.CustomerID,
                        paymentSettelmentDetails = p.paymentSettelmentDetails,
                        NumberToWords = new Repository.NumberToEnglish().changeToWords(Convert.ToDecimal(p.ReceiptAmount).ToString()),

                    };
                }

                return _Pntmst;
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

        //Insert Receipt Master Data
        public int InsertReceiptMaster(PaymentSettelmentMaster rtpPay)
        {
            try
            {
                db.PaymentSettelmentMasters.Add(rtpPay);
                db.SaveChanges();
                return rtpPay.ID;
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

        //Insert Payment Receipt Details
        public void InsertReceiptDetails(PaymentSettelmentDetails paymentDetails)
        {
            try
            {
                db.PaymentSettelmentDetails.Add(paymentDetails);
                db.SaveChanges();

                Insert_PaymentDetailsForAuditTray(paymentDetails.Audit_tray_recipt_MasterID, paymentDetails.PaymentSettelmentMasterID, paymentDetails.InvoiceID, paymentDetails.ReceiptAmount, paymentDetails.ID, paymentDetails.CreatedBy, paymentDetails.CreatedOn, paymentDetails.InvGrandTot, paymentDetails.UnitBalance);

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

        // update Sales Pending data - last Receipt Amount ,Unit Banalce For Create Receipt Payment
        public void UpdateSalesPendingPayment(int getinvoiceID, string getPaytype, decimal? getunitbalance, decimal? getRecptAmnt)
        {
            try
            {
                Sales _Pendingsales = new Sales();
                _Pendingsales = db.Sales.Find(getinvoiceID);
                _Pendingsales.IsPayment = true;
                _Pendingsales.PaymentType = getPaytype;
                _Pendingsales.unitbalance = getunitbalance;
                _Pendingsales.LastReceiptAmt = getRecptAmnt;
                db.SaveChanges();
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

        //update Sales Pending data - last Receipt Amount ,Unit Banalce For Update Receipt Payment
        public void UpdateSalesPendingPaymentForChangereciptDetails(Sales updateSales)
        {
            try
            {
                Sales _Updateslaes = new Sales();
                _Updateslaes = db.Sales.Find(updateSales.ID);

                _Updateslaes.PaymentType = updateSales.PaymentType;
                _Updateslaes.LastReceiptAmt = updateSales.LastReceiptAmt;
                _Updateslaes.unitbalance = updateSales.unitbalance;
                db.SaveChanges();
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

        //Update Receipt Master Data
        public int UpdateReceiptMaster(PaymentSettelmentMaster rtpPay)
        {
            try
            {
                PaymentSettelmentMaster _UpdatePaymst = new PaymentSettelmentMaster();
                _UpdatePaymst = db.PaymentSettelmentMasters.Find(rtpPay.ID);
                _UpdatePaymst.ReceiptAmount = rtpPay.ReceiptAmount;
                _UpdatePaymst.CustomerAmount = rtpPay.CustomerAmount;
                _UpdatePaymst.InvoiceNos = rtpPay.InvoiceNos;
                _UpdatePaymst.PaymentMode = rtpPay.PaymentMode;
                _UpdatePaymst.BankID = rtpPay.BankID;
                _UpdatePaymst.PaymentDepositDate = rtpPay.PaymentDepositDate;
                _UpdatePaymst.ChequeNo = rtpPay.ChequeNo;
                _UpdatePaymst.PaymentMode = rtpPay.PaymentMode;
                db.SaveChanges();
                return rtpPay.ID;
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

        //Update Receipt Details Data
        public int UpdateReceiptDetails(PaymentSettelmentDetails rtpPay)
        {
            try
            {
                PaymentSettelmentDetails _UpdatePaydet = new PaymentSettelmentDetails();
                _UpdatePaydet = db.PaymentSettelmentDetails.Find(rtpPay.ID);

                _UpdatePaydet.ReceiptAmount = rtpPay.ReceiptAmount;
                _UpdatePaydet.UnitBalance = rtpPay.UnitBalance;
                db.SaveChanges();
                return rtpPay.ID;
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


        // Insert Audit Tray Receipt Master Data - Create Receipt Master Datas
        public int auditTrayReceiptMaster(Audit_tray_recipt_master atrm)
        {
            try
            {
                db.Audit_tray_recipt_masters.Add(atrm);
                db.SaveChanges();
                return atrm.ID;
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

        public int auditTrayReceiptMaster_Update(Audit_tray_recipt_master atrm)
        {
            try
            {
                db.Audit_tray_recipt_masters.Add(atrm);
                db.SaveChanges();
                return atrm.ID;
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

        public int auditTrayReceiptDetails_Insert(Audit_tray_recipt_details atrd)
        {
            try
            {
                db.Audit_tray_recipt_details.Add(atrd);
                db.SaveChanges();
                return atrd.ID;
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

    

        //Insert Receipt payment Data - Audit Tray - Update Mode
        public void Insert_PaymentDetailsForAuditTray(int audit_tray_recipt_MasterID, int paymentSettelmentMasterID, int invoiceID, decimal receiptAmount, int ReciptDetailsID, string createdBy, DateTime? createdOn,decimal invGrandTotal,decimal? unitBalance)
        {
            Audit_tray_recipt_details atrmd = new Audit_tray_recipt_details();
            atrmd.Audit_tray_recipt_masterID = audit_tray_recipt_MasterID;
            atrmd.ReceiptMasterID = paymentSettelmentMasterID;
            atrmd.InvoiceNo = invoiceID;
            atrmd.ReceiptAmount = receiptAmount;
            atrmd.audit_Mode = "I";
            atrmd.ReciptDetaisID = ReciptDetailsID;
            atrmd.InvGrandTot = invGrandTotal;
            atrmd.UnitBalance = unitBalance;
            atrmd.CreatedBy = createdBy;
            atrmd.CreatedOn = createdOn;

            try
            {
                db.Audit_tray_recipt_details.Add(atrmd);
                db.SaveChanges();
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