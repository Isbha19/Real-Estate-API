using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Response;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using RealEstate.Application.Extensions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using RealEstate.Infrastructure.Services;
using Stripe.BillingPortal;
using RealEstate.Application.DTOs.Response.Company;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities.CompanyEntity;
using RealEstate.Application.Helpers;

namespace RealEstate.Infrastructure.Repo
{
    public class CompanyRepo : ICompany
    {
        private readonly UserManager<User> userManager;
        private readonly AppDbContext context;
        private readonly FileService fileService;
        private readonly NotificationService _notificationService;
        private readonly ICompanyService companyService;
        private readonly GetUserHelper getUserHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CompanyRepo(UserManager<User> userManager, AppDbContext context, IHttpContextAccessor httpContextAccess,
            FileService fileService,
            NotificationService notificationService,
            ICompanyService companyService,GetUserHelper getUserHelper)
        {
            this.userManager = userManager;
            this.context = context;
            this.fileService = fileService;
            this._notificationService = notificationService;
            this.companyService = companyService;
            this.getUserHelper = getUserHelper;
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
                //var userUpdateResult = await userManager.UpdateAsync(currentUser);
                //await userManager.AddToRoleAsync(currentUser, Constant.CompanyAdmin);
                await context.SaveChangesAsync();
                return new CompanyRegisterResponse(true, "Company details saved for verification. You will be notified once the admin verifies your company.", company.Id);
            }
            catch (Exception ex)
            {

                return new CompanyRegisterResponse(false, "An error occurred while saving the company details.");
            }
        }
        public async Task<SubscriptionPackageDto> GetSubscriptionPackage()
        {
            var currentUser = await getUserHelper.GetUser();
           

            var company = await context.companies
                .Include(c => c.Subscription)
                .FirstOrDefaultAsync(c => c.RepresentativeId == currentUser.Id);


            var plan = await context.Plan.FindAsync(company.Subscription.PlanId);
           
            var subscriptionPackageDto = new SubscriptionPackageDto
            {
                PlanName = plan.Name,
                SubscriptionStartDate = (DateTime)company.Subscription.SubscriptionStartDate,
                SubscriptionEndDate = (DateTime)company.Subscription.SubscriptionEndDate,
                IsActive = company.Subscription.IsActive
            };

            return subscriptionPackageDto;
        }
        public async Task<string> CreateCustomerPortalSession(string customerId)
        {
            var options = new SessionCreateOptions
            {
                Customer = customerId, 
                ReturnUrl = "https://localhost:4200/company-dashboard"
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options); 

            // session.Url contains the customer portal URL
            // Return the URL to the frontend
            return session.Url;
        }

        public async Task<CompanyStripeDto> GetCurrentUserStripeCustomerIdAsync()
        {
            var currentUser = await getUserHelper.GetUser();
            if (currentUser == null)
            {
                throw new ApplicationException("Current user not found.");
            }

            var company = await context.companies
                .Include(c => c.Subscription)
                .FirstOrDefaultAsync(c => c.RepresentativeId == currentUser.Id);

            if (company?.Subscription != null)
            {
                return new CompanyStripeDto
                {
                    CompanyName = company.CompanyName, // Assuming there's a property like this in your Company entity
                    StripeCustomerId = company.Subscription.StripeCustomerId
                };
            }

            return null; // Handle scenario where company or subscription is not found
        }

