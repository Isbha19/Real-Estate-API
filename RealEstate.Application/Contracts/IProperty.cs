using RealEstate.Application.DTOs.Request.Property;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RealEstate.Application.DTOs.Response;
using RealEstate.Domain.Entities.Property;


namespace RealEstate.Application.Contracts
{
    public interface IProperty
    {
        Task<PropertyDetailDto> GetPropertyById(int id);

        Task<IEnumerable<PropertyListDto>> GetPropertiesByListingTypeAsync(string listingType);
        Task<Property> GetPropertyAsync(int id);

        Task<GeneralResponse> AddProperty(propertyDto property);
        void DeleteProperty(int Id);
        Task<IEnumerable<PropertyType>> GetPropertyTypesAsync();
        Task<IEnumerable<FurnishingType>> GetFurnishingTypesAsync();
        Task<IEnumerable<ListingType>> GetListingTypesAsync();
        Task<IEnumerable<Facility>> GetNearbyFacilitiesAsync();

        Task<IEnumerable<Amenity>> GetAmenitiesAsync();



    }
}
