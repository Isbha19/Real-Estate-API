using Microsoft.AspNetCore.Http;
using RealEstate.Application.DTOs.Request.Agent;
using RealEstate.Application.DTOs.Request.Company;
using RealEstate.Application.DTOs.Response;
using RealEstate.Application.DTOs.Response.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Contracts
{
    public interface IAgent
    {
        Task<GeneralResponse> RegisterAgentAsync(AgentRegisterDto agent);
        Task<IEnumerable<AgentDetailDto>> GetUnVerifiedAgentsDetailsAsync();


    }
}
