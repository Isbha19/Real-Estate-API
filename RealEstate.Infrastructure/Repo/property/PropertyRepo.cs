﻿using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RealEstate.Application.Contracts.property;
using RealEstate.Application.DTOs.Request.Property;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Property;
using RealEstate.Application.Helpers;
using RealEstate.Domain.Entities.CompanyEntity;
using RealEstate.Domain.Entities.PropertyEntity;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Services;
using System.Globalization;
using System.Management;
using FileService = RealEstate.Infrastructure.Services.FileService;

namespace RealEstate.Infrastructure.Repo.property
{
    public class PropertyRepo : IProperty
    {
        private readonly AppDbContext context;
        private readonly GetUserHelper getuserHelper;
        private readonly Services.FileService fileService;
        private readonly NotificationService notificationService;

        public PropertyRepo(AppDbContext context, GetUserHelper getuserHelper, FileService fileService,
            NotificationService notificationService)
        {
            this.context = context;
            this.getuserHelper = getuserHelper;
            this.fileService = fileService;
            this.notificationService = notificationService;
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
          .Include(p => p.Agent)
              .ThenInclude(agent => agent.ImageUrl)
              .Include(p => p.Agent)
              .ThenInclude(agent => agent.user)
          .Include(p => p.Agent)
              .ThenInclude(agent => agent.company)
                  .ThenInclude(company => company.CompanyLogo)
          .FirstOrDefaultAsync(p => p.Id == id);


            if (property == null)
                return null; // Or handle not found scenario as needed

            if (property != null)
            {
                await IncrementPropertyViewsAsync(id);
            }


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
                AgentName = property.Agent.user.FirstName + " " + property.Agent.user.LastName,
                AgentImage = property.Agent.ImageUrl?.ImageUrl, // Assuming ImageUrl is a string property in AgentImage entity
                AgentPhoneNumber = property.Agent.phoneNumber,
                AgentUserId=property.Agent.user.Id,
                AgentEmail = property.Agent.user.Email,
                AgentWhatsapp = property.Agent.whatsAppNumber,
                CompanyName = property.Agent.company.CompanyName,
                CompanyLogo = property.Agent.company.CompanyLogo?.ImageUrl, // Assuming CompanyLogo is a string property in CompanyFile entity
                AgentPropertyCounts = property.Agent.company.Agents.Count.ToString()
            };
        }
        public async Task IncrementPropertyViewsAsync(int propertyId)
        {
            var property = await context.Properties.FindAsync(propertyId);

            if (property != null)
            {
                property.PropertyViews++;
                await context.SaveChangesAsync();
            }
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
                .Where(p => p.ListingType.Name.Equals(listingType, StringComparison.OrdinalIgnoreCase)&& p.IsCompanyAdminVerified)
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
            var agent = await context.Agents
                .Include(a => a.company)
                     .ThenInclude(c => c.Representative)
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (agent == null)
            {
                return new GeneralResponse(false, "Agent not found");
            }
            var company = agent.company;
        

            if (company == null)
            {
                return new GeneralResponse(false, "Company not found");
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
                Images = new List<Image>(),
                PropertyAmenties = new List<PropertyAmenties>(), // Initialize Amenities collection
                PropertyNearByFacilities = new List<PropertyNearByFacilities>(),
                AvailabilityDate = propertyDto.AvailabilityDate,
                VirtualTourUrl = propertyDto.VirtualTourUrl,
            };
            var propertyAmenties = JsonConvert.DeserializeObject<List<string>>(propertyDto.PropertyAmenties);

            foreach (var amenityName in propertyAmenties)
            {
                var amenity = await context.Amenities.FirstOrDefaultAsync(a => a.Name == amenityName);
                if (amenity != null)
                {
                    newProperty.PropertyAmenties.Add(new PropertyAmenties { AmenityId = amenity.Id });
                }
                // Handle case where amenity does not exist or other error handling
            }

            var propertyNearByFacilities = JsonConvert.DeserializeObject<List<string>>(propertyDto.PropertyNearByFacilities);

            foreach (var facilityName in propertyNearByFacilities)
            {
                var facility = await context.Facilities.FirstOrDefaultAsync(f => f.Name == facilityName);
                if (facility != null)
                {
                    newProperty.PropertyNearByFacilities.Add(new PropertyNearByFacilities { FacilityId = facility.Id });
                }
                // Handle case where facility does not exist or other error handling
            }

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
                var companyAdmin = company.RepresentativeId;
                var message = $"🏠 A new property '{newProperty.PropertyTitle}' has been added by agent '{agent.user.FirstName} {agent.user.LastName}' and needs verification. Please review it as soon as possible. 👍";
                var url = "/company-dashboard/properties/unverified-properties";

                await notificationService.NotifyUserAsync(companyAdmin, message, url);

                return new GeneralResponse(true, "Property Details Saved Successfully, It will be posted once you company representative verifies it");
              
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

    
        //verified properties
        public async Task<IEnumerable<PropertyDetailForDashboardDto>> GetCompanyPropertiesAsync()
        {
            var user = await getuserHelper.GetUser();
            var representativeId = user.Id;

            if (string.IsNullOrEmpty(representativeId))
            {
                return Enumerable.Empty<PropertyDetailForDashboardDto>(); // Handle no representative ID scenario
            }

            // Step 2: Find the company associated with the representativeId
            var company = await context.companies
          .Include(c => c.Agents)
              .ThenInclude(a => a.Properties)
                  .ThenInclude(p => p.PropertyType)
          .Include(c => c.Agents)
              .ThenInclude(a => a.Properties)
                  .ThenInclude(p => p.ListingType)
          .Include(c => c.Agents)
              .ThenInclude(a => a.Properties)
                  .ThenInclude(p => p.Images)
          .Include(c => c.Agents)
              .ThenInclude(a => a.user) // Assuming Agent has a navigation property 'User'
          .FirstOrDefaultAsync(c => c.RepresentativeId == representativeId);

            if (company == null)
            {
                return Enumerable.Empty<PropertyDetailForDashboardDto>(); // Handle company not found scenario
            }

            //    // Step 3: Retrieve all properties associated with the agents of the found company
            var properties = company.Agents
    .Where(a => a.isCompanyAdminVerified)
    .SelectMany(a => a.Properties)
    .Where(p => p.IsCompanyAdminVerified)
    .ToList();


            // Step 4: Map properties to PropertyListDto
            var propertyList = properties.Select(p => new PropertyDetailForDashboardDto
            {
                PropertyId = p.Id,
                PropertyTitle = p.PropertyTitle,
                PropertyType = p.PropertyType.Name,
                ListingType = p.ListingType.Name,
                Location = p.Location,
                Size = p.Size,
                Price = p.Price,
                Bathrooms = p.Bathrooms,
                Bedrooms = p.Bedrooms,
                PostedOn = p.PostedOn,
                PrimaryImageUrl = p.Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl,
                AgentName = p.Agent.user.FirstName + " " + p.Agent.user.LastName, 
                PropertyViews = p.PropertyViews
            }).ToList();

            return propertyList;
        }
        //unverified properties
        public async Task<IEnumerable<PropertyDetailForDashboardDto>> GetCompanyUnVerifiedPropertiesAsync()
        {
            var user = await getuserHelper.GetUser();
            var representativeId = user.Id;

            if (string.IsNullOrEmpty(representativeId))
            {
                return Enumerable.Empty<PropertyDetailForDashboardDto>(); // Handle no representative ID scenario
            }

            // Step 2: Find the company associated with the representativeId
            var company = await context.companies
          .Include(c => c.Agents)
              .ThenInclude(a => a.Properties)
                  .ThenInclude(p => p.PropertyType)
          .Include(c => c.Agents)
              .ThenInclude(a => a.Properties)
                  .ThenInclude(p => p.ListingType)
          .Include(c => c.Agents)
              .ThenInclude(a => a.Properties)
                  .ThenInclude(p => p.Images)
          .Include(c => c.Agents)
              .ThenInclude(a => a.user) // Assuming Agent has a navigation property 'User'
          .FirstOrDefaultAsync(c => c.RepresentativeId == representativeId);

            if (company == null)
            {
                return Enumerable.Empty<PropertyDetailForDashboardDto>(); // Handle company not found scenario
            }

            //    // Step 3: Retrieve all properties associated with the agents of the found company
            var properties = company.Agents
   .SelectMany(a => a.Properties)
   .Where(p => !p.IsCompanyAdminVerified)
   .ToList();

            // Step 4: Map properties to PropertyListDto
            var propertyList = properties.Select(p => new PropertyDetailForDashboardDto
            {
                PropertyId = p.Id,
                PropertyTitle = p.PropertyTitle,
                PropertyType = p.PropertyType.Name,
                ListingType = p.ListingType.Name,
                Location = p.Location,
                Size = p.Size,
                Price = p.Price,
                Bathrooms = p.Bathrooms,
                Bedrooms = p.Bedrooms,
                PostedOn = p.PostedOn,
                PrimaryImageUrl = p.Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl,
                AgentName = p.Agent.user.FirstName + " " + p.Agent.user.LastName,
                PropertyViews = p.PropertyViews
            }).ToList();

            return propertyList;
        }
        public async Task<GeneralResponse> VerifyProperty(int propertyId)
        {
            var property = await context.Properties
        .Include(p => p.Agent)
         .ThenInclude(a => a.company)
                .ThenInclude(c => c.Subscription)
                    .ThenInclude(s => s.Plan)
        .Include(p => p.Agent)
        .ThenInclude(a => a.user)
        .FirstOrDefaultAsync(p => p.Id == propertyId);

            var company = property.Agent.company;
            if (company.Subscription.SubscriptionEndDate < DateTime.UtcNow)
            {
                return new GeneralResponse(false, "Subscription has expired");
            }

            if (company.UsedPropertyCounts >= company.Subscription.Plan.NumberOfListings)
            {
                return new GeneralResponse(false, "Property limit for the current subscription plan has been reached. You can upgrade to the next package by going to the Manage Billing section in your dashboard.");
            }
            if (property == null)
            {
                return new GeneralResponse(false, "property not found");
            }
            if (property.IsCompanyAdminVerified)
            {
                return new GeneralResponse(false, "property already verified");

            }
            property.IsCompanyAdminVerified = true;
            await context.SaveChangesAsync();
            company.UsedPropertyCounts++;
            context.companies.Update(company);
            await context.SaveChangesAsync();
            var userId = property.Agent.UserId;
            var message = "🎉 Congratulations! The property you submitted has been successfully verified by our admin team. To view your listing, please click here. 🏡✅";
            var url = $"/property-detail/{property.Id}";

            await notificationService.NotifyUserAsync(userId, message, url);

            return new GeneralResponse(true, "company verified");

        }
        public async Task<PropertyStatisticsDto> GetPropertiesStatistics(string type, int year, int month = 0)
        {
            var propertiesStatistics = new PropertyStatisticsDto
            {
                Labels = new List<string>(),
                ListedProperties = new List<int>(),
                SoldProperties = new List<int>()
            };

            var user = await getuserHelper.GetUser();

            // Get the company ID based on the representative ID of the current user
            var companyId = await context.companies
                .Where(c => c.RepresentativeId == user.Id)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            // Define the base query
            IQueryable<Property> query = context.Properties
                .Where(p => p.Agent.CompanyId == companyId) // Filter properties by the user's company
                .AsQueryable();


            if (type == "yearly")
            {
                propertiesStatistics.Labels = Enumerable.Range(2018, DateTime.Now.Year - 2018 + 1).Select(y => y.ToString()).ToList();
                foreach (var label in propertiesStatistics.Labels)
                {
                    int currentYear = int.Parse(label); 
                    propertiesStatistics.ListedProperties.Add(await query.CountAsync(p => p.PostedOn.Year == currentYear));
                    propertiesStatistics.SoldProperties.Add(await query.CountAsync(p => p.isSold && p.SoldOn.Year == currentYear));
                }
            }
            else if (type == "monthly" && year > 0)
            {
                propertiesStatistics.Labels = Enumerable.Range(1, 12).Select(m => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)).ToList();
                foreach (var label in propertiesStatistics.Labels)
                {
                    int currentMonth = DateTime.ParseExact(label, "MMMM", CultureInfo.CurrentCulture).Month; // Changed from `month` to `currentMonth`
                    propertiesStatistics.ListedProperties.Add(await query.CountAsync(p => p.PostedOn.Year == year && p.PostedOn.Month == currentMonth));
                    propertiesStatistics.SoldProperties.Add(await query.CountAsync(p => p.isSold && p.SoldOn.Year == year && p.SoldOn.Month == currentMonth));
                }
            }
            else if (type == "daily" && year > 0 && month > 0)
            {
                int daysInMonth = DateTime.DaysInMonth(year, month);
                propertiesStatistics.Labels = Enumerable.Range(1, daysInMonth).Select(d => d.ToString()).ToList();
                foreach (var label in propertiesStatistics.Labels)
                {
                    int currentDay = int.Parse(label); // Changed from `day` to `currentDay`
                    propertiesStatistics.ListedProperties.Add(await query.CountAsync(p => p.PostedOn.Year == year && p.PostedOn.Month == month && p.PostedOn.Day == currentDay));
                    propertiesStatistics.SoldProperties.Add(await query.CountAsync(p => p.isSold && p.SoldOn.Year == year && p.SoldOn.Month == month && p.SoldOn.Day == currentDay));
                }
            }

            return propertiesStatistics;
        }
        public async Task<List<TopPropertiesDto>> GetTopPropertiesAsync()
        {
            var user = await getuserHelper.GetUser();

            // Get the company ID based on the representative ID of the current user
            var companyId = await context.companies
                .Where(c => c.RepresentativeId == user.Id)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();


            return await context.Properties
                .Include(p => p.Images)
                .Include(p=>p.Agent)
               .Where(p => p.Agent.CompanyId == companyId) // Filter by AgentId and CompanyId

                .OrderByDescending(p => p.PropertyViews)
                .Take(10)
                .Select(p => new TopPropertiesDto
                {
                    propertyId = p.Id,
                    PropertyTitle = p.PropertyTitle,
                    PropertyViews = p.PropertyViews,
                    PrimaryImageUrl = p.Images.FirstOrDefault(i => i.IsPrimary).ImageUrl
                })
                .ToListAsync();
        }


