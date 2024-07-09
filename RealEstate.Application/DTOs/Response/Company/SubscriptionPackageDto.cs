using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Company
{
    public class SubscriptionPackageDto
    {
       
            public string PlanName { get; set; }
            public DateTime SubscriptionStartDate { get; set; }
            public DateTime SubscriptionEndDate { get; set; }
            public bool IsActive { get; set; }

    }
}
