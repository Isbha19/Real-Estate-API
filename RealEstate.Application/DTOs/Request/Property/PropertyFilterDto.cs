

namespace RealEstate.Application.DTOs.Request.Property
{
    public class PropertyFilterDto
    {
        public string? Location { get; set; }
        public int? ListingType { get; set; }
        public int? PropertyType { get; set; }
        public List<int>? Bedrooms { get; set; } = new List<int>();
        public List<int>? Bathrooms { get; set; } = new List<int>();
        public int? MinPrice { get; set; }
        public int? MaxPrice { get; set; }
        public List<int>? NearbyFacilities { get; set; } = new List<int>();
        public List<int>? Amenities { get; set; } = new List<int>();
        public int? Furnished { get; set; }
        public int? MinSize { get; set; }
        public int? MaxSize { get; set; }
        public bool? VirtualTour { get; set; }
    }
}
