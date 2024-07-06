using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using Stripe;

namespace RealEstate.Infrastructure.Services
{
    public class StripeWebHookHandler
    {
        private readonly ILogger<StripeWebHookHandler> _logger;
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public StripeWebHookHandler(ILogger<StripeWebHookHandler> logger, UserManager<User> userManager,
            AppDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task HandleEventAsync(Event stripeEvent)
        {
            try
            {
                switch (stripeEvent.Type)
                {
                    case Events.CheckoutSessionCompleted:
                        await HandleCheckoutSessionCompletedEvent(stripeEvent);
                        break;
                    case Events.CustomerSubscriptionCreated:
                        await HandleCustomerSubscriptionCreatedEvent(stripeEvent);
                        break;
                    case Events.CustomerSubscriptionDeleted:
                        await HandleCustomerSubscriptionDeletedEvent(stripeEvent);
                        break;
                    case Events.CustomerSubscriptionUpdated:
                        await HandleCustomerSubscriptionUpdatedEvent(stripeEvent);
                        break;
                    default:
                        _logger.LogWarning($"Unhandled event type: {stripeEvent.Type}");
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error handling Stripe event '{stripeEvent.Type}': {ex.Message}");
            }
        }

        private async Task HandleCheckoutSessionCompletedEvent(Event stripeEvent)
        {
            var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
            var customerId = session.CustomerId;

            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(customerId);

            if (customer.Email != null)
            {
                var user = await _userManager.FindByEmailAsync(customer.Email);
                if (user != null)
                {
                    var company = await _context.companies
                        .FirstOrDefaultAsync(c => c.RepresentativeId == user.Id);
                    if (company != null)
                    {
                        company.ReraCertificateCopy = "Payment Successful";
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogWarning("Company not found for user.");
                    }
                }
                else
                {
                    _logger.LogError("No user found");
                    throw new Exception("No user found");
                }
            }
        }

        private async Task HandleCustomerSubscriptionCreatedEvent(Event stripeEvent)
        {
            var subscription = stripeEvent.Data.Object as Subscription;
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(subscription.CustomerId);

            if (customer.Email != null)
            {
                var user = await _userManager.FindByEmailAsync(customer.Email);
                if (user != null)
                {
                    var company = await _context.companies.FirstOrDefaultAsync(c => c.RepresentativeId == user.Id);
                    if (company != null)
                    {
                        //company.SubscriptionId = subscription.Id;
                        //company.SubscriptionStatus = subscription.Status;
                        //company.SubscriptionStartDate = subscription.StartDate;
                        //company.SubscriptionEndDate = subscription.CurrentPeriodEnd;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogWarning("Company not found for user.");
                    }
                }
                else
                {
                    _logger.LogError("No user found");
                    throw new Exception("No user found");
                }
            }
        }

        private async Task HandleCustomerSubscriptionDeletedEvent(Event stripeEvent)
        {
            var subscription = stripeEvent.Data.Object as Subscription;
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(subscription.CustomerId);

            if (customer.Email != null)
            {
                var user = await _userManager.FindByEmailAsync(customer.Email);
                if (user != null)
                {
                    var company = await _context.companies
                        .FirstOrDefaultAsync(c => c.RepresentativeId == user.Id);
                    if (company != null)
                    {
                        //company.SubscriptionStatus = "canceled";
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogWarning("Company not found for user.");
                    }
                }
                else
                {
                    _logger.LogError("No user found");
                    throw new Exception("No user found");
                }
            }
        }

        private async Task HandleCustomerSubscriptionUpdatedEvent(Event stripeEvent)
        {
            var subscription = stripeEvent.Data.Object as Subscription;
            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(subscription.CustomerId);

            if (customer.Email != null)
            {
                var user = await _userManager.FindByEmailAsync(customer.Email);
                if (user != null)
                {
                    var company = await _context.companies
                        .FirstOrDefaultAsync(c => c.RepresentativeId == user.Id);
                    if (company != null)
                    {
                        //company.SubscriptionStatus = subscription.Status;
                        //company.SubscriptionEndDate = subscription.CurrentPeriodEnd;
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        _logger.LogWarning("Company not found for user.");
                    }
                }
                else
                {
                    _logger.LogError("No user found");
                    throw new Exception("No user found");
                }
            }
        }
    }
}
