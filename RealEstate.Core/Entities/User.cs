﻿
using Microsoft.AspNetCore.Identity;
using RealEstate.Domain.Entities.AgentEntity;
using RealEstate.Domain.Entities.CompanyEntity;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Domain.Entities
{
    public class User:IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public string Provider { get; set; }
        public  ICollection<Notification> Notifications { get; set; }
        public  Agent Agent { get; set; }
        public  Company company { get; set; }
        public  ICollection<Message> MessagesSent { get; set; }
        public  ICollection<Message> MessagesRecieved { get; set; }



    }
}
