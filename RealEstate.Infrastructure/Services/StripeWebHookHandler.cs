using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RealEstate.Application.DTOs.Request.Account;
using RealEstate.Domain.Entities;
using RealEstate.Infrastructure.Data;
using Stripe;

namespace RealEstate.Infrastructure.Services
{
    public class StripeWebHookHandler
    {
        private readonly ILogger<StripeWebHookHandler> _logger;
        private readonly UserManager<User> userManager;
        private readonly AppDbContext context;

        public StripeWebHookHandler(ILogger<StripeWebHookHandler> logger, UserManager<User> userManager,
            AppDbContext context)
        {
            _logger = logger;
            this.userManager = userManager;
            this.context = context;
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
                    case Events.CustomerSubscriptionDeleted:
                        await HandleCustomerSubscriptionDeletedEvent(stripeEvent);
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
                var user = await userManager.FindByEmailAsync(customer.Email);

                if (user != null)
                {
                    var company = await context.companies
        .FirstOrDefaultAsync(c => c.RepresentativeId == user.Id);
                    if (company != null)
                    {
                        company.ReraCertificateCopy = "DONEE";
                        await context.SaveChangesAsync();

                    }
                    else
                    {
                        // Handle case where company is not found for the user
                    }
                }


                // Update user data and grant access to your product
                //user.PriceId = session.LineItems.Data[0].Price.Id;
                //user.HasAccess = true;
                //await _userService.UpdateAsync(user);
            }
            if (stripeEvent.Type == Events.CheckoutSessionCompleted)
            {
            }
            else if (stripeEvent.Type == Events.CheckoutSessionExpired)
            {
            }
            else if (stripeEvent.Type == Events.CustomerSubscriptionCreated)
            {
            }
            else if (stripeEvent.Type == Events.CustomerSubscriptionDeleted)
            {

            }
            else if (stripeEvent.Type == Events.CustomerSubscriptionPaused)
            {
            }
            else if (stripeEvent.Type == Events.CustomerSubscriptionUpdated)
            {
            }
            else
            {
                _logger.LogError("No user found");
                throw new Exception("No user found");
            }
        }

        private async Task HandleCustomerSubscriptionDeletedEvent(Event stripeEvent)
        {
            var subscription = stripeEvent.Data.Object as Stripe.Subscription;

            var customerService = new CustomerService();
            var customer = await customerService.GetAsync(subscription.CustomerId);

            if (customer.Email != null)
            {
                var user = await userManager.FindByEmailAsync(customer.Email);

                if (user != null)
                {
                    var company = await context.companies
                        .FirstOrDefaultAsync(c => c.RepresentativeId == user.Id);
                    if (company != null)
                    {
                        company.ReraCertificateCopy = "NOT DONE";
                        await context.SaveChangesAsync();
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
