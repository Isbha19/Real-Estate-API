using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities.CompanyEntity
{
    public class CompanyFile
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public int CompanyId { get; set; }
        public string PublicId { get; set; }
        public  Company Company { get; set; }
    }
}
