using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Company
{
    public class CompanyDetailsDto
    {
  
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyStructure{ get; set; }
        public string CompanyRegistrationNumber { get; set; }
        public string LicenseNumber { get; set; }
        public string ReraCertificateNumber { get; set; }
        public string BusinessActivity { get; set; }
        public string CompanyAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string WebsiteUrl { get; set; }

        public string RepresentativeName { get; set; }
        public string RepresentativeEmail { get; set; }
        public string RepresentativePosition { get; set; }
        public string RepresentativeContactNumber { get; set; }
        public string CompanyRegistrationDoc { get; set; }

        public string TradeLicenseCopy { get; set; }
        public string ReraCertificateCopy { get; set; }
        public string TenancyContract { get; set; }
        public string CompanyLogo { get; set; }
        public string BusinessDescription { get; set; }
        public int NumberOfEmployees { get; set; }
    }
}
