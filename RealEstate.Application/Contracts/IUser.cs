using Microsoft.AspNetCore.Mvc;
using RealEstate.Application.DTOs.Account;
using RealEstate.Application.DTOs.Request.Account;
using RealEstate.Application.DTOs.Response.Account;

namespace RealEstate.Application.Contracts
{
    public interface IUser
    {
        Task<GeneralResponseGen<UserDto>> Login(LoginDto loginDto);
        Task<GeneralResponse> Register(RegisterDto registerDto);
        Task<GeneralResponseGen<UserDto>> RefreshToken(string email);
        Task<GeneralResponse> ConfirmEmail(ConfirmEmailDto confirmEmailDto);
        Task<GeneralResponse> ResendEmailConfirmation(string email);
        Task<GeneralResponse> ForgotUsernameorPassword(string email);
        Task<GeneralResponse> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<GeneralResponseGen<UserDto>> RegisterWithThirdParty(RegisterWithExternalDto model);
        Task<GeneralResponseGen<UserDto>> LoginWithThirdParty(LoginWithExternalDto model);

    }
}
