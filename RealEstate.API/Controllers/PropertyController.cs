using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Account;
using RealEstate.Application.DTOs.Request.Property;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IProperty property;

        public PropertyController(IProperty property)
        {
            this.property = property;
        }
        [HttpGet("get-properties")]
        public async Task<IActionResult> GetPropertyList()
        {
            var result = await property.GetPropertiesAsync();
            return Ok(result);
        }
        [HttpGet("get-property-type")]
        public async Task<IActionResult> GetPropertyTypes()
        {
            var result = await property.GetPropertyTypesAsync();
            return Ok(result);
        }
        [HttpGet("get-furnishing-type")]
        public async Task<IActionResult> GetFurnishingTypes()
        {
            var result = await property.GetFurnishingTypesAsync();
            return Ok(result);
        }
        [HttpGet("get-listing-type")]
        public async Task<IActionResult> GetListingTypes()
        {
            var result = await property.GetListingTypesAsync();
            return Ok(result);
        }

        [HttpGet("get-amenities")]
        public async Task<IActionResult> GetAmenities()
        {
            var result = await property.GetAmenitiesAsync();
            return Ok(result);
        }
        [HttpGet("get-facilities")]
        public async Task<IActionResult> GetNearbyFacilities()
        {
            var result = await property.GetNearbyFacilitiesAsync();
            return Ok(result);
        }
        [HttpPost("add-property")]
        public async Task<IActionResult> AddProperty(propertyDto model)
        {
            var result=await property.AddProperty(model);
            return Ok(result);
        }
    }
}