        public async Task<IEnumerable<CompanyDetailsDto>> GetVerifiedCompaniesDetailsAsync()
        {
            var verifiedCompanies = await context.companies
                .Where(c => c.isAdminVerified)
                .Include(c => c.CompanyStructure)
                .Include(c => c.BusinessActivityType)
                .Include(c => c.Representative)
                .Include(c => c.CompanyLogo)
                .ToListAsync();

            var companyDetailsList = verifiedCompanies.Select(company => new CompanyDetailsDto
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                CompanyStructure = company.CompanyStructure.Name,
                CompanyRegistrationNumber = company.CompanyRegistrationNumber,
                LicenseNumber = company.LicenseNumber,
                ReraCertificateNumber = company.ReraCertificateNumber,
                BusinessActivity = company.BusinessActivityType.Name,
                CompanyAddress = company.CompanyAddress,
                PhoneNumber = company.PhoneNumber,
                EmailAddress = company.EmailAddress,
                WebsiteUrl = company.WebsiteUrl,
                RepresentativeName = company.Representative.FirstName,
                RepresentativeEmail = company.Representative.Email,
                RepresentativePosition = company.RepresentativePosition,
                RepresentativeContactNumber = company.RepresentativeContactNumber,
                CompanyRegistrationDoc = company.CompanyRegistrationDoc,
                TradeLicenseCopy = company.TradeLicenseCopy,
                ReraCertificateCopy = company.ReraCertificateCopy,
                TenancyContract = company.TenancyContract,
                CompanyLogo = company.CompanyLogo?.ImageUrl,
                BusinessDescription = company.BusinessDescription,
                NumberOfEmployees = company.NumberOfEmployees
            }).ToList();

