using RealEstate.Domain.Entities.SubscriptionEntity; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.SubscriptionEntity
{
    public class Plan
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int NumberOfListings { get; set; }
        public string PaymentLink { get; set; }

        public List<PlanDescription> PlanDescriptions { get; set; } // Navigation property

    }
}
