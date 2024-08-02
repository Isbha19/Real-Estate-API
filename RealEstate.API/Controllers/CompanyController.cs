using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Request.Property;
using RealEstate.Infrastructure.Services.Subscription;
using Stripe;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CompanyController : ControllerBase
    {
        private readonly ICompany company;
        private readonly IConfiguration configuration;
        private readonly StripeWebHookHandler stripeWebhookHandler;
        private readonly string _webhookSecret;


        public CompanyController(ICompany company, IConfiguration configuration, StripeWebHookHandler stripeWebhookHandler)
        {
            this.company = company;
            this.configuration = configuration;
            this.stripeWebhookHandler = stripeWebhookHandler;
           _webhookSecret = configuration["Stripe:WebhookSecret"];


        }
        [HttpPost("add-company")]
        [Authorize]

        public async Task<IActionResult> AddCompany(CompanyDto model)
        {
            var result = await company.RegisterCompanyAsync(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("get-company-structures")]
        [Authorize]

        public async Task<IActionResult> GetCompanyStructures()
        {
            var result = await company.GetCompanyStructuresAsync();

            return Ok(result);
        }
        [HttpGet("get-company-names")]

        public async Task<IActionResult> GetCompanyNames()
        {
            var result = await company.GetCompanyNamesAsync();

            return Ok(result);
        }
        [HttpGet("get-verified-companies-details")]
        [Authorize]

        public async Task<IActionResult> GetVerifiedCompanyDetails()
        {
            var result = await company.GetVerifiedCompaniesDetailsAsync();

            return Ok(result);
        }
        [HttpGet("get-unverified-companies-details")]
        [Authorize]

        public async Task<IActionResult> GetUnVerifiedCompanyDetails()
        {
            var result = await company.GetUnVerifiedCompaniesDetailsAsync();

            return Ok(result);
        }
        [HttpGet("get-business-activity-types")]
        [Authorize]

        public async Task<IActionResult> GetBusinessActivityTypes()
        {
            var result = await company.GetBusinessActivityTypesAsync();

            return Ok(result);
        }
        [HttpPost("add-company-logo")]
        [Authorize]

        public async Task<IActionResult> AddCompanyLogo(IFormFile file, int companyId)
        {
            var result = await company.AddCompanyLogoAsync(file, companyId);
            return Ok(result);
        }

        [HttpPost("verify/{companyId}")]
        [Authorize]

        public async Task<IActionResult> VerifyCompany(int companyId)
        {
            var result = await company.VerifyCompany(companyId);
            return Ok(result);

        }
        [HttpGet("validate-user-for-payment")]
        [Authorize]

        public async Task<IActionResult> ValidateUserForPayment()
        {
            var result = await company.ValidateUserForPayment();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("Get-companyDashboard-statistics")]
        [Authorize]

        public async Task<IActionResult> GetCompanyDasboardStatistics()
        {
            var result = await company.GetCompanyDashboardStatitics();
            return Ok(result);
        }
        [HttpGet("Get-company-by-User")]
        [Authorize]

        public async Task<IActionResult> GetCompanyByUser()
        {
            var result = await company.GetCompanyDetailByUser();
            return Ok(result);
        }
        [HttpGet("get-stripe-customerId")]
        [Authorize]

        public async Task<IActionResult> GetStripeCustomerId()
        {
            var result = await company.GetCurrentUserStripeCustomerIdAsync();
            return Ok(result);
        }
        [HttpGet("get-subscription-package")]
        [Authorize]

        public async Task<IActionResult> GetSubscriptionPackage()
        {
            var result = await company.GetSubscriptionPackage();
            return Ok(result);
        }


        [HttpPost("Webhook")]
        public async Task<IActionResult> WebhookHandler()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _webhookSecret);

                // Handle the event
                await stripeWebhookHandler.HandleEventAsync(stripeEvent);

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }

        }
        [HttpPost("create-customer-portal-session/{customerId}")]
        [Authorize]

        public async Task<IActionResult> CreateCustomerPortalSession(string customerId)
        {
            var url = await company.CreateCustomerPortalSession(customerId);
            return Ok(new { url = url });
        }


    }
}
