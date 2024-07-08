using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Services
{
    public interface ICompanyService
    {
        Task<bool> IsUserRegistered(string userId);
        Task<bool> IsUserValidated(string userId);
        Task<bool> IsUserCompanyAdmin(string userId);
    }
}
