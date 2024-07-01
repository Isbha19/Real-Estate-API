using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Account;
using RealEstate.Application.DTOs.Request.Property;
using RealEstate.Application.Services;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IProperty property;
        private readonly IPropertyPhotoService propertyPhotoService;

        public PropertyController(IProperty property,IPropertyPhotoService propertyPhotoService)
        {
            this.property = property;
            this.propertyPhotoService = propertyPhotoService;
        }
        [HttpGet("get-properties/{listType}")]
        public async Task<IActionResult> GetPropertyList(string listType)
        {
            var result = await property.GetPropertiesByListingTypeAsync(listType);
            return Ok(result);
        }
        [HttpGet("get-property/{id}")]
        public async Task<IActionResult> GetPropertyById(int id)
        {
            var result = await property.GetPropertyById(id);
            return Ok(result);
        }
        [Authorize(Roles = "Agent")]

        [HttpPost("add-property-photo")]
        public async Task<IActionResult> AddPropertyPhoto(IFormFile file, int propertyId)
        {
            var result = await propertyPhotoService.AddPropertyPhotoAsync(file,propertyId);
            return Ok(result);
        }
        [HttpPost("set-primary-photo/{propertyId}/{photoPublicId}")]
        public async Task<IActionResult> SetPrimaryPhoto(int propertyId, string photoPublicId)
        {
            var result = await propertyPhotoService.SetPrimaryPhotoAsync(propertyId, photoPublicId);
         
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
        [HttpDelete("delete-photo/{propertyId}/{photoPublicId}")]
        public async Task<IActionResult> deletePhoto(int propertyId, string photoPublicId)
        {
            var result = await propertyPhotoService.DeletePhotoAsync(propertyId, photoPublicId);

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
