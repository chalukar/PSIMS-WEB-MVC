using IdentitySample.Models;
using PSIMS.Models.PurchaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PSIMS.Repository
{
    public class CountryRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public int CountryDuplicationCheck(Country country)
        {
            //check if the input Location name already exists
            List<Country> _country = (from b in db.Countries where (b.CountryName == country.CountryName) select b).ToList();
            return _country.Count;
        }
    }
}