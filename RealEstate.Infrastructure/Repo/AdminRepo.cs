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


namespace RealEstate.Infrastructure.Repo
{
    public class AdminRepo : IAdmin
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminRepo(UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        public async Task<GetMembersResponse> GetMembers()
        {
            var members = await userManager.Users
                 .Where(x => x.UserName != Constant.AdminUserName)
                 //projection
                 .Select(member => new MemberViewDto
                 {
                     Id = member.Id,
                     UserName = member.UserName,
                     FirstName = member.FirstName,
                     LastName = member.LastName,
                     DateCreated = member.DateCreated,
                     IsLocked = userManager.IsLockedOutAsync(member).GetAwaiter().GetResult(),
                     Roles = userManager.GetRolesAsync(member).GetAwaiter().GetResult()
                 }).ToListAsync();

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
            var member = await userManager.Users
                .Where(x => x.UserName != Constant.AdminUserName && x.Id == Id)
                .Select(m => new MemberAddEditDto
                {
                    Id = m.Id,
                    UserName = m.UserName,
                    FirstName = m.FirstName,
                    LastName = m.LastName,
                    Roles = string.Join(",", userManager.GetRolesAsync(m).GetAwaiter().GetResult())
                }).FirstOrDefaultAsync();

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
                    Email=model.UserName.ToLower(),
                    EmailConfirmed = true

                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return new GeneralResponse(false, string.Join(",", result.Errors));
                }
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
            return new GeneralResponse(true, message);


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
