using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.Property
{
    public class FurnishingType
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
