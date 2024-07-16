using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts.property;
using RealEstate.Application.DTOs.Request.Property;
using RealEstate.Domain.Entities.CompanyEntity;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IProperty property;
        private readonly IPropertyPhoto propertyPhoto;

        public PropertyController(IProperty property,IPropertyPhoto propertyPhoto)
        {
            this.property = property;
            this.propertyPhoto = propertyPhoto;
        }
        [HttpGet("get-properties/{listType}")]
        public async Task<IActionResult> GetPropertyList(string listType)
        {
            var result = await property.GetPropertiesByListingTypeAsync(listType);
            return Ok(result);
        }
        [HttpGet("get-property/{propertyId}")]
        public async Task<IActionResult> GetPropertyById(int propertyId)
        {
            var result = await property.GetPropertyById(propertyId);
            return Ok(result);
        }
        [HttpPost("verify/{propertyId}")]
        [Authorize]

        public async Task<IActionResult> VerifyProperty(int propertyId)
        {
            var result = await property.VerifyProperty(propertyId);
            return Ok(result);

        }

        [HttpGet("get-company-properties")]
        public async Task<IActionResult> GetCompanyProperties()
        {
            var result = await property.GetCompanyPropertiesAsync();
            return Ok(result);
        }
        [HttpGet("get-unverified-company-properties")]
        public async Task<IActionResult> GetUnverifiedCompanyProperties()
        {
            var result = await property.GetCompanyUnVerifiedPropertiesAsync();
            return Ok(result);
        }

        [HttpPost("add-property-photo")]
        public async Task<IActionResult> AddPropertyPhoto(IFormFile file, int propertyId)
        {
            var result = await propertyPhoto.AddPropertyPhotoAsync(file,propertyId);
            return Ok(result);
        }
        [HttpPost("filter-properties")]
        public async Task<IActionResult> FilterProperties([FromBody] PropertyFilterDto filter)
        {
            var result = await property.GetFilteredPropertiesAsync(filter);
            return Ok(result);
        }
        [HttpPost("set-primary-photo/{propertyId}/{photoPublicId}")]
        public async Task<IActionResult> SetPrimaryPhoto(int propertyId, string photoPublicId)
        {
            var result = await propertyPhoto.SetPrimaryPhotoAsync(propertyId, photoPublicId);
         
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
        [HttpDelete("delete-photo/{propertyId}/{photoPublicId}")]
        public async Task<IActionResult> deletePhoto(int propertyId, string photoPublicId)
        {
            var result = await propertyPhoto.DeletePhotoAsync(propertyId, photoPublicId);

            if (!result.Success)
            {
                return BadRequest(result);
            }
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
