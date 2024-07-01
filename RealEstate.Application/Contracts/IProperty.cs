using RealEstate.Application.DTOs.Request.Property;

using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Entities.Property.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RealEstate.Application.DTOs.Response;


namespace RealEstate.Application.Contracts
{
    public interface IProperty
    {
        Task<IEnumerable<PropertyListDto>> GetPropertiesByListingTypeAsync(string listingType);
        Task<PropertyDetailDto> GetPropertyById(int id);

        Task<Property> GetPropertyAsync(int id);
        Task<GeneralResponse> AddPropertyPhoto(IFormFile file, int propertyId);
        Task<IEnumerable<PropertyType>> GetPropertyTypesAsync();
        Task<IEnumerable<FurnishingType>> GetFurnishingTypesAsync();
        Task<IEnumerable<ListingType>> GetListingTypesAsync();
        Task<IEnumerable<Facility>> GetNearbyFacilitiesAsync();

        Task<GeneralResponse> SetPrimaryPhoto(int propertyId, string photoPublicId);
        Task<GeneralResponse> DeletePhoto(int propertyId, string photoPublicId);


        Task<IEnumerable<Amenity>> GetAmenitiesAsync();

        Task<GeneralResponse> AddProperty(propertyDto property);
        void DeleteProperty(int Id);

    }
}
