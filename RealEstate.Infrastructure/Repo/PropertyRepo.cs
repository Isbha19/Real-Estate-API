using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Property;
using RealEstate.Application.DTOs.Response.Property;
using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Entities.Property.Property;
using RealEstate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Repo
{
    public class PropertyRepo : IProperty
    {
        private readonly AppDbContext context;

        public PropertyRepo(AppDbContext context)
        {
            this.context = context;
        }
      
      
        public async Task<AddPropertyResponse> AddProperty(propertyDto propertyDto)
        {
            var newProperty = new Property
            {
                PropertyTitle = propertyDto.PropertyTitle,
                PropertyDescription = propertyDto.PropertyDescription,
                PropertyTypeId = propertyDto.PropertyTypeId,
                ListingTypeId = propertyDto.ListingTypeId,
                Price = propertyDto.Price,
                Location = propertyDto.Location,
                Bedrooms = propertyDto.Bedrooms,
                Bathrooms = propertyDto.Bathrooms,
                Size = propertyDto.Size,
                FurnishingTypeId = propertyDto.FurnishingTypeId
            };
            newProperty.AgentId = "5764cc3b-2c2f-4601-b3f5-653388c6684a";
            //newProperty.LastUpdatedBy = "5764cc3b-2c2f-4601-b3f5-653388c6684a";
        
            try
            {
                // Adding and saving the entity
                await context.Properties.AddAsync(newProperty);
                await context.SaveChangesAsync();
                return new AddPropertyResponse(true, "Property Added Successfully");
            }
            catch (DbUpdateException ex)
            {
                // Handle specific database exceptions (e.g., constraint violations)
                return new AddPropertyResponse(false, $"Failed to add property: {ex.InnerException}");
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                return new AddPropertyResponse(false, $"An error occurred: {ex.Message}");
            }
        }

        public void AddPropertyPhoto(IFormFile file, int propertyId)
        {
            throw new NotImplementedException();
        }

        public void DeleteProperty(int Id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Amenity>> GetAmenitiesAsync()
        {
            return await context.Amenities.ToListAsync();
        }

        public async Task<IEnumerable<FurnishingType>> GetFurnishingTypesAsync()
        {
            return await context.FurnishingTypes.ToListAsync();
        }

        public async Task<IEnumerable<ListingType>> GetListingTypesAsync()
        {
            return await context.ListingTypes.ToListAsync();
        }

        public async Task<IEnumerable<Facility>> GetNearbyFacilitiesAsync()
        {
            return await context.Facilities.ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetPropertiesAsync()
        {
            var properties = await context.Properties.ToListAsync();
            return properties;
        }

        public async Task<Property> GetPropertyAsync(int id)
        {
            var property = await context.Properties
            .Include(p => p.PropertyTitle) 
            .Include(p => p.PropertyDescription)
            .Include(p => p.FurnishingType)
            .FirstOrDefaultAsync(p => p.Id == id);

            return property;
        }

        public async Task<IEnumerable<PropertyType>> GetPropertyTypesAsync()
        {
            return await context.PropertyTypes.ToListAsync();   
        }

        
    }
}
