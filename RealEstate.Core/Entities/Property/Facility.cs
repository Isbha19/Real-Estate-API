using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.Property
{
    [Table("Facilities")]

    public class Facility:baseEntity
    {
        public string Name { get; set; }
        public ICollection<PropertyNearByFacilities> PropertyNearByFacilities { get; set; }
    }
}
