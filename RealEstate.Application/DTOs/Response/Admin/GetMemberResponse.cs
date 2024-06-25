

using RealEstate.Application.DTOs.Request.Admin;

namespace RealEstate.Application.DTOs.Response.Admin
{
    public class GetMemberResponse
    {
        public MemberAddEditDto Member { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }

        public GetMemberResponse(MemberAddEditDto member, string message, bool success)
        {
            Member = member;
            Message = message;
            Success = success;
        }
    }
}
