
using RealEstate.Application.DTOs.Request.Admin;
using RealEstate.Application.DTOs.Response.Admin;

namespace RealEstate.Application.Contracts
{
    public interface IAdmin
    {
        Task<GetMembersResponse> GetMembers();
        Task<GetMemberResponse> GetMember(string userId);
        Task<GeneralResponse> AddEditMember(MemberAddEditDto memberAddEditDto);
        Task<AdminDashboardStatisticsDto> GetDashboardStatisticsAsync();
        Task<UserRoleStatisticsDto> GetUserRoleStatisticsAsync();
        Task<List<TopPerformingCompany>> GetTopPerformingCompaniesAsync();


        Task<GeneralResponse> LockMember(string userId);
        Task<GeneralResponse> UnLockMember(string userId);
        Task<GeneralResponse> DeleteMember(string userId);
        Task<GetRolesResponse> GetApplicationRoles();

    }
}
