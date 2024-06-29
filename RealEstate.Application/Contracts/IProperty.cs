using RealEstate.Application.DTOs.Request.Property;
using RealEstate.Application.DTOs.Response.Property;
using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Entities.Property.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace RealEstate.Application.Contracts
{
    public interface IProperty
    {
        Task<IEnumerable<Property>> GetPropertiesAsync();
        Task<Property> GetPropertyAsync(int id);
        void AddPropertyPhoto(IFormFile file, int propertyId);
        Task<IEnumerable<PropertyType>> GetPropertyTypesAsync();
        Task<IEnumerable<FurnishingType>> GetFurnishingTypesAsync();
        Task<IEnumerable<ListingType>> GetListingTypesAsync();
        Task<IEnumerable<Facility>> GetNearbyFacilitiesAsync();



        Task<IEnumerable<Amenity>> GetAmenitiesAsync();

        Task<AddPropertyResponse> AddProperty(propertyDto property);
        void DeleteProperty(int Id);

    }
}
