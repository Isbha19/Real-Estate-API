using RealEstate.Application.DTOs.Response.Subscription;


namespace RealEstate.Application.Contracts.Subscription
{
    public interface ISubscription
    {
        Task<List<PlanDto>> GetAllPlansAsync();
        Task<PlanDto> GetPlanByIdAsync(string id);

    }
}
