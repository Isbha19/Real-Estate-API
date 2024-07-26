using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Agent;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Response.Agent;
using RealEstate.Domain.Entities.CompanyEntity;
using RealEstate.Infrastructure.Repo;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgentController : ControllerBase
    {
        private readonly IAgent agent;

        public AgentController(IAgent agent)
        {
            this.agent = agent;
        }
        [HttpPost("add-agent")]
        public async Task<IActionResult> AddAgent( AgentRegisterDto model)
        {
            var result = await agent.RegisterAgentAsync(model);

            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("get-unverified-agents-details")]

        public async Task<IActionResult> GetUnVerifiedAgentDetails()
        {
            var result = await agent.GetUnVerifiedAgentsDetailsAsync();

            return Ok(result);
        }
        [HttpGet("get-verified-agents-details")]

        public async Task<IActionResult> GetVerifiedAgentDetails()
        {
            var result = await agent.GetVerifiedAgentDetailsAsync();

            return Ok(result);
        }
        [HttpGet("top-agents")]
        public async Task<ActionResult<List<TopAgentDto>>> GetTopPerformingAgents()
        {
            var agents = await agent.GetTopPerformingAgentsAsync();
            return Ok(agents);
        }

        [HttpPost("mark-as-sold")]
        public async Task<IActionResult> MarkPropertyAsSoldAsync([FromBody] MarkPropertyAsSoldDto dto)
        {
            if (dto == null)
            {
                return BadRequest("Invalid property data.");
            }

            var result = await agent.MarkPropertyAsSoldAsync(dto);

            if (result.Success)
            {
                return Ok(result.Message);
            }

            return BadRequest(result.Message);
        }
    
    [HttpGet("get-properties-byAgent")]
        [Authorize]

        public async Task<IActionResult> GetPropertiesByAgent()
        {
            var result = await agent.GetPropertiesbyAgentAsync();

            return Ok(result);
        }
        [HttpPost("verify/{agentId}")]
        [Authorize]

        public async Task<IActionResult> VerifyAgent(int agentId)
        {
            var result = await agent.VerifyAgent(agentId);
            return Ok(result);

        }
    }
}
