using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Request.Property;
using RealEstate.Infrastructure.Services;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class CompanyController : ControllerBase
    {
        private readonly ICompany company;

        public CompanyController(ICompany company)
        {
            this.company = company;
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
    }
}
