using RealEstate.Application.DTOs.Request.Admin;


namespace RealEstate.Application.DTOs.Response.Admin
{
 
        public class GetMembersResponse
        {
            public IEnumerable<MemberViewDto> Members { get; set; }
            public string Message { get; set; }
            public bool Success { get; set; }
        }
    
}
