﻿using Microsoft.AspNetCore.Http;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Company;
using RealEstate.Domain.Entities.Company;
using RealEstate.Domain.Entities.Property;
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

        Task<string> CreateCustomerPortalSession(string customerId);



    }

}
