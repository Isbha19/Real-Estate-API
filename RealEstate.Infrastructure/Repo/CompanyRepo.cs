using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Response;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.Company;
using RealEstate.Infrastructure.Data;
using RealEstate.Application.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using RealEstate.Infrastructure.Services;
using RealEstate.Domain.Entities.Property;
using RealEstate.Application.DTOs.Response.Company;

namespace RealEstate.Infrastructure.Repo
{
    public class CompanyRepo : ICompany
    {
        private readonly UserManager<User> userManager;
        private readonly AppDbContext context;
        private readonly FileService fileService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyRepo(UserManager<User> userManager, AppDbContext context, IHttpContextAccessor httpContextAccess,
            FileService fileService)
        {
            this.userManager = userManager;
            this.context = context;
            this.fileService = fileService;
            this._httpContextAccessor = httpContextAccess;

        }

        public async Task<IEnumerable<CompanyStructure>> GetCompanyStructuresAsync()
        {
            return await context.companyStructures.ToListAsync();
        }

        public async Task<IEnumerable<BusinessActivityType>> GetBusinessActivityTypesAsync()
        {
            return await context.businessActivityTypes.ToListAsync();
        }

        public async Task<CompanyRegisterResponse> RegisterCompanyAsync(CompanyDto model)
        {
          
            var currentUser = await GetUser();

            if (await userManager.IsInRoleAsync(currentUser, Constant.Agent))
            {
                return new CompanyRegisterResponse(false, "User is already registered as an agent and cannot be made a company admin.");
            }
            if (await userManager.IsInRoleAsync(currentUser,Constant.CompanyAdmin))
            {
                return new CompanyRegisterResponse(false, "User is already registered as a representative of a company.");
            }
          
            var companyExists = await context.companies
        .AnyAsync(c =>
            c.CompanyRegistrationNumber == model.CompanyRegistrationNumber ||
            c.LicenseNumber == model.LicenseNumber ||
            c.ReraCertificateNumber == model.ReraCertificateNumber);

            if (companyExists)
            {
                return new CompanyRegisterResponse(false, "Company with provided details already exists.");
            }
            currentUser.PhoneNumber = model.RepresentativeContactNumber;

            var company = new Company
            {
                CompanyName = model.CompanyName,
                TradeName = model.TradeName,
                CompanyStructureId = model.CompanyStructureId,
                CompanyRegistrationNumber = model.CompanyRegistrationNumber,
                LicenseNumber = model.LicenseNumber,
                ReraCertificateNumber = model.ReraCertificateNumber,
                BusinessActivityTypeId = model.BusinessActivityTypeId,
                CompanyAddress = model.CompanyAddress,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                WebsiteUrl = model.WebsiteUrl,
                RepresentativeId = currentUser.Id,
                RepresentativePosition = model.RepresentativePosition,
                CompanyRegistrationDoc = model.CompanyRegistrationDoc,
                TradeLicenseCopy = model.TradeLicenseCopy,
                ReraCertificateCopy = model.ReraCertificateCopy,
                TenancyContract = model.TenancyContract,
                //CompanyLogo = model.CompanyLogo,
                BusinessDescription = model.BusinessDescription,
                NumberOfEmployees = model.NumberOfEmployees,
            };

            var result = await context.companies.AddAsync(company);
            try
            {
                var userUpdateResult = await userManager.UpdateAsync(currentUser);
                await userManager.AddToRoleAsync(currentUser, Constant.CompanyAdmin);
                await context.SaveChangesAsync();
                return new CompanyRegisterResponse(true, "Company details saved for verification.", company.Id);
            }
            catch (Exception ex)
            {

                return new CompanyRegisterResponse(false, "An error occurred while saving the company details.");
            }
        }
        private async Task<User> GetUser()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                // Handle null case appropriately
                return null;
            }
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                return await userManager.FindByIdAsync(userId);
            }
            else
            {
                // Handle the case where HttpContext or User is null
                // For example, return null or throw an exception
                throw new ApplicationException("User identity not found.");
            }
        }

        public async Task<GeneralResponse> AddCompanyLogoAsync(IFormFile file, int companyId)
        {
            var result = await fileService.UploadPhotoAsync(file);

            if (result.Error != null)
            {
                return new GeneralResponse(false, "Error uploading the logo");
            }
            var company = await GetCompanyAsync(companyId);
            var logo = new CompanyFile
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            
            company.CompanyLogo= logo;
            await context.SaveChangesAsync();
            return new GeneralResponse(true, "Image Uploaded");
        }
        private async Task<Company> GetCompanyAsync(int companyId)
        {
            return await context.companies
                .Include(c => c.CompanyLogo) // Include related images if applicable
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

    }
}
