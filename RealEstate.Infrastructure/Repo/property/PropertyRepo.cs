using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.DTOs.Request.Property;
using RealEstate.Application.DTOs.Response;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Application.DTOs.Response.Property;
using RealEstate.Domain.Entities.PropertyEntity;
using RealEstate.Application.Contracts.property;
using RealEstate.Application.Helpers;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using System.Management;
using Stripe;
using FileService = RealEstate.Infrastructure.Services.FileService;

namespace RealEstate.Infrastructure.Repo.property
{
    public class PropertyRepo : IProperty
    {
        private readonly AppDbContext context;
        private readonly GetUserHelper getuserHelper;
        private readonly Services.FileService fileService;

        public PropertyRepo(AppDbContext context,GetUserHelper getuserHelper,FileService fileService)
        {
            this.context = context;
            this.getuserHelper = getuserHelper;
            this.fileService = fileService;
        }
        public async Task<PropertyDetailDto> GetPropertyById(int id)
        {
            var property = await context.Properties
          .Include(p => p.PropertyType)
          .Include(p => p.ListingType)
          .Include(p => p.FurnishingType)
          .Include(p => p.Images)
          .Include(p => p.PropertyAmenties)
              .ThenInclude(pa => pa.Amenity)
          .Include(p => p.PropertyNearByFacilities)
              .ThenInclude(nf => nf.Facility)
          //.Include(p => p.agent)
          //    .ThenInclude(agent => agent.ImageUrl)
          //.Include(p => p.agent)
          //    .ThenInclude(agent => agent.company)
          //        .ThenInclude(company => company.CompanyLogo)
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
                }).ToList(),
                AvailableFrom = property.AvailabilityDate,
                Amenities = property.PropertyAmenties.Select(pa => pa.Amenity.Name).ToList(),
                NearByFacilities = property.PropertyNearByFacilities.Select(nf => nf.Facility.Name).ToList(),
                //AgentName = property.agent.user.FirstName + " " + property.agent.user.LastName,
                //AgentImage = property.agent.ImageUrl?.ImageUrl, // Assuming ImageUrl is a string property in AgentImage entity
                //AgentPhoneNumber = property.agent.phoneNumber,
                //AgentEmail = property.agent.user.Email,
                //AgentWhatsapp = property.agent.whatsAppNumber,
                //CompanyName = property.agent.company.CompanyName,
                //CompanyLogo = property.agent.company.CompanyLogo?.ImageUrl, // Assuming CompanyLogo is a string property in CompanyFile entity
                //AgentPropertyCounts = property.agent.company.Agents.Count.ToString()
            };
        }
        public async Task<Property> GetPropertyAsync(int id)
        {
            var property = await context.Properties

            .Include(p => p.Images)
            .Include(p => p.FurnishingType)
            .FirstOrDefaultAsync(p => p.Id == id);

            return property;
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


        public async Task<GeneralResponse> AddProperty(propertyDto propertyDto)
        {
            var user = await getuserHelper.GetUser(); // Assuming getUserHelper.GetUser() is replaced with a proper service call
            var userId = user.Id;
            var agent = await context.Agents.FirstOrDefaultAsync(agent => agent.UserId == userId);

            if (agent == null)
            {
                return new GeneralResponse(false, "Agent not found");
            }

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
                FurnishingTypeId = propertyDto.FurnishingTypeId,
                AgentId = agent.Id, // Assuming AgentId is set correctly
                PostedOn = DateTime.Now,
                Images = new List<Image>()
            };

            try
            {
                // Adding and saving the entity
                await context.Properties.AddAsync(newProperty);
                await context.SaveChangesAsync();

                // Add Images to the saved Property entity
                foreach (var imageDto in propertyDto.Images)
                {
                    // Upload image to cloud and get necessary details (PublicId, ImageUrl)
                    var result = await fileService.UploadPhotoAsync(imageDto);

                    if (result == null || result.Error != null)
                    {
                        return new GeneralResponse(false, "Error uploading the image");
                    }

                    // Create new Image entity
                    var newImage = new Image
                    {
                        ImageUrl = result.SecureUrl?.AbsoluteUri ?? string.Empty,
                        PublicId = result.PublicId ?? string.Empty,
                        IsPrimary = newProperty.Images.Count == 0 // Set IsPrimary based on existing count
                    };

                    // Associate Image with Property
                    newImage.PropertId = newProperty.Id; 
                    newImage.Property = newProperty;

                    newProperty.Images.Add(newImage);
                }

                // Save changes to include Images
                await context.SaveChangesAsync();
                
                return new GeneralResponse(true, "Property Added Successfully");
            }
            catch (DbUpdateException ex)
            {
                // Handle specific database exceptions (e.g., constraint violations)
                return new GeneralResponse(false, $"Failed to add property: {ex.InnerException?.Message}");
            }
            catch (Exception ex)
            {
                // Handle other unexpected exceptions
                return new GeneralResponse(false, $"An error occurred: {ex.Message}");
            }
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
        public async Task<IEnumerable<PropertyType>> GetPropertyTypesAsync()
        {
            return await context.PropertyTypes.ToListAsync();
        }

        public async Task<IEnumerable<ListingType>> GetListingTypesAsync()
        {
            return await context.ListingTypes.ToListAsync();
        }

        public async Task<IEnumerable<Facility>> GetNearbyFacilitiesAsync()
        {
            return await context.Facilities.ToListAsync();
        }


    }
}
