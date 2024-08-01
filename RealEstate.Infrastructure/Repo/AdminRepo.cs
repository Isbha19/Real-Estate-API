using Microsoft.AspNetCore.Identity;
using RealEstate.Application.DTOs.Request.Admin;
using RealEstate.Domain.Entities;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Response.Admin;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data;
using RealEstate.Application.DTOs.Request.Account;
using RealEstate.Application.DTOs.Account;
using RealEstate.Application.DTOs.Response.Company;
using RealEstate.Infrastructure.Data;
using RealEstate.Application.DTOs.Request.Company;


namespace RealEstate.Infrastructure.Repo
{
    public class AdminRepo : IAdmin
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext context;

        public AdminRepo(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.context = context;
        }


        public async Task<GetMembersResponse> GetMembers()
        {

            List<MemberViewDto> members = new List<MemberViewDto>();
            var users = await userManager.Users.
                Where(x => x.UserName != Constant.AdminUserName)
                .ToListAsync();


            foreach (var user in users)
            {
                var memberToAdd = new MemberViewDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateCreated = user.DateCreated,
                    IsLocked=await userManager.IsLockedOutAsync(user),
                    Roles=await userManager.GetRolesAsync(user)

                };
                members.Add(memberToAdd);   
            }
            //var members = await userManager.Users
            //     .Where(x => x.UserName != Constant.AdminUserName)
            //     //projection
            //     .Select(member => new MemberViewDto
            //     {
            //         Id = member.Id,
            //         UserName = member.UserName,
            //         FirstName = member.FirstName,
            //         LastName = member.LastName,
            //         DateCreated = member.DateCreated,
            //         IsLocked = userManager.IsLockedOutAsync(member).GetAwaiter().GetResult(),
            //         Roles = userManager.GetRolesAsync(member).GetAwaiter().GetResult()
            //     }).ToListAsync();

            var response = new GetMembersResponse
            {
                Members = members,
                Message = "Members retrieved successfully",
                Success = true
            };

