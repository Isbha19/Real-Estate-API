using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Helpers
{
    public class GetUserHelper
    {
        private readonly UserManager<User> userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetUserHelper(UserManager<User> userManager, IHttpContextAccessor httpContextAccess)
        {
            this.userManager = userManager;
            this._httpContextAccessor = httpContextAccess;
        }
        public async Task<User> GetUser()
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                // Handle null case appropriately
                return null;
            }
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                return await userManager.FindByIdAsync(userId);
            }
            else
            {
                // Handle the case where HttpContext or User is null
                // For example, return null or throw an exception
                throw new ApplicationException("User identity not found.");
            }
        }
    }
}
