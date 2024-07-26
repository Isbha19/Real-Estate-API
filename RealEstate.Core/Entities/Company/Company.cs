


using RealEstate.Domain.Entities.AgentEntity;
using RealEstate.Domain.Entities.PropertyEntity;
using RealEstate.Domain.Entities.SubscriptionEntity;
namespace RealEstate.Domain.Entities.CompanyEntity
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string TradeName { get; set; }
        public int CompanyStructureId { get; set; }
        public  CompanyStructure CompanyStructure { get; set; }

        public string CompanyRegistrationNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string ReraCertificateNumber { get; set; }
        public int BusinessActivityTypeId { get; set; }
        public  BusinessActivityType BusinessActivityType { get; set; }

        public string CompanyAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string WebsiteUrl { get; set; }

        public string RepresentativeId { get; set; }
        public  User Representative { get; set; }
        public string RepresentativePosition { get; set; }
        public string RepresentativeContactNumber { get; set; }
        public string CompanyRegistrationDoc { get; set; }

        public bool isAdminVerified {  get; set; }
        public VerificationStatus VerificationStatus { get; set; }
        public string RejectionReason { get; set; }
        public string TradeLicenseCopy { get; set; }
        public string ReraCertificateCopy { get; set; }
        public string TenancyContract { get; set; }
        public  CompanyFile CompanyLogo { get; set; }
        public  ICollection<Agent> Agents { get; set; }
        public int UsedPropertyCounts { get; set; }
        public decimal SubscriptionAmtPaid { get; set; }

        public string BusinessDescription { get; set; }
        public int NumberOfEmployees { get; set; }
        public  Subscription Subscription { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
    public enum VerificationStatus
    {
        Pending,
        Verified,
        Rejected
    }
}