            return response;
        }
        public async Task<GetMemberResponse> GetMember(string Id)
        {
            var user = await userManager.Users.
                Where(x => x.UserName != Constant.AdminUserName && x.Id == Id)
                .FirstOrDefaultAsync();
            var member = new MemberAddEditDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = string.Join(",", await userManager.GetRolesAsync(user))

            };
            //var member = await userManager.Users
            //    .Where(x => x.UserName != Constant.AdminUserName && x.Id == Id)
            //    .Select(m => new MemberAddEditDto
            //    {
            //        Id = m.Id,
            //        UserName = m.UserName,
            //        FirstName = m.FirstName,
            //        LastName = m.LastName,
            //        Roles = string.Join(",", userManager.GetRolesAsync(m).GetAwaiter().GetResult())
            //    }).FirstOrDefaultAsync();

            if (member == null)
            {
                return new GetMemberResponse(null, "Member not found", false);
            }

            return new GetMemberResponse(member, "Member retrieved successfully", true);
        }
        public async Task<GeneralResponse> LockMember(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null) return new GeneralResponse(false, "user not found");
            if (IsAdmin(Id))
            {
                return new GeneralResponse(false, Constant.SuperAdminChangeNotAllowed);
            }
            await userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(5));
            return new GeneralResponse(true);
        }

        public async Task<GeneralResponse> UnLockMember(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null) return new GeneralResponse(false, "user not found");
            if (IsAdmin(Id))
            {
                return new GeneralResponse(false, Constant.SuperAdminChangeNotAllowed);
            }

            await userManager.SetLockoutEndDateAsync(user, null);
            return new GeneralResponse(true);
        }
        public async Task<GeneralResponse> DeleteMember(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            if (user == null) return new GeneralResponse(false, "user not found");
            if (IsAdmin(Id))
            {
                return new GeneralResponse(false, Constant.SuperAdminChangeNotAllowed);
            }

            await userManager.DeleteAsync(user);
            return new GeneralResponse(true);
        }
        public async Task<GetRolesResponse> GetApplicationRoles()
        {
            var roles = await roleManager.Roles.Select(x => x.Name).ToListAsync();
            return new GetRolesResponse
            {
                Roles = roles,
                Success = true
            };
        }

        public async Task<GeneralResponse> AddEditMember(MemberAddEditDto model)
        {
            User user;
            if (string.IsNullOrEmpty(model.Id))
            {

                //add a new member
                if (string.IsNullOrEmpty(model.Password))
                {
                    return new GeneralResponse(false, "Password is required");
                }
                else if (model.Password.Length < 6)
                {
                    return new GeneralResponse(false, "Password must have atleast 6 characters");


                }
                if (await CheckEmailExistAsync(model.UserName))
                {
                    return new GeneralResponse(false, $"An existing account is using {model.UserName}, email address, please try with another email address");
                }
                user = new User
                {
                    FirstName = model.FirstName.ToLower(),
                    LastName = model.LastName.ToLower(),
                    UserName = model.UserName.ToLower(),
                    Email = model.UserName.ToLower(),
                    EmailConfirmed = true

                };


                var result = await userManager.CreateAsync(user, model.Password);
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                if (!result.Succeeded) return new GeneralResponse(false, errors);

            }
            else
            {
                if (!string.IsNullOrEmpty(model.Password))
                {
                    if (model.Password.Length < 6)
                    {
                        return new GeneralResponse(false, "Password must have atleast 6 characters");

                    }
                }

                if (IsAdmin(model.Id))
                {
                    return new GeneralResponse(false, Constant.SuperAdminChangeNotAllowed);
                }
                //edit an existing member
                user = await userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return new GeneralResponse(false, "user not found");
                }
                if (!user.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    // Check if the new email already exists
                    if (await CheckEmailExistAsync(model.UserName))
                    {
                        return new GeneralResponse(false, $"An existing account is using {model.UserName} email address, please try with another email address");
                    }
                }

                user.FirstName = model.FirstName.ToLower();
                user.LastName = model.LastName.ToLower();
                user.UserName = model.UserName.ToLower();
                user.Email = model.UserName.ToLower();


                if (!string.IsNullOrEmpty(model.Password))
                {
                    await userManager.RemovePasswordAsync(user);
                    await userManager.AddPasswordAsync(user, model.Password);
                }
            }

            var userRoles = await userManager.GetRolesAsync(user);
            //remove existing roles
            await userManager.RemoveFromRolesAsync(user, userRoles);
            foreach (var role in model.Roles.Split(",").ToArray())
            {
                var roleToAdd = await roleManager.Roles.FirstOrDefaultAsync(r => r.Name == role);
                if (roleToAdd != null)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
            var message = string.IsNullOrEmpty(model.Id)
        ? $"Member named {model.FirstName} has been created"
        : $"Member named {model.FirstName} has been updated";
            var newuser = new MemberAddEditDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Roles = string.Join(",", userRoles)
            };
            return new GeneralResponse(true, message, newuser);




        }

        public async Task<AdminDashboardStatisticsDto> GetDashboardStatisticsAsync()
        {
            var totalRegisteredUsers = await userManager.Users.CountAsync();
            var totalCompanies = await context.companies.CountAsync();
            var totalAgents = await context.Agents.CountAsync();
            var totalSubscriptions = await context.Subscriptions.CountAsync();

            // Assuming you have a field in Subscription that tracks cancellation
            //var cancelledSubscriptions = await context.Subscriptions.CountAsync(s => s.IsCancelled);

            // Get today's revenue
            var today = DateTime.UtcNow.Date;
            var revenueToday = await context.Subscriptions
                .Include(s => s.Plan)
                .Where(s => s.SubscriptionStartDate == today)
                .SumAsync(s => s.Plan.Price);
            //Assuming Amount is the revenue field

            // Get total revenue
            var totalRevenue = await context.companies.SumAsync(s => s.SubscriptionAmtPaid);

            var totalProperties = await context.Properties.CountAsync();

            return new AdminDashboardStatisticsDto
            {
                TotalRegisteredUsers = totalRegisteredUsers,
                TotalCompanies = totalCompanies,
                TotalAgents = totalAgents,
                TotalSubscriptions = totalSubscriptions,
                RevenueToday = revenueToday,
                TotalRevenue = totalRevenue,
                CancelledSubscriptions = 0,
                TotalProperties = totalProperties
            };
        }
        public async Task<List<TopPerformingCompany>> GetTopPerformingCompaniesAsync()
        {
            return await context.companies
                .Include(c => c.CompanyLogo)
                .Select(c => new
                {
                    Company = c,
                    TotalRevenue = c.Agents
                        .SelectMany(a => a.Properties)
                        .Sum(p => p.Revenue), // Assuming Property has a Revenue property
                    TotalListings = c.Agents
                        .SelectMany(a => a.Properties)
                        .Count() // Count of all properties/listings for the company's agents
                })
                .OrderByDescending(x => x.TotalRevenue) // First order by total revenue
                .ThenByDescending(x => x.TotalListings) // Then order by total listings
                .Take(10)
                .Select(x => new TopPerformingCompany
                {
                    Id = x.Company.Id,
                    Name = x.Company.CompanyName,
                    Revenue = x.TotalRevenue,
                    PropertiesCount = x.TotalListings,
                    CompanyLogo = x.Company.CompanyLogo.ImageUrl,
                })
                .ToListAsync();
        }

        public async Task<UserRoleStatisticsDto> GetUserRoleStatisticsAsync()
        {
            var totalUsers = await userManager.Users.CountAsync();
            var totalCompanyAdmins = await userManager.GetUsersInRoleAsync(Constant.CompanyAdmin);
            var totalAgents = await userManager.GetUsersInRoleAsync(Constant.Agent);
            // Calculate total number of company admins and agents
            var totalAdminsAndAgents = totalCompanyAdmins.Count + totalAgents.Count;

            // Calculate the total number of normal users
            var totalNormalUsers = totalUsers - totalAdminsAndAgents;
            return new UserRoleStatisticsDto
            {
                TotalUsers = totalUsers,
                TotalCompanyAdmins = totalCompanyAdmins.Count,
                TotalAgents = totalAgents.Count,

                TotalNormalUsers = totalNormalUsers
            };
        }


        #region private helper methods
        private bool IsAdmin(string userId)
        {
            return userManager.FindByIdAsync(userId).GetAwaiter().GetResult().UserName.Equals(Constant.AdminUserName);
        }

        private async Task<bool> CheckEmailExistAsync(string email)
        {
            return await userManager.Users.AnyAsync(x => x.Email == email.ToLower());
        }




        #endregion
    }
}
