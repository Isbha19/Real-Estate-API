using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Contracts.Subscription;
using RealEstate.Application.DTOs.Response.Subscription;
using RealEstate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Infrastructure.Repo.Subscription
{
    public class Subscriptionrepo : ISubscription
    {
        private readonly AppDbContext context;

        public Subscriptionrepo(AppDbContext context)
        {
            this.context = context;
        }
        public async Task<List<PlanDto>> GetAllPlansAsync()
        {
            return await context.Plan
            .Include(p => p.PlanDescriptions)
                .ThenInclude(pd => pd.Description)
            .Select(p => new PlanDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                NumberOfListings = p.NumberOfListings,
                PaymentLink = p.PaymentLink,
                Descriptions = p.PlanDescriptions.Select(pd => pd.Description.Text).ToList()
            })
            .ToListAsync();
        }
        public async Task<PlanDto> GetPlanByIdAsync(string id)
        {
            var plan = await context.Plan
                .Include(p => p.PlanDescriptions)
                    .ThenInclude(pd => pd.Description)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
            {
                return null; // Or throw an exception if preferred
            }

            return new PlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                Price = plan.Price,
                NumberOfListings = plan.NumberOfListings,
                PaymentLink = plan.PaymentLink,
                Descriptions = plan.PlanDescriptions.Select(pd => pd.Description.Text).ToList()
            };
        }
    }
}
