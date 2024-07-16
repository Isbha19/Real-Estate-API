using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.PropertyEntity
{
    public class PropertyNearByFacilities:baseEntity
    {
        public int PropertyId { get; set; }
        public  Property Property { get; set; }

        public int FacilityId { get; set; }
        public  Facility Facility { get; set; }
    }
}
