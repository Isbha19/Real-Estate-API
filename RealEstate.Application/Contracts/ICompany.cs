using Microsoft.AspNetCore.Http;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Company;
using RealEstate.Domain.Entities.CompanyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Contracts
{

    public interface ICompany
        {
        Task<CompanyRegisterResponse> RegisterCompanyAsync(CompanyDto companyDto);
        Task<IEnumerable<CompanyStructure>> GetCompanyStructuresAsync();
        Task<IEnumerable<BusinessActivityType>> GetBusinessActivityTypesAsync();
        Task<GeneralResponse> AddCompanyLogoAsync(IFormFile logo, int companyId);
        Task<GeneralResponse> VerifyCompany(int companyId);
        Task<GeneralResponse> ValidateUserForPayment();
        Task<IEnumerable<CompanyNames>> GetCompanyNamesAsync();



        Task<string> CreateCustomerPortalSession(string customerId);
        Task<IEnumerable<CompanyDetailsDto>> GetVerifiedCompaniesDetailsAsync();
        Task<IEnumerable<CompanyDetailsDto>> GetUnVerifiedCompaniesDetailsAsync();
        Task<CompanyDetailsDto> GetCompanyDetailByUser();




    }

}
