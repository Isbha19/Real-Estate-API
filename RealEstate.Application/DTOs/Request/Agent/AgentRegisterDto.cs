using RealEstate.Domain.Entities.AgentEntity;
using RealEstate.Domain.Entities;
using Microsoft.AspNetCore.Http;


namespace RealEstate.Application.DTOs.Request.Agent
{
    public class AgentRegisterDto
    {
        public string UserName { get; set; }
        public int phoneNumber { get; set; }
        public int whatsAppNumber { get; set; }
        public string licenseNumber { get; set; }

        public string Nationality { get; set; }
        public string LanguagesKnown { get; set; }
        public string Specialization { get; set; }
        public int CompanyId { get; set; }
        public IFormFile AgentImage {  get; set; }  
        public string About { get; set; }
        public int yearsOfExperience { get; set; }
    }
}
