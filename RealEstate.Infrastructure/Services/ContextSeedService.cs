using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Extensions;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using System.Security.Claims;


namespace RealEstate.Infrastructure.Services
{
    public class ContextSeedService : IContextSeedService
    {
        private readonly AppDbContext context;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public ContextSeedService(AppDbContext context,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task InitializeContextAsync()
        {
            if (context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0)
            {
                //applies any pending migration into our database
                await context.Database.MigrateAsync();
            }
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole { Name = Application.Extensions.Constant.User });
                await roleManager.CreateAsync(new IdentityRole { Name = Application.Extensions.Constant.Admin });
                await roleManager.CreateAsync(new IdentityRole { Name = Application.Extensions.Constant.CompanyAdmin });
                await roleManager.CreateAsync(new IdentityRole { Name = Application.Extensions.Constant.Agent });
            }
            if (!await userManager.Users.AnyAsync())
            {
                var user = new User
                {
                    FirstName = "user",
                    LastName = "Ishani",
                    UserName = "user@example.com",
                    Email = "user@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, "123456");
                await userManager.AddToRoleAsync(user, Constant.User);
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));
                await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, user.LastName));
                var admin = new User
                {
                    FirstName = "Admin",
                    LastName = "Isbha",
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "123456");
                await userManager.AddToRoleAsync(admin, Constant.Admin);
                await userManager.AddClaimAsync(admin, new Claim(ClaimTypes.Email, admin.Email));
                await userManager.AddClaimAsync(admin, new Claim(ClaimTypes.Surname, admin.LastName));
                var companyAdmin = new User
                {
                    FirstName = "companyAdmin",
                    LastName = "Shana",
                    UserName = "companyAdmin@example.com",
                    Email = "companyAdmin@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(companyAdmin, "123456");
                await userManager.AddToRoleAsync(companyAdmin, Constant.CompanyAdmin);
                await userManager.AddClaimAsync(companyAdmin, new Claim(ClaimTypes.Email, companyAdmin.Email));
                await userManager.AddClaimAsync(companyAdmin, new Claim(ClaimTypes.Surname, companyAdmin.LastName));
                var agent = new User
                {
                    FirstName = "agent",
                    LastName = "Krishna",
                    UserName = "agent@example.com",
                    Email = "agent@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(agent, "123456");
                await userManager.AddToRoleAsync(agent, Constant.Agent);
                await userManager.AddClaimAsync(agent, new Claim(ClaimTypes.Email, agent.Email));
                await userManager.AddClaimAsync(agent, new Claim(ClaimTypes.Surname, agent.LastName));

            }
        }
    }
}
