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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RealEstate.Infrastructure.Repo
{
    public class AgentRepo : IAgent
        
    {
        private readonly FileService imageUploadService;
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;
        private readonly GetUserHelper getuserHelper;

        public AgentRepo(FileService imageUploadService,AppDbContext context, UserManager<User> userManager,GetUserHelper getuserHelper)
        {
            this.imageUploadService = imageUploadService;
            this.context = context;
            this.userManager = userManager;
            this.getuserHelper = getuserHelper;
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

            context.AgentImage.Add(agentImage);
            await context.SaveChangesAsync();
         
            return new GeneralResponse(true, "Registration successful! You can list properties once your company verifies you. We will notify you upon verification.");
            
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

