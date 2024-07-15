
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
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual Company company { get; set; }
        public virtual ICollection<Message> MessagesSent { get; set; }
        public virtual ICollection<Message> MessagesRecieved { get; set; }



    }
}
