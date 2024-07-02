

using RealEstate.Domain.Entities.Property;

namespace RealEstate.Domain.Entities.Company
{
    public class Company
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string TradeName { get; set; }
        public int CompanyStructureId { get; set; }
        public CompanyStructure CompanyStructure { get; set; }

        public string CompanyRegistrationNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string ReraCertificateNumber { get; set; }
        public int BusinessActivityTypeId { get; set; }
        public BusinessActivityType BusinessActivityType { get; set; }

        public string CompanyAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string WebsiteUrl { get; set; }

        public string RepresentativeId { get; set; }
        public User Representative { get; set; }
        public string RepresentativePosition { get; set; }
        public string RepresentativeContactNumber { get; set; }
        public string CompanyRegistrationDoc { get; set; }


        public string TradeLicenseCopy { get; set; }
        public string ReraCertificateCopy { get; set; }
        public string TenancyContract { get; set; }

        public CompanyFile CompanyLogo { get; set; }
        public string BusinessDescription { get; set; }
        public int NumberOfEmployees { get; set; }
    }
}
