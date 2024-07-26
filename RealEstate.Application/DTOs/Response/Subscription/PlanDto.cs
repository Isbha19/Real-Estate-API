using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Subscription
{
    public class PlanDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int NumberOfListings { get; set; }
        public string PaymentLink { get; set; }
        public List<string> Descriptions { get; set; }
    }
}
