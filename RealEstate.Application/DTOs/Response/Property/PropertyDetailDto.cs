using RealEstate.Application.DTOs.Request.Property;

namespace RealEstate.Application.DTOs.Response.Property
{
    public class PropertyDetailDto
    {
        public int Id { get; set; }
        public string PropertyTitle { get; set; }
        public string PropertyDescription { get; set; }
        public string PropertyType { get; set; }
        public string ListingType { get; set; }
        public string Location { get; set; }
        public double Size { get; set; }
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public string FurnishingType { get; set; }
        public ICollection<ImageDto> Images { get; set; }
        public DateOnly AvailableFrom { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> NearByFacilities { get; set; }

        public string AgentName { get; set; }
        public string AgentUserId { get; set; }

        public string AgentImage { get; set; }
        public int AgentPhoneNumber { get; set; }
        public string AgentEmail { get; set; }
        public int AgentWhatsapp { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLogo { get; set; }
        public string AgentPropertyCounts { get; set; }
    }


}
