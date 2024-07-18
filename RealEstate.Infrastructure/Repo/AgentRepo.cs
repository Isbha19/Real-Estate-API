using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Agent;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Agent;
using RealEstate.Application.DTOs.Response.Company;
using RealEstate.Application.DTOs.Response.Property;
using RealEstate.Application.Extensions;
using RealEstate.Application.Helpers;
using RealEstate.Domain.Entities;
using RealEstate.Domain.Entities.AgentEntity;
using RealEstate.Domain.Entities.CompanyEntity;
using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Services;
using System.Security.Policy;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RealEstate.Infrastructure.Repo
{
    public class AgentRepo : IAgent
        
    {
        private readonly FileService imageUploadService;
        private readonly AppDbContext context;
        private readonly UserManager<Domain.Entities.User> userManager;
        private readonly GetUserHelper getuserHelper;
        private readonly NotificationService notificationService;

        public AgentRepo(FileService imageUploadService,AppDbContext context, UserManager<User> userManager,GetUserHelper getuserHelper,NotificationService notificationService)
        {
            this.imageUploadService = imageUploadService;
            this.context = context;
            this.userManager = userManager;
            this.getuserHelper = getuserHelper;
            this.notificationService = notificationService;
        }
        public async Task<GeneralResponse> RegisterAgentAsync(AgentRegisterDto model)
        {
            var user = await getuserHelper.GetUser();
            string userId = user.Id;
            if (user == null)
            {
                return new GeneralResponse(false,"User not found.");
            }

            var userRoles = await userManager.GetRolesAsync(user);
            if (userRoles.Contains(Constant.Agent) || userRoles.Contains(Constant.CompanyAdmin))
            {
                return new GeneralResponse(false,"You are already registered as an agent or company admin.");
            }
            var existingAgent = await context.Agents.FirstOrDefaultAsync(a => a.UserId == user.Id);
            if (existingAgent != null)
            {
                return new GeneralResponse(false, "You have already filled out the agent registration form.");
            }
            // Create agent entity
            var agent = new Agent
            {
                UserId = userId,
                phoneNumber = model.phoneNumber,
                whatsAppNumber = model.whatsAppNumber,
                Nationality = model.Nationality,
                LanguagesKnown = model.LanguagesKnown,
                Specialization = model.Specialization,
                licenseNumber=model.licenseNumber,
                CompanyId = model.CompanyId,
                About = model.About,
                yearsOfExperience = model.yearsOfExperience,
               
            };

            // Save agent to database
            context.Agents.AddAsync(agent);
            await context.SaveChangesAsync();

            var uploadResult = await imageUploadService.UploadPhotoAsync(model.AgentImage);
            if (uploadResult.Error != null)
            {
                return new GeneralResponse(false, uploadResult.Error.Message);
            }
            var agentImage = new AgentImage
            {
                ImageUrl = uploadResult.Url.ToString(),
                PublicId = uploadResult.PublicId,
                AgentId = agent.Id // Set the AgentId here
            };
            var company = await context.companies.FirstOrDefaultAsync(u => u.Id == model.CompanyId);
            var companyAdminId = company?.RepresentativeId;
            var message = $"User named {user.FirstName} {user.LastName} has registered under your company. Please verify their registration to ensure they are authorized to represent your company.";
            var url = "/company-dashboard/unverified-agents";
            await notificationService.NotifyUserAsync(companyAdminId, message, url);
            context.AgentImage.Add(agentImage);
            await context.SaveChangesAsync();
         
            return new GeneralResponse(true, "Registration successful! You can list properties once your company verifies you. We will notify you upon verification.");
            
        }
        public async Task<GeneralResponse> VerifyAgent(int agentId)
        {
            var agent = await context.Agents.FindAsync(agentId);

            if (agent == null)
            {
                return new GeneralResponse(false, "Agent not found");
            }

            if (agent.isCompanyAdminVerified)
            {
                return new GeneralResponse(false, "Agent already verified");
            }

            agent.isCompanyAdminVerified = true;
            await context.SaveChangesAsync();

            var userId = agent.UserId; // Assuming agent has a UserId property
            var message = $"Congratulations!🎉 You are now a verified agent, verified by your company. You can now list properties.";
            var url = "/list-property"; // Example URL for agent's dashboard

            await notificationService.NotifyUserAsync(userId, message, url);
            var user = await userManager.FindByIdAsync(userId);

            if (!await userManager.IsInRoleAsync(user, Constant.Agent))
            {
                await userManager.AddToRoleAsync(user, Constant.Agent);
            }

            return new GeneralResponse(true, "Agent verified successfully");
        }
        public async Task<IEnumerable<AgentDetailDto>> GetVerifiedAgentDetailsAsync()
        {
            var user = await getuserHelper.GetUser();
            if (user == null)
            {
                return Enumerable.Empty<AgentDetailDto>();
            }

            var companyId = await context.companies
                .Where(c => c.RepresentativeId == user.Id)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (companyId == 0)
            {
                return Enumerable.Empty<AgentDetailDto>();
            }

            var unverifiedAgents = await context.Agents
                .Where(a => a.isCompanyAdminVerified && a.CompanyId == companyId)
                .Include(a => a.company)
                .Include(a => a.user)
                .Include(a => a.ImageUrl)
                .ToListAsync();

            var agentList = unverifiedAgents.Select(agent => new AgentDetailDto
            {
                AgentId = agent.Id,
                UserName = agent.user.FirstName,
                UserEmail = agent.user.Email,
                phoneNumber = agent.phoneNumber,
                whatsAppNumber = agent.whatsAppNumber,
                licenseNumber = agent.licenseNumber,
                Nationality = agent.Nationality,
                LanguagesKnown = agent.LanguagesKnown,
                Specialization = agent.Specialization,
                ImageUrl = agent.ImageUrl.ImageUrl,
                About = agent.About,
                yearsOfExperience = agent.yearsOfExperience
            }).ToList();

            return agentList;
        }
        public async Task<IEnumerable<AgentDetailDto>> GetUnVerifiedAgentsDetailsAsync()
        {
            var user = await getuserHelper.GetUser();
            if (user == null)
            {
                return Enumerable.Empty<AgentDetailDto>();
            }

            var companyId = await context.companies
                .Where(c => c.RepresentativeId == user.Id)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();

            if (companyId == 0)
            {
                return Enumerable.Empty<AgentDetailDto>();
            }

            var unverifiedAgents = await context.Agents
                .Where(a => !a.isCompanyAdminVerified && a.CompanyId == companyId)
                .Include(a => a.company)
                .Include(a => a.user)
                .Include(a => a.ImageUrl)
                .ToListAsync();

            var agentList = unverifiedAgents.Select(agent => new AgentDetailDto
            {
                AgentId = agent.Id,
                UserName = agent.user.FirstName,
                UserEmail = agent.user.Email,
                phoneNumber = agent.phoneNumber,
                whatsAppNumber = agent.whatsAppNumber,
                licenseNumber = agent.licenseNumber,
                Nationality = agent.Nationality,
                LanguagesKnown = agent.LanguagesKnown,
                Specialization = agent.Specialization,
                ImageUrl = agent.ImageUrl.ImageUrl,
                About = agent.About,
                yearsOfExperience = agent.yearsOfExperience
            }).ToList();

            return agentList;
        }

        public async Task<IEnumerable<PropertyDetailForDashboardDto>> GetPropertiesbyAgentAsync()
        {
            var user = await getuserHelper.GetUser();
            if (user == null)
            {
                return Enumerable.Empty<PropertyDetailForDashboardDto>();
            }

            var agent = await context.Agents
                .Include(a => a.Properties)
                .ThenInclude(p => p.PropertyType)
                .Include(a => a.Properties)
                .ThenInclude(p => p.ListingType)
                .Include(a => a.Properties)
                .ThenInclude(p => p.Images)
                .Include(a => a.Properties)
                .ThenInclude(p => p.SoldToUser)

                .FirstOrDefaultAsync(a => a.UserId == user.Id);

            if (agent == null)
            {
                return Enumerable.Empty<PropertyDetailForDashboardDto>();
            }
            var agentProperties = agent.Properties.Select(p => new PropertyDetailForDashboardDto
            {
                PropertyId = p.Id,
                PropertyTitle = p.PropertyTitle,
                PropertyType = p.PropertyType.Name,
                ListingType = p.ListingType.Name,
                Location = p.Location,
                Price = p.Price,
                Bedrooms = p.Bedrooms,
                Bathrooms = p.Bathrooms,
                Size = p.Size,
                PropertyViews = p.PropertyViews,
                AgentName = $"{agent.user.FirstName} {agent.user.LastName}", // Assuming you have a User navigation property
                PostedOn = p.PostedOn,
                PrimaryImageUrl = p.Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl,
               isVerified=p.IsCompanyAdminVerified,
               isSold=p.isSold,
                soldTo = p.SoldToUser != null ? $"{p.SoldToUser.FirstName} {p.SoldToUser.LastName}" : "Not Sold Yet",
                Revenue = p.Revenue

    });

            return agentProperties;
        }
        public async Task<GeneralResponse> MarkPropertyAsSoldAsync(MarkPropertyAsSoldDto dto)
        {
            // Retrieve the property from the database
            var property = await context.Properties.FindAsync(dto.PropertyId);
            if (property == null)
            {
                return new GeneralResponse(false, "Property not found");
            }

            // Check if the property is already marked as sold
            if (property.isSold)
            {
                return new GeneralResponse(false, "Property is already marked as sold");
            }
            var soldToUser = await userManager.FindByIdAsync(dto.SoldToUserId);
            if (soldToUser == null)
            {
                return new GeneralResponse(false, "User not found");
            }
            // Update the property details
            property.isSold = true;
            property.Revenue = dto.Revenue;
            property.SoldToUserId = dto.SoldToUserId;
            property.SoldToUser = soldToUser;
            property.SoldOn = DateTime.UtcNow; // Assuming you want to store the date of the sale

            // Save the changes to the database
            await context.SaveChangesAsync();

            // Optionally, send a notification to the user who bought the property
            var message = $"Congratulations!🎉 You have successfully purchased the property '{property.PropertyTitle}'. We value your feedback! Please take a moment to share your experience by adding a testimonial. Click here";
            var url = "/testimonial-form"; // Example URL for the property details page
            await notificationService.NotifyUserAsync(dto.SoldToUserId, message, url);
            return new GeneralResponse(true, "Property marked as sold successfully");
        }



    }
}

