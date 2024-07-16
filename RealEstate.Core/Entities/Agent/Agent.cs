using RealEstate.Domain.Entities.CompanyEntity;
using RealEstate.Domain.Entities.PropertyEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.AgentEntity
{
    public class Agent
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public  User user { get; set; }
        public int phoneNumber { get; set; }
        public int whatsAppNumber { get; set; }
        public string licenseNumber { get; set; }
        public bool isCompanyAdminVerified { get; set; }


        public string Nationality { get; set; }
        public string LanguagesKnown { get; set; }
        public string Specialization { get; set; }
        public  AgentImage ImageUrl { get; set; }

        public int CompanyId {  get; set; }
        public  Company company { get; set; }
        public string About { get; set; }
        public int yearsOfExperience { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public  ICollection<Property> Properties { get; set; }
    }
}
