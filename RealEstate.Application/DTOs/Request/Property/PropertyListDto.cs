using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Request.Property
{
    public class PropertyListDto
    {
        public int Id { get; set; }
        public string PropertyTitle { get; set; }
        public string PropertyType { get; set; }
        public string ListingType { get; set; }
        public string Location { get; set; }
        public int size { get; set; }
        public int price { get; set; }
        public int Bathrooms { get; set; }
        public int Bedrooms { get; set; }
        public DateTime ListedDate { get; set; }
        public string PrimaryImageUrl { get; set; } 

    }
}
