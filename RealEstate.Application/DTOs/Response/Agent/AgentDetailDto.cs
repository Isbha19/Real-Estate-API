using RealEstate.Domain.Entities.AgentEntity;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Agent
{
    public class AgentDetailDto
    {
        public int AgentId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }

        public int phoneNumber { get; set; }
        public int whatsAppNumber { get; set; }
        public string licenseNumber { get; set; }

        public string Nationality { get; set; }
        public string LanguagesKnown { get; set; }
        public string Specialization { get; set; }
        public string ImageUrl { get; set; }

        public string About { get; set; }
        public int yearsOfExperience { get; set; }
    }
}
