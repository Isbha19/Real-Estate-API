using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RealEstate.Domain.Entities.CompanyEntity;

namespace RealEstate.Domain.Entities.SubscriptionEntity
{
    public class Subscription
    {
        public int SubscriptionId { get; set; }
        public string StripeCustomerId { get; set; }
        public string StripeSubscriptionId { get; set; }
        public string SubscriptionStatus { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public string PlanId { get; set; }

        public Plan Plan { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
