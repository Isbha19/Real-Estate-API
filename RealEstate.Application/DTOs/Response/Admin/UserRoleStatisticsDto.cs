using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Admin
{
    public class UserRoleStatisticsDto
    {
        public int TotalUsers { get; set; }
        public int TotalCompanyAdmins { get; set; }
        public int TotalAgents { get; set; }
        public int TotalNormalUsers { get; set; }
    }
}
