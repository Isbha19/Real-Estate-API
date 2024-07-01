using RealEstate.Domain.Entities.Property.Property;

namespace RealEstate.Application.DTOs.Request.Property
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
    }


}
