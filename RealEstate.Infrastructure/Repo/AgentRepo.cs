using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Agent;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Agent;
using RealEstate.Application.DTOs.Response.Company;
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


    }
}