            return companyDetailsList;
        }
        public async Task<GeneralResponse> VerifyCompany(int companyId)
        {
            var company = await context.companies.FindAsync(companyId);
            if (company == null)
            {
                return new GeneralResponse(false, "company not found");
            }
            if (company.isAdminVerified)
            {
                return new GeneralResponse(false, "company already verified");

            }
            company.isAdminVerified = true;
            await context.SaveChangesAsync();

            var userId = company.RepresentativeId;
            var message = "Congratulations!🎉 Your company is verified. Choose your subscription package to list properties.";
            var url = "/subscription-package";
        
                await _notificationService.NotifyUserAsync(userId, message, url);
           
            return new GeneralResponse(true, "company verified");

        }
      
        public async Task<DashboardStatisticsDto> GetCompanyDashboardStatitics()
        {
            var user = await GetUser();
            string userId = user.Id;

            // Fetch company details for the authorized user
            var company = await context.companies
                .Include(c => c.Subscription)
                .ThenInclude(c => c.Plan)
                .Include(c => c.Agents)
                .ThenInclude(a => a.Properties)
                .FirstOrDefaultAsync(c => c.RepresentativeId == userId && c.isAdminVerified);

            if (company == null)
            {
                // Handle the case where the company is not found
                return null;
            }

            // Calculate the number of properties listed by agents under the given company
            var propertiesListedCount = company.Agents.SelectMany(a => a.Properties).Count();

            // Calculate the sum of all property views for properties under the given company
            var propertyViews = company.Agents.SelectMany(a => a.Properties).Sum(p => p.PropertyViews);
            var propertiesSoldCount = company.Agents.SelectMany(a => a.Properties).Count(p => p.isSold);
            var totalRevenue = company.Agents.SelectMany(a => a.Properties).Where(p => p.isSold).Sum(p => p.Revenue);



            var statisticsDto = new DashboardStatisticsDto
            {
                propertiesUsed = company.UsedPropertyCounts,
                propertyLimit = company.Subscription.Plan.NumberOfListings,
                PropertiesListedCount = propertiesListedCount,
                NumberOfAgents = company.Agents.Count,
                PropertyViews = propertyViews,
                SubscriptionAmtPaid=company.SubscriptionAmtPaid,
                PropertiesSold=propertiesSoldCount,
                TotalRevenue=totalRevenue,
            };

            return statisticsDto;
        }


        public async Task<IEnumerable<CompanyDetailsDto>> GetUnVerifiedCompaniesDetailsAsync()
        {
            var verifiedCompanies = await context.companies
                .Where(c => !c.isAdminVerified)
                .Include(c => c.CompanyStructure)
                .Include(c => c.BusinessActivityType)
                .Include(c => c.Representative)
                .Include(c => c.CompanyLogo)
                .ToListAsync();

            var companyDetailsList = verifiedCompanies.Select(company => new CompanyDetailsDto
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                CompanyStructure = company.CompanyStructure.Name,
                CompanyRegistrationNumber = company.CompanyRegistrationNumber,
                LicenseNumber = company.LicenseNumber,
                ReraCertificateNumber = company.ReraCertificateNumber,
                BusinessActivity = company.BusinessActivityType.Name,
                CompanyAddress = company.CompanyAddress,
                PhoneNumber = company.PhoneNumber,
                EmailAddress = company.EmailAddress,
                WebsiteUrl = company.WebsiteUrl,
                RepresentativeName = company.Representative.FirstName,
                RepresentativeEmail = company.Representative.Email,
                RepresentativePosition = company.RepresentativePosition,
                RepresentativeContactNumber = company.RepresentativeContactNumber,
                CompanyRegistrationDoc = company.CompanyRegistrationDoc,
                TradeLicenseCopy = company.TradeLicenseCopy,
                ReraCertificateCopy = company.ReraCertificateCopy,
                TenancyContract = company.TenancyContract,
                CompanyLogo = company.CompanyLogo?.ImageUrl,
                BusinessDescription = company.BusinessDescription,
                NumberOfEmployees = company.NumberOfEmployees
            }).ToList();

            return companyDetailsList;
        }
        public async Task<GeneralResponse> ValidateUserForPayment()
        {
            var user =await GetUser();
            if (user == null)
            {
                return new GeneralResponse(false, "user not found");
            }
            string userId = user.Id;

            var isRegistered = await companyService.IsUserRegistered(userId);
            var isValidated = await companyService.IsUserValidated(userId);
            var isCompanyAdmin = await companyService.IsUserCompanyAdmin(userId);

            if (!isRegistered)
            {
                return new GeneralResponse (false,"User has not filled the company registration form. Please fill the form and get admin verified!");
            }

            if (!isValidated)
            {
                return new GeneralResponse(false,"User has not been validated by admin.");
            }

            if (isCompanyAdmin)
            {
                return new GeneralResponse(false,"You are already a company admin!");
            }

            return new GeneralResponse(true,"User is eligible for payment.");
        }
        public async Task<CompanyDetailsDto> GetCompanyDetailByUser()
        {
            var user = await GetUser();
            string userId = user.Id;

            // Fetch company details for the authorized user
            var company = await context.companies
      .Include(c => c.CompanyStructure)
      .Include(c => c.BusinessActivityType)
      .Include(c => c.Representative)
      .Include(c => c.CompanyLogo)
      .FirstOrDefaultAsync(c => c.RepresentativeId == userId && c.isAdminVerified);
            var companyDetailsDto =new CompanyDetailsDto
            {
                CompanyId = company.Id,
                CompanyName = company.CompanyName,
                CompanyStructure = company.CompanyStructure.Name,
                CompanyRegistrationNumber = company.CompanyRegistrationNumber,
                LicenseNumber = company.LicenseNumber,
                ReraCertificateNumber = company.ReraCertificateNumber,
                BusinessActivity = company.BusinessActivityType.Name,
                CompanyAddress = company.CompanyAddress,
                PhoneNumber = company.PhoneNumber, // Assuming 'CompanyPhone' corresponds to 'PhoneNumber' in DTO
                EmailAddress = company.EmailAddress, // Assuming 'CompanyEmail' corresponds to 'EmailAddress' in DTO
                WebsiteUrl = company.WebsiteUrl,
                RepresentativeName =company.Representative.FirstName,
                RepresentativeEmail = company.Representative.Email,
                RepresentativePosition = company.RepresentativePosition,
                RepresentativeContactNumber = company.RepresentativeContactNumber,
                CompanyRegistrationDoc = company.CompanyRegistrationDoc,
                TradeLicenseCopy = company.TradeLicenseCopy,
                ReraCertificateCopy = company.ReraCertificateCopy,
                TenancyContract = company.TenancyContract,
                CompanyLogo = company.CompanyLogo.ImageUrl,
                BusinessDescription = company.BusinessDescription,
                NumberOfEmployees = company.NumberOfEmployees
            };
            return companyDetailsDto;
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
        public async Task<IEnumerable<CompanyNames>> GetCompanyNamesAsync()
        {
            var companies = await context.companies
                       .Select(c => new CompanyNames
                       {
                           CompanyId = c.Id,
                           CompanyName = c.CompanyName
                       })
                       .ToListAsync();

            return companies;
        }


        private async Task<Company> GetCompanyAsync(int companyId)
        {
            return await context.companies
                .Include(c => c.CompanyLogo) // Include related images if applicable
                .FirstOrDefaultAsync(c => c.Id == companyId);
        }

      
    }
}
