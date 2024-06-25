using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.Contracts;
using RealEstate.Application.DTOs.Account;
using RealEstate.Application.DTOs.Request.Account;
using RealEstate.Application.DTOs.Response.Account;
using RealEstate.Infrastructure.Repo;
using System.Security.Claims;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUser _user;

        public AccountController(IUser user)
        {
            this._user = user;
        }
        [HttpPost("login")]

        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var result=await _user.Login(loginDto);
            if (!result.Success)
            {
                // If result.flag is false, return BadRequest with the message from result.message
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("register")]

        public async Task<ActionResult> Register(RegisterDto registerDto)
        {
            var result = await _user.Register(registerDto);

            if (!result.Flag)
            {
                // If result.flag is false, return BadRequest with the message from result.message
                return BadRequest(result);
            }

            return Ok(result);
        }
        [Authorize]
        [HttpGet("refresh-user-token")]
        public async Task<ActionResult> RefreshUserToken()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("User email not found");
            }

            var response = await _user.RefreshToken(email);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        [HttpPut("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto confirmEmailDto)
        {
            var result = await _user.ConfirmEmail(confirmEmailDto);
            if (!result.Flag)
            {
                // If result.flag is false, return BadRequest with the message from result.message
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("resend-email-confirmation-link/{email}")]
        public async Task<IActionResult> ResendEmailConfirmationLink(string email)
        {
            var result = await _user.ResendEmailConfirmation(email);
            if (!result.Flag)
            {
                // If result.flag is false, return BadRequest with the message from result.message
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("forgot-username-or-password/{email}")]
        public async Task<IActionResult> ForgotUserNameOrPassword(string email)
        {
            var result = await _user.ForgotUsernameorPassword(email);
            if (!result.Flag)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await _user.ResetPassword(resetPasswordDto);
            if (!result.Flag)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("register-with-third-party")]
        public async Task<IActionResult> RegisterWithThirdParty(RegisterWithExternalDto model)
        {
            var result = await _user.RegisterWithThirdParty(model);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("login-with-third-party")]
        public async Task<IActionResult> LoginWithThirdParty(LoginWithExternalDto model)
        {
            var result = await _user.LoginWithThirdParty(model);
            if (!result.Success)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
