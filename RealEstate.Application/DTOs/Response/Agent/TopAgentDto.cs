using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.DTOs.Response.Agent
{
    public class TopAgentDto
    {
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public int PropertiesSold { get; set; }
        public int PropertyViews { get; set; }
        public string AgentImage { get; set; }
    }
}
