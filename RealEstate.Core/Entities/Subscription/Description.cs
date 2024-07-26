using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.SubscriptionEntity
{
    public class Description
    {
        public int Id { get; set; } // Unique identifier for the description
        public string Text { get; set; } // The actual description text
    }
}
