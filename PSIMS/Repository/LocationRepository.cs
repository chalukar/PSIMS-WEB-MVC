using IdentitySample.Models;
using PSIMS.Models.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PSIMS.Repository
{
    public class LocationRepository
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public int LocationDuplicationCheck(Location location)
        {
            //check if the input Location name already exists
            List<Location> _location = (from b in db.Locations where (b.LocationCode == location.LocationCode) || (b.LocationName == location.LocationName) select b).ToList();
            return _location.Count;
        }
    }
}