using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.Property.Property
{
    [Table("Amenities")]

    public class Amenity:baseEntity
    {
        public string Name { get; set; }
        public ICollection<PropertyAmenties> PropertyAmenities { get; set; }
    }
}
