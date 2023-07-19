using IdentitySample.Models;
using PSIMS.Models.InventoryModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.Repository
{
    public class ManufacturerRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public int ManufacturerDuplicationCheck(Manufacturer manufacturer)
        {
            List<Manufacturer> _manufacturer = (from m in db.Manufacturers
                                                where m.ManufacturerName == manufacturer.ManufacturerName
                                                select m).ToList();
            return _manufacturer.Count;
        }
    }
}