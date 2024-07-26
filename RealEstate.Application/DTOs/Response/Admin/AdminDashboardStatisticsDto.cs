using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Admin
{
    public class AdminDashboardStatisticsDto
    {
       
            public int TotalRegisteredUsers { get; set; }
            public int TotalCompanies { get; set; }
            public int TotalAgents { get; set; }
            public int TotalSubscriptions { get; set; }
            public decimal RevenueToday { get; set; }
            public decimal TotalRevenue { get; set; }
            public int CancelledSubscriptions { get; set; }
            public int TotalProperties { get; set; }
        

    }
}
