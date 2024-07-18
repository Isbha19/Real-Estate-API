using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Request.Agent
{
    public class MarkPropertyAsSoldDto
    {
        public Decimal Revenue { get; set; }
        public string SoldToUserId { get; set; }
        public int PropertyId { get; set; }
    }
}
