using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Domain.Entities
{
    public class baseEntity
    {
        public int Id { get; set; }
        public DateTime LastUpdatedOn { get; set; } = DateTime.Now;
        public DateTime LastUpdatedBy { get; set; } 


    }
}
