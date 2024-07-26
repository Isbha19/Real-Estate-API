

namespace RealEstate.Application.DTOs.Request.Property
{
    public class PropertyStatisticsDto
    {
        public List<string> Labels { get; set; }
        public List<int> ListedProperties { get; set; }
        public List<int> SoldProperties { get; set; }
    }
}
