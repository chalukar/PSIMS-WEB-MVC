using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentitySample.Models;
using PSIMS.Models.Finance;

namespace PSIMS.Repository
{
    public class BankRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public int BankDuplicationCheck(Bank bank)
        {
            //check if the input supplier name already exists
            List<Bank> _bank = (from s in db.Banks
                                        where s.BankCode == bank.BankCode || s.BankName == bank.BankName
                                        select s).ToList();
            return _bank.Count;
        }
    }
}