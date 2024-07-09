using RealEstate.Application.DTOs.Request.Property;

using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Property;
using RealEstate.Domain.Entities.PropertyEntity;


namespace RealEstate.Application.Contracts.property
{
    public interface IProperty
    {
        Task<PropertyDetailDto> GetPropertyById(int propertyId);

        Task<IEnumerable<PropertyListDto>> GetPropertiesByListingTypeAsync(string listingType);
        Task<Property> GetPropertyAsync(int propertyId);

        Task<GeneralResponse> AddProperty(propertyDto propertyDto);
        void DeleteProperty(int propertyId);
        Task<IEnumerable<PropertyType>> GetPropertyTypesAsync();
        Task<IEnumerable<FurnishingType>> GetFurnishingTypesAsync();
        Task<IEnumerable<ListingType>> GetListingTypesAsync();
        Task<IEnumerable<Facility>> GetNearbyFacilitiesAsync();

        Task<IEnumerable<Amenity>> GetAmenitiesAsync();



    }
}
