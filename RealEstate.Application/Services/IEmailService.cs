using RealEstate.Application.DTOs.Request.Account;


namespace RealEstate.Application.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(EmailSendDto emailSend);
    }
}
