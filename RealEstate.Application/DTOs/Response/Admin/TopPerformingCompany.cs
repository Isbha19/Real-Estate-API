using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Admin
{
    public class TopPerformingCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Revenue { get; set; } // Total revenue from all agents' properties
        public int PropertiesCount { get; set; } // Total properties associated with the company
        public string CompanyLogo { get; set; }
    }
}
