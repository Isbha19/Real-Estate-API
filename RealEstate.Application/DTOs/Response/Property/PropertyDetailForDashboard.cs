using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Property
{
    public class PropertyDetailForDashboardDto
    {
        public int PropertyId { get; set; }
        public string PropertyTitle { get; set; }
        public string PropertyType { get; set; }
        public string ListingType { get; set; }
        public string Location { get; set; }
        public int Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Size { get; set; }
        public int PropertyViews { get; set; }
        public string AgentName { get; set; }
        public DateTime PostedOn { get; set; }
        public string PrimaryImageUrl { get; set; }
    }
}
