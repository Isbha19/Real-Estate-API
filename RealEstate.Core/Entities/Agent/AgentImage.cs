using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.AgentEntity
{
    public class AgentImage
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public int AgentId { get; set; }
        public Agent Agent { get; set; }

        public string PublicId { get; set; }
    }
}
