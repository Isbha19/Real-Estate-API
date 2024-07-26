using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Property
{
    public class TopPropertiesDto
    {
        public int propertyId { get; set; }
        public string PropertyTitle { get; set; }
        public int PropertyViews { get; set; }
        public string PrimaryImageUrl { get; set; }

    }
}
