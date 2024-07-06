using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Request.Property;
using RealEstate.Infrastructure.Services;
using Stripe;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class CompanyController : ControllerBase
    {
        private readonly ICompany company;
        private readonly IConfiguration configuration;
        private readonly StripeWebHookHandler stripeWebhookHandler;
        private readonly StripeService stripeService;
        private readonly string _webhookSecret;


        public CompanyController(ICompany company, IConfiguration configuration, StripeWebHookHandler stripeWebhookHandler, StripeService stripeService)
        {
            this.company = company;
            this.configuration = configuration;
            this.stripeWebhookHandler = stripeWebhookHandler;
            this.stripeService = stripeService;
            _webhookSecret = configuration["Stripe:WebhookSecret"];


        }
        [HttpPost("add-company")]
        public async Task<IActionResult> AddProperty(CompanyDto model)
        {
            var result = await company.RegisterCompanyAsync(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("get-company-structures")]
        public async Task<IActionResult> GetCompanyStructures()
        {
            var result = await company.GetCompanyStructuresAsync();

            return Ok(result);
        }
        [HttpGet("get-verified-companies-details")]
        public async Task<IActionResult> GetVerifiedCompanyDetails()
        {
            var result = await company.GetVerifiedCompaniesDetailsAsync();

            return Ok(result);
        }
        [HttpGet("get-unverified-companies-details")]
        public async Task<IActionResult> GetUnVerifiedCompanyDetails()
        {
            var result = await company.GetUnVerifiedCompaniesDetailsAsync();

            return Ok(result);
        }
        [HttpGet("get-business-activity-types")]
        public async Task<IActionResult> GetBusinessActivityTypes()
        {
            var result = await company.GetBusinessActivityTypesAsync();

            return Ok(result);
        }
        [HttpPost("add-company-logo")]
        public async Task<IActionResult> AddCompanyLogo(IFormFile file, int companyId)
        {
            var result = await company.AddCompanyLogoAsync(file, companyId);
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
        [HttpPost("create-customer-portal-session")]
        public async Task<IActionResult> CreateCustomerPortalSession([FromBody] CustomerPortalRequest request)
        {
            var url = await company.CreateCustomerPortalSession(request.CustomerId);
            return Ok(new { url = url });
        }


    }
}