        public async Task<List<PropertyListDto>> GetFilteredPropertiesAsync(PropertyFilterDto filter)
        {
            var query = context.Properties.AsQueryable();

            query = query.Include(p => p.Images);
            query = query.Include(p => p.PropertyType);
            query = query.Include(p => p.ListingType);
            query = query.Include(p => p.FurnishingType);
            query = query.Include(p => p.PropertyAmenties).ThenInclude(pa => pa.Amenity); // Include PropertyAmenties and Amenities
            query = query.Include(p => p.PropertyNearByFacilities).ThenInclude(nf => nf.Facility); // Include PropertyNearByFacilities and Facilities




            if (!string.IsNullOrEmpty(filter.Location))
            {
                var normalizedLocation = filter.Location.ToUpper(); // or ToLower()

                query = query.Where(p => p.Location.ToUpper().Contains(normalizedLocation));
            }
            if (filter.ListingType.HasValue)
            {
                query = query.Where(p => p.ListingTypeId == filter.ListingType.Value);
            }
            if (filter.PropertyType.HasValue)
            {
                query = query.Where(p => p.PropertyTypeId == filter.PropertyType.Value);
            }
            if (filter.Furnished.HasValue)
            {
                query = query.Where(p => p.FurnishingTypeId == filter.Furnished.Value);
            }
            if (filter.Bedrooms != null && filter.Bedrooms.Any())
            {
        query = query.Where(p => filter.Bedrooms.Contains(p.Bedrooms) || (filter.Bedrooms.Contains(7) && p.Bedrooms >= 7));
            }
            if (filter.Bathrooms != null && filter.Bathrooms.Any())
            {
                query = query.Where(p => filter.Bathrooms.Contains(p.Bathrooms) || (filter.Bathrooms.Contains(7) && p.Bathrooms >= 7));
            }
            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }
            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }
            if (filter.MinSize.HasValue)
            {
                query = query.Where(p => p.Size >= filter.MinSize.Value);
            }
            if (filter.MaxSize.HasValue)
            {
                query = query.Where(p => p.Size <= filter.MaxSize.Value);
            }
            if (filter.VirtualTour.HasValue && filter.VirtualTour.Value)
            {
                query = query.Where(p => !string.IsNullOrEmpty(p.VirtualTourUrl));
            }
            if (filter.Amenities != null && filter.Amenities.Any())
            {
                query = query.Where(p => filter.Amenities.All(a => p.PropertyAmenties.Any(pa => pa.Amenity.Id == a)));
            }
            if (filter.NearbyFacilities != null && filter.NearbyFacilities.Any())
            {
                query = query.Where(p => filter.NearbyFacilities.All(f => p.PropertyNearByFacilities.Any(nf => nf.Facility.Id == f)));
            }

            var properties = await query.ToListAsync();

            // Map properties to PropertyListDto
            var propertyListDtos = properties.Select(p => new PropertyListDto
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
                PrimaryImageUrl = p.Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl,
            }).ToList();

            return propertyListDtos;
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
