
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Application.Helpers;

using RealEstate.Infrastructure.Data;
using RealEstate.Infrastructure.Services.Subscription;
using RealEstate.Domain.Entities.SubscriptionEntity;
using RealEstate.Application.DTOs.Response.Subscription;
using Stripe;
using RealEstate.Application.Contracts.Subscription;

namespace RealEstate.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly StripeProductsService _stripeService;
        private readonly AppDbContext context;
        private readonly GetUserHelper getUserHelper;
        private readonly ISubscription subscription;

        public SubscriptionController(StripeProductsService stripeService,AppDbContext context, GetUserHelper getUserHelper,ISubscription subscription)
        {
            _stripeService = stripeService;
            this.context = context;
            this.getUserHelper = getUserHelper;
            this.subscription = subscription;
        }
        [HttpGet("get-packages")]
        public async Task<ActionResult<List<PlanDto>>> GetPackages()
        {
            var plans = await subscription.GetAllPlansAsync();
            return Ok(plans);
        }
        [HttpGet("get-package/{id}")]
        public async Task<ActionResult<PlanDto>> GetPackageById(string id)
        {
            var plan = await subscription.GetPlanByIdAsync(id);
            if (plan == null)
            {
                return NotFound(new { Error = "Plan not found" });
            }
            return Ok(plan);
        }


        [HttpPost("create-product")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var user = await getUserHelper.GetUser();
                if (user == null)
                {
                    return Unauthorized();
                }
                var product = await _stripeService.CreateProductAsync(request.Name,request.Description);
                var price = await _stripeService.CreatePriceAsync(request.Price, "aed", product.Id);
                var paymentLink = await _stripeService.CreatePaymentLinkAsync(price.Id);

                var plan = new Domain.Entities.SubscriptionEntity.Plan
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = request.Price,
                    NumberOfListings = request.NumberOfListings,
                    PaymentLink = paymentLink
                };

                context.Plan.Add(plan);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                var newproduct = new PlanDto
                {
                    Name = product.Name,
                    Price = request.Price,
                    NumberOfListings = request.NumberOfListings,
                    PaymentLink = paymentLink

                };

                return Ok(newproduct);
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpDelete("delete-product/{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await _stripeService.DeleteProductAsync(id);

                var plan = await context.Plan.FindAsync(id);
                if (plan != null)
                {
                    context.Plan.Remove(plan);
                    await context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return Ok();
            }
            catch (System.Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new { Error = ex.Message });
            }
        }
        public class CreateProductRequest
        {
            public string Name { get; set; }
            public int Price { get; set; }
            public int NumberOfListings { get; set; }
            public string Description { get; set; }
        }
    }
}
