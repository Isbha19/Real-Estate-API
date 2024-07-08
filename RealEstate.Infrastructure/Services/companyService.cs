using Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Services;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using RealEstate.Application.Extensions;


namespace RealEstate.Infrastructure.Services
{
    public class companyService : ICompanyService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> userManager;

        public companyService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }
        public async Task<bool> IsUserRegistered(string userId)
        {
            var isRegistered = await _context.companies.AnyAsync(c => c.RepresentativeId == userId);
            return isRegistered;

        }

        public async Task<bool> IsUserValidated(string userId)
        {
            var isValidated = await _context.companies.AnyAsync(c => c.RepresentativeId == userId && c.isAdminVerified);
            return isValidated;
        }

        public async Task<bool> IsUserCompanyAdmin(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var roles = await userManager.GetRolesAsync(user);
            return roles.Contains(Constant.CompanyAdmin);
        }
    }
}
