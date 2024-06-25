
using RealEstate.Application.DTOs.Request.Admin;
using RealEstate.Application.DTOs.Response.Admin;

namespace RealEstate.Application.Contracts
{
    public interface IAdmin
    {
        Task<GetMembersResponse> GetMembers();
        Task<GetMemberResponse> GetMember(string Id);
        Task<GeneralResponse> AddEditMember(MemberAddEditDto model);

        Task<GeneralResponse> LockMember(string Id);
        Task<GeneralResponse> UnLockMember(string Id);
        Task<GeneralResponse> DeleteMember(string Id);
        Task<GetRolesResponse> GetApplicationRoles();

    }
}
