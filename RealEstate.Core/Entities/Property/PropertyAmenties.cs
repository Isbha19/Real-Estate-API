using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.Property.Property
{
    public class PropertyAmenties:baseEntity
    {
        public int PropertyId { get; set; }
        public Property Property { get; set; }

        public int AmenityId { get; set; }
        public Amenity Amenity { get; set; }
    }
}
