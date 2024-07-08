using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.Property
{
    [Table("Amenities")]

    public class Amenity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PropertyAmenties> PropertyAmenities { get; set; }
    }
}
