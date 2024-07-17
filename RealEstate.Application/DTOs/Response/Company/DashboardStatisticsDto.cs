

namespace RealEstate.Application.DTOs.Response.Company
{
    public class DashboardStatisticsDto
    {
        public int propertiesUsed { get; set; }
        public int propertyLimit { get; set; }
        public int PropertiesListedCount { get; set; }
        public int NumberOfAgents { get; set; }
        public int PropertyViews { get; set; }
        public decimal SubscriptionAmtPaid { get; set; }
    }
}
