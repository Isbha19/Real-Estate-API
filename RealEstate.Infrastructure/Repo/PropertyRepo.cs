using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Property;
using RealEstate.Application.DTOs.Response;
using RealEstate.Domain.Entities.Property;
using RealEstate.Domain.Entities.Property.Property;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Services;
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
        private readonly FileService fileService;

        public PropertyRepo(AppDbContext context, FileService fileService)
        {
            this.context = context;
            this.fileService = fileService;
        }


        public async Task<GeneralResponse> AddProperty(propertyDto propertyDto)
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
                return new GeneralResponse(true, "Property Added Successfully");
            }
            catch (DbUpdateException ex)
            {
                // Handle specific database exceptions (e.g., constraint violations)
                return new GeneralResponse(false, $"Failed to add property: {ex.InnerException}");
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                return new GeneralResponse(false, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<GeneralResponse> AddPropertyPhoto(IFormFile file, int propertyId)
        {
            var result = await fileService.UploadPhotoAsync(file);

            if (result.Error != null)
            {
                return new GeneralResponse(false, "Error uploading the image");
            }
            var property = await GetPropertyAsync(propertyId);
            var image = new Image
            {
                ImageUrl = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if (property.Images.Count == 0)
            {
                image.IsPrimary = true;
            }
            property.Images.Add(image);
            await context.SaveChangesAsync();
            return new GeneralResponse(true, "Image Uploaded");

        }
        public async Task<PropertyDetailDto> GetPropertyById(int id)
        {
            var property = await context.Properties
                 .Include(p => p.FurnishingType) // Include FurnishingType
                 .Include(p => p.PropertyType)   // Include PropertyType
                 .Include(p => p.ListingType)
                         .Include(p => p.Images) // Include Images

                 .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
                return null; // Or handle not found scenario as needed

            return new PropertyDetailDto
            {
                Id = property.Id,
                PropertyTitle = property.PropertyTitle,
                PropertyDescription = property.PropertyDescription,
                PropertyType = property.PropertyType.Name,
                ListingType = property.ListingType.Name,
                Location = property.Location,
                Size = property.Size,
                Price = property.Price,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                FurnishingType = property.FurnishingType.Name,
                Images = property.Images.Select(i => new ImageDto
                {
                    ImageUrl = i.ImageUrl,
                    PublicId = i.PublicId,
                    IsPrimary = i.IsPrimary
                }).ToList()


            };
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



        public async Task<IEnumerable<PropertyListDto>> GetPropertiesByListingTypeAsync(string listingType)
        {
            var properties = await context.Properties
       .Include(p => p.PropertyType)
       .Include(p => p.ListingType)
              .Include(p => p.Images)

       .ToListAsync();

            return properties
                .Where(p => p.ListingType.Name.Equals(listingType, StringComparison.OrdinalIgnoreCase))
                .Select(p => new PropertyListDto
                {
                    Id = p.Id,
                    PropertyTitle = p.PropertyTitle,
                    PropertyType = p.PropertyType.Name,
                    ListingType = p.ListingType.Name,
                    Location = p.Location,
                    size = p.Size,
                    price = p.Price,
                    Bathrooms = p.Bathrooms,
                    Bedrooms = p.Bedrooms,
                    ListedDate = p.PostedOn,
                    PrimaryImageUrl = p.Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl

                })
                .ToList();
        }

        public async Task<Property> GetPropertyAsync(int id)
        {
            var property = await context.Properties

            .Include(p => p.Images)
            .Include(p => p.FurnishingType)
            .FirstOrDefaultAsync(p => p.Id == id);

            return property;
        }

        public async Task<IEnumerable<PropertyType>> GetPropertyTypesAsync()
        {
            return await context.PropertyTypes.ToListAsync();
        }

        public async Task<GeneralResponse> SetPrimaryPhoto(int propertyId, string photoPublicId)
        {
            var propertyDetailDto = await GetPropertyById(propertyId);
            if (propertyDetailDto == null)
            {
                return new GeneralResponse(false, "No such property exist");
            }
            //if (property.agent != userId)
            //{
            //    return new GeneralResponse(false, "you are not authorized to change the photo");
            //}
            var property = await context.Properties
                  .Include(p => p.Images)
                  .FirstOrDefaultAsync(p => p.Id == propertyId);

            if (property == null)
            {
                return new GeneralResponse(false, "No such property exists");
            }
            var photo = property.Images.FirstOrDefault(p => p.PublicId == photoPublicId);
            if (photo == null)
            {
                return new GeneralResponse(false, "Image does not exist");
            }
            if (photo.IsPrimary)
            {
                return new GeneralResponse(false, "This is already a primary photo");
            }

            var currentPrimary = property.Images.FirstOrDefault(p => p.IsPrimary);
            if (currentPrimary != null)
            {
                currentPrimary.IsPrimary = false;
            }

            photo.IsPrimary = true;

            // Ensure the context tracks changes
            context.Properties.Update(property);
            await context.SaveChangesAsync();

            return new GeneralResponse(true);


        }
        public async Task<GeneralResponse> DeletePhoto(int propertyId, string photoPublicId)
        {
            try
            {
                var propertyDetailDto = await GetPropertyById(propertyId);
                if (propertyDetailDto == null)
                {
                    return new GeneralResponse(false, "No such property exists");
                }
                // Uncomment and modify the authorization check if needed
                // if (property.AgentId != userId)
                // {
                //     return new GeneralResponse(false, "You are not authorized to delete the photo");
                // }

                var property = await context.Properties
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == propertyId);

                if (property == null)
                {
                    return new GeneralResponse(false, "No such property exists");
                }

                var photo = property.Images.FirstOrDefault(p => p.PublicId == photoPublicId);
                if (photo == null)
                {
                    return new GeneralResponse(false, "Image does not exist");
                }
                if (photo.IsPrimary)
                {
                    return new GeneralResponse(false, "You cannot delete the primary photo");
                }
                var result = await fileService.DeletePhotoAsync(photoPublicId);
                if (result.Error != null)
                {
                    return new GeneralResponse(false,result.Error.Message);

                }
                property.Images.Remove(photo);

                // Ensure the context tracks changes
                context.Properties.Update(property);
                await context.SaveChangesAsync();

                return new GeneralResponse(true);
            }
            catch (Exception ex)
            {
                // Return an error response with the exception message
                return new GeneralResponse(false, $"Failed to delete the photo: {ex.Message}");
            }
        }
    }
}
