using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.SubscriptionEntity
{
    public class PlanDescription
    {
        public string PlanId { get; set; }
        public Plan Plan { get; set; }

        public int DescriptionId { get; set; }
        public Description Description { get; set; }
    }
}
