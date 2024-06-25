using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Request.Admin;


namespace RealEstate.API.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin admin;

        public AdminController(IAdmin admin)
        {
            this.admin = admin;
        }
        [HttpGet("get-members")]
        public async Task<IActionResult> GetMembers()
        {
            var result = await admin.GetMembers();
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Members);

        }
        [HttpGet("get-member/{id}")]
        public async Task<IActionResult> GetMember(string id)
        {
            var result = await admin.GetMember(id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result.Member);

        }
        [HttpPost("add-edit-member")]
        public async Task<IActionResult> AddEditMember(MemberAddEditDto model)
        {
            var result = await admin.AddEditMember(model);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);

        }
        [HttpPut("lock-member/{Id}")]
        public async Task<IActionResult> LockMember(string Id)
        {
            var result = await admin.LockMember(Id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("unlock-member/{Id}")]
        public async Task<IActionResult> UnLockMember(string Id)
        {
            var result = await admin.UnLockMember(Id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("delete-member/{Id}")]
        public async Task<IActionResult> DeleteMember(string Id)
        {
            var result = await admin.DeleteMember(Id);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        
             [HttpGet("get-application-roles")]
        public async Task<IActionResult> GetApplicationRoles()
        {
            var result = await admin.GetApplicationRoles();
            if (result.Success)
            {
                return Ok(result.Roles);
            }
            return BadRequest();
        }

    }
}
